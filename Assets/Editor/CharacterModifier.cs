using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

using System.IO;

using System.Collections.Generic;

[InitializeOnLoad]
static class CharacterModifier {

  static Dictionary<CharacterType, Object> replacements = new Dictionary<CharacterType, Object>();

  static CharacterModifier() {
    replacements.Add(CharacterType.Human, Resources.Load("Characters/Human"));
    replacements.Add(CharacterType.Lizard, Resources.Load("Characters/Lizard"));
    replacements.Add(CharacterType.Snake, Resources.Load("Characters/Snake"));
  }

  [MenuItem("Modify Map/Modify Character/change height - All")]
  private static void modifyAllMaps() {
    DirectoryInfo dir = new DirectoryInfo("Assets/maps");
    FileInfo[] info = dir.GetFiles("*.csv");
    string prevScene = EditorSceneManager.GetActiveScene().path;
    foreach (FileInfo f in info) {
      string name = f.Name.Remove(f.Name.Length-4);
      Debug.Log("Changing char heights in : " + name);
      moveToTop("Assets/Scene/maps/"+name+".unity");
    }
    EditorSceneManager.OpenScene(prevScene);
  }

  // [MenuItem("Modify Map/Modify Character/change Height - Select Map...")]
  // private static void selectMap() {
  //   string fileName = EditorUtility.OpenFilePanel("Select Map File", Application.dataPath, "csv");
  //   modifyMap(fileName);
  // }

  [MenuItem("Modify Map/Modify Character/change Height - current Map")]
  private static void currentMap() {
    changeHeight(EditorSceneManager.GetActiveScene().path);
  }

  [MenuItem("Modify Map/Modify Character/moveToTop - current Map")]
  private static void currentMapMoveToTop() {
    moveToTop(EditorSceneManager.GetActiveScene().path);
  }

  private static void moveToTop(string name) {
    Scene currentScene = EditorSceneManager.OpenScene(name);
    GameObject pieces = GameObject.Find("Pieces");
    List<GameObject> toMove = new List<GameObject>();
    GameObject enemies = new GameObject();
    enemies.name = "Enemies";
    foreach (Transform child in pieces.transform) {
      toMove.Add(child.gameObject);
    }
    foreach (GameObject piece in toMove) {
      piece.transform.SetParent(enemies.transform);
    }
    toMove.Clear();
    GameObject Doodads = GameObject.Find("Doodads");
    GameObject newDoodads = new GameObject();
    newDoodads.name = "Doodads";
    foreach (Transform child in Doodads.transform) {
      toMove.Add(child.gameObject);
    }
    foreach (GameObject doodad in toMove) {
      doodad.transform.SetParent(newDoodads.transform);
    }
    GameObject board = GameObject.Find("Board");
    board.transform.SetParent(null);
    EditorSceneManager.SaveScene(currentScene);
  }

  private static void changeHeight(string name){
    Scene currentScene = EditorSceneManager.OpenScene(name);
    GameObject pieces = GameObject.Find("Pieces");
    Transform pTransform = pieces.transform;
    List<Transform> toAdd = new List<Transform>();
    List<GameObject> toDelete = new List<GameObject>();
    GameManager gameManager = GameObject.Find("__GameManager").GetComponent<GameManager>();
    foreach (Transform child in pTransform) {
      GameObject childObject = child.gameObject;
      // toDelete.Add(childObject);
      // BattleCharacter character = childObject.GetComponent<BattleCharacter>();
      // GameObject newObject = (GameObject)PrefabUtility.InstantiatePrefab(replacements[character.characterType]);
      // toAdd.Add(newObject.transform);
      // newObject.name = childObject.name;
      // newObject.transform.position = childObject.transform.position;
      // newObject.transform.rotation = childObject.transform.rotation;
      // newObject.transform.localScale = childObject.transform.localScale;
      // BattleCharacter newCharacter = newObject.GetComponent<BattleCharacter>();
      // newCharacter.baseChar = character.baseChar;
      // newCharacter.aiType = character.aiType;
      // newCharacter.skillSet = character.skillSet;
      // newCharacter.team = character.team;
      // if (gameManager.boss == character) {
      //   gameManager.boss = newCharacter;
      // }
      GameObject board = GameObject.Find("Board");
      foreach(Transform row in board.transform) {
        if (row.gameObject.name != "Row") continue;
        if (row.position.x == childObject.transform.position.x) {
          foreach(Transform cube in row.transform) {
            if (cube.position.z == childObject.transform.position.z) {
              childObject.transform.position = new Vector3(childObject.transform.position.x, cube.gameObject.GetComponent<Tile>().position.y, childObject.transform.position.z);
              break;
            }
          }
        }
      }

    }

    foreach(Transform t in toAdd) {
      t.SetParent(pTransform);
    }

    foreach(GameObject o in toDelete) {
      GameObject.DestroyImmediate(o);
    }
    EditorSceneManager.SaveScene(currentScene);
  }
}
