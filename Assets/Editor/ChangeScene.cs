using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;
using System.IO;

public abstract class ChangeScene {}

[InitializeOnLoad]
public static class MenuItemGenerator {
  private static readonly string outputDir = "Assets/Editor/SceneSwitcher";
  private static readonly string sceneDir = "Assets/Scene";

  // cache switchToScene handle
  private static MethodInfo switchToSceneInfo;

  // Toggle menu generation
  private const string menuName = "Open Scene/Enable Button Generation";
  private const string settingName = "EnableButtonGeneration";
  public static bool isEnabled {
    get { return EditorPrefs.GetBool(settingName, true); }
    set { EditorPrefs.SetBool(settingName, value); }
  }

  // put item at priority 101 so it's always at the top
  [MenuItem(menuName, false, 101)]
  private static void toggleButtonGeneration() {
    isEnabled = !isEnabled;
    if (isEnabled) {
      EditorSceneManager.sceneSaved += onSceneSaved;
      regenerateAll();  // may have been new scenes since last enabled
    } else {
      EditorSceneManager.sceneSaved -= onSceneSaved;
    }
  }

  // validator is needed because setting menu checked in toggleButton doesn't work correctly
  [MenuItem(menuName, true, 101)]
  private static bool toggleButtonGenerationValidate() {
    Menu.SetChecked(menuName, isEnabled);
    return true;
  }

  // Toggle debug
  private static Channel channel = new Channel("ChangeScene", isDebugEnabled);
  private const string debugMenuName = "Open Scene/Debug Button Generation";
  private const string debugSettingName = "EnableDebugButtonGeneration";
  public static bool isDebugEnabled {
    get { return EditorPrefs.GetBool(debugSettingName, false); }
    set { EditorPrefs.SetBool(debugSettingName, value); channel.enabled = value; }
  }

  //
  [MenuItem(debugMenuName, false, 102)]
  private static void toggleDebug() {
    isDebugEnabled = !isDebugEnabled;
  }

  [MenuItem(debugMenuName, true, 102)]
  private static bool toggleDebugValidate() {
    Menu.SetChecked(debugMenuName, isDebugEnabled);
    return true;
  }

  [MenuItem("Open Scene/Reset Scene Switcher", false, 103)]
  private static void resetSceneSwitcher() {
    foreach (var asset in Directory.EnumerateFiles(outputDir, "*.dll")) {
      channel.Log("{0} {1}", asset, AssetDatabase.DeleteAsset(asset));
    }
    regenerateAll();
  }

  private static void regenerateAll() {
    channel.Log("Regenerating all scene switchers");
    // search for scenes in sceneDir and generate menu items for each
    string[] sceneGUIDs = AssetDatabase.FindAssets("t:SceneAsset", new string[] { sceneDir });
    foreach (string guid in sceneGUIDs) {
      generateMenuItem(AssetDatabase.GUIDToAssetPath(guid));
    }
    // refresh the asset database so that the new items are loaded in
    // TODO: is it better to do this or import each asset that was actually generated?
    AssetDatabase.Refresh();
  }

  static MenuItemGenerator() {
    if (isEnabled) {
      EditorSceneManager.sceneSaved += onSceneSaved;
    }

    // retrieve MenuItemGenerator.switchScene
    Type thisType = typeof(MenuItemGenerator);
    switchToSceneInfo = thisType.GetMethod("switchToScene");

    // ensure outputDir exists
    System.IO.Directory.CreateDirectory(outputDir);

    regenerateAll();
  }

  private static void onSceneSaved(Scene scene) {
    channel.Log("Generating menu item for: " + scene.name + " // " + scene.path);
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
      Channel.editor.LogError(string.Format("error: scene not found: {0}", name));
    }
  }

  static void generateMenuItem(string scenePath) {
    // TODO: should strip only Assets/ or Assets/Scene/
    string sceneName = scenePath.Substring(sceneDir.Length+1, scenePath.Length-".unity".Length-(sceneDir.Length+1));  // +1 for /
    string sanitizedName = sanitize(sceneName);

    AssemblyName aName = new AssemblyName(sanitizedName);
    string assetPath = Path.Combine(outputDir, aName.Name + ".dll");
    string guid = AssetDatabase.AssetPathToGUID(assetPath);
    if (guid != "") {
      // don't need to generate the same code more than once
      channel.Log("Skipping generation for " + guid + " // " + assetPath);
      return;
    }
    channel.Log("Generating menu item for " + scenePath);

    // define the dll and class
    var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(aName, AssemblyBuilderAccess.RunAndSave, outputDir);
    var moduleBuilder = assemblyBuilder.DefineDynamicModule(aName.Name, aName.Name + ".dll");
    TypeBuilder typeBuilder = moduleBuilder.DefineType("ChangeScene_" + sanitizedName, System.Reflection.TypeAttributes.Public, typeof(ChangeScene));

    // add attribute [MenuItem("Open Scene/{sceneName}", false, 201)]
    // using priority 201 so it's always after the toggle button, and 201 > 101 + 10, so
    // a line separator is added.
    var attrCtorParams = new Type[] { typeof(string), typeof(bool), typeof(int) };
    var attrCtorInfo = typeof(MenuItem).GetConstructor(attrCtorParams);
    var attrBuilder = new CustomAttributeBuilder(attrCtorInfo, new object[] { "Open Scene/" + sceneName, false, 201 });

    // define: static void switchToScene_{sceneName}() {}
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
