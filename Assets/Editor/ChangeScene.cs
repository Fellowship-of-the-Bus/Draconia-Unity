using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System;
using System.Threading;
using System.Reflection;
using System.Reflection.Emit;

[InitializeOnLoad]
public static class MenuItemGenerator {
  private static readonly string outputDir = "Assets/Editor/SceneSwitcher";
  private static readonly string sceneDir = "Assets/Scene";

  private static TypeBuilder typeBuilder;

  private static MethodInfo switchToSceneInfo;

  static MenuItemGenerator() {
    EditorSceneManager.sceneSaved += onSceneSaved;

    Type thisType = typeof(MenuItemGenerator);
    switchToSceneInfo = thisType.GetMethod("switchToScene");

    // ensure outputDir exists
    // TODO: import directory to silence warning
    System.IO.Directory.CreateDirectory(outputDir);

    string[] sceneGUIDs = AssetDatabase.FindAssets("t:SceneAsset", new string[] { sceneDir });
    foreach (string guid in sceneGUIDs) {
      generateMenuItem(AssetDatabase.GUIDToAssetPath(guid));
    }
    AssetDatabase.Refresh();
  }

  private static void onSceneSaved(Scene scene) {
    Debug.Log(scene.name + " // " + scene.path);
    if (scene.name == "") return;
    generateMenuItem(scene.path);
    AssetDatabase.Refresh();
  }

  private static string sanitize(string name) {
    return name.replaceAll(" \t\r\n/\\.", '_');
  }

  public static void switchToScene(string name) {
    // User pressed play -- autoload master scene.
    EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

    if (!EditorSceneManager.OpenScene(name).IsValid()) {
      Debug.LogError(string.Format("error: scene not found: {0}", name));
    }
  }

  static void generateMenuItem(string scenePath) {
    // TODO: should strip only Assets/ or Assets/Scene/
    string sceneName = scenePath.Substring(sceneDir.Length+1, scenePath.Length-".unity".Length-(sceneDir.Length+1));  // +1 for /
    string sanitizedName = sanitize(sceneName);

    AssemblyName aName = new AssemblyName(sanitizedName);
    string[] guids = AssetDatabase.FindAssets(aName.Name, new string[] { outputDir });
    if (guids.Length > 0) {
      // don't need to generate the same code more than once
      return;
    }

    Debug.Log("Generating menu item for " + scenePath);
    var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(aName, AssemblyBuilderAccess.RunAndSave, outputDir);
    var moduleBuilder = assemblyBuilder.DefineDynamicModule(aName.Name, aName.Name + ".dll");
    typeBuilder = moduleBuilder.DefineType("ChangeScene_" + sanitizedName, System.Reflection.TypeAttributes.Public);

    // add attribute [MenuItem("Open Scene/{sceneName}")]
    var attrCtorParams = new Type[] { typeof(string) };
    var attrCtorInfo = typeof(MenuItem).GetConstructor(attrCtorParams);
    var attrBuilder = new CustomAttributeBuilder(attrCtorInfo, new object[] { "Open Scene/" + sceneName });

    // define: void switchToScene_{sceneName}() {}
    MethodAttributes methodAttr = MethodAttributes.Public | MethodAttributes.Static;
    MethodBuilder methodBuilder = typeBuilder.DefineMethod("switchToScene_" + sanitizedName, methodAttr, null, Type.EmptyTypes);
    methodBuilder.SetCustomAttribute(attrBuilder);

    // generate code to call switchToScene({scenePath})
    ILGenerator il = methodBuilder.GetILGenerator();
    il.Emit(OpCodes.Ldstr, scenePath);
    il.EmitCall(OpCodes.Call, switchToSceneInfo, null);
    il.Emit(OpCodes.Ret);

    // generate the new type and save it
    var newType = typeBuilder.CreateType();
    assemblyBuilder.Save(aName.Name + ".dll");
  }
}
