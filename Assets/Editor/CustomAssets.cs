using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
static class CustomAssets {

  // put these menus right below the UI menu
  private const int priority = 10;

  [MenuItem("GameObject/Custom UI/Text", false, priority)]
  private static void createText(MenuCommand menuCommand) {
    instantiate("UI/General/Text", menuCommand);
  }

  [MenuItem("GameObject/Custom UI/Button", false, priority)]
  private static void createButton(MenuCommand menuCommand) {
    instantiate("UI/General/Button", menuCommand);
  }

  // instantiates Prefab at path, sets parent to selected object, and sets new object as selected
  private static void instantiate(string path, MenuCommand command) {
    GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load(path));
    if (command.context) {
      obj.transform.SetParent((command.context as GameObject).transform);
    }
    Selection.objects = new Object[] { obj };
  }
}
