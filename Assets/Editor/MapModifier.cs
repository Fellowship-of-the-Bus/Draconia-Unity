using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

using System.IO;

using System.Collections.Generic;
using System;

[InitializeOnLoad]
static class MapModifier {

  static Dictionary<CharacterType, UnityEngine.Object> replacements = new Dictionary<CharacterType, UnityEngine.Object>();

  static MapModifier() {
    replacements.Add(CharacterType.Human, Resources.Load("Characters/Human"));
    replacements.Add(CharacterType.Lizard, Resources.Load("Characters/Lizard"));
    replacements.Add(CharacterType.Snake, Resources.Load("Characters/Snake"));
  }

  private static void modifyAllMaps(Action<string> fun) {
    DirectoryInfo dir = new DirectoryInfo("Assets/maps");
    FileInfo[] info = dir.GetFiles("*.csv");
    string prevScene = EditorSceneManager.GetActiveScene().path;
    foreach (FileInfo f in info) {
      string name = f.Name.Remove(f.Name.Length-4);
      Channel.editor.Log("Operating in Scene : " + name);
      fun("Assets/Scene/maps/"+name+".unity");
    }
    EditorSceneManager.OpenScene(prevScene);
  }

  [MenuItem("Modify Map/Change Height/Current Map")]
  private static void currentMapChangeHeight() {
    changeHeight(EditorSceneManager.GetActiveScene().path);
  }

  [MenuItem("Modify Map/Change Height/All Maps")]
  private static void allMapChangeHeight() {
    modifyAllMaps(changeHeight);
  }

  [MenuItem("Modify Map/Move to Top/Current Map")]
  private static void currentMapMoveToTop() {
    moveToTop(EditorSceneManager.GetActiveScene().path);
  }

  [MenuItem("Modify Map/Move to Top/All Maps")]
  private static void allMapMoveToTop() {
    modifyAllMaps(moveToTop);
  }

  [MenuItem("Modify Map/Add Object/Current Map")]
  private static void currentMapAddObject() {
    addObject(EditorSceneManager.GetActiveScene().path);
  }

  [MenuItem("Modify Map/Add Object/All Maps")]
  private static void allMapAddObject() {
    modifyAllMaps(addObject);
  }

  [MenuItem("Modify Map/Set Character Stats/Current Map")]
  private static void currentMapSetStats() {
    setStats(EditorSceneManager.GetActiveScene().path);
  }
  // This is commented because it will overwrite stats for all the maps
  // [MenuItem("Modify Map/Set Character Stats/Current Map")]
  // private static void allMapSetStats() {
  //   modifyAllMaps(currentMapSetStats);
  // }

  private static void addObject(string name) {
    Scene currentScene = EditorSceneManager.OpenScene(name);
    GameObject o = new GameObject();
    o.name = "Allies";
    EditorSceneManager.SaveScene(currentScene);
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
    GameObject[] pieces = GameObject.FindGameObjectsWithTag("Unit");
    //GameObject pieces = GameObject.Find("Enemies");
    //Transform pTransform = pieces.transform;
    // List<Transform> toAdd = new List<Transform>();
    // List<GameObject> toDelete = new List<GameObject>();
    GameManager gameManager = GameObject.Find("__GameManager").GetComponent<GameManager>();
    // foreach (Transform child in pTransform) {
      // GameObject childObject = child.gameObject;
    foreach (GameObject childObject in pieces) {
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
            Transform child = cube.GetChild(0);
            if (child.position.z == childObject.transform.position.z) {
              childObject.transform.position = new Vector3(childObject.transform.position.x, child.gameObject.GetComponent<Tile>().position.y, childObject.transform.position.z);
              SerializedObject sObject = new SerializedObject(childObject.GetComponent<BattleCharacter>());
              SerializedProperty team = sObject.FindProperty("team");
              team.intValue = 1;
              sObject.ApplyModifiedProperties();
              break;
            }
          }
        }
      }

    }

    // foreach(Transform t in toAdd) {
    //   t.SetParent(pTransform);
    // }

    // foreach(GameObject o in toDelete) {
    //   GameObject.DestroyImmediate(o);
    // }
    EditorSceneManager.SaveScene(currentScene);
  }

  //PSP = per skill point
  private static int DamageStatGainLowPSP = 3;
  private static int SpeedGainLowPSP = 0;
  private static int HPGainLowPSP = 0;
  private static int DefGainLowPSP = 0;
  private static void setStats(string name) {
    Scene currentScene = EditorSceneManager.OpenScene(name);
    GameObject enemies = GameObject.Find("Enemies");
    foreach (Transform child in enemies.transform) {
      BattleCharacter enemyBCharacter = child.gameObject.GetComponent<BattleCharacter>();
      SerializedObject sObject = new SerializedObject(enemyBCharacter);
      int level = enemyBCharacter.baseChar.curLevel;
      int numSkillPoints = SkillTree.skillPointsAtLevel(level);
      //set strength and int
      SerializedProperty strength = sObject.FindProperty("baseChar.attr.strength");
      SerializedProperty intelligence = sObject.FindProperty("baseChar.attr.intelligence");
      strength.intValue = CharacterGenerator.STR_BASE + (level-1)*Character.STR_GAIN + numSkillPoints*DamageStatGainLowPSP;
      intelligence.intValue = CharacterGenerator.INT_BASE + (level-1)*Character.INT_GAIN + numSkillPoints*DamageStatGainLowPSP;
      //set speed
      SerializedProperty speed = sObject.FindProperty("baseChar.attr.speed");
      speed.intValue = CharacterGenerator.SPEED_BASE + (level-1)*Character.SPEED_GAIN + numSkillPoints*SpeedGainLowPSP;
      //set hp
      SerializedProperty maxHP = sObject.FindProperty("baseChar.attr.maxHealth");
      maxHP.intValue = CharacterGenerator.HEALTH_BASE + (level-1)*Character.HEALTH_GAIN + numSkillPoints*HPGainLowPSP;
      //set defenses
      SerializedProperty pDef = sObject.FindProperty("baseChar.attr.physicalDefense");
      SerializedProperty mDef = sObject.FindProperty("baseChar.attr.magicDefense");
      pDef.intValue = CharacterGenerator.PDEF_BASE + (level-1)*Character.PDEF_GAIN + numSkillPoints*DefGainLowPSP;
      mDef.intValue = CharacterGenerator.MDEF_BASE + (level-1)*Character.MDEF_GAIN + numSkillPoints*DefGainLowPSP;
      sObject.ApplyModifiedProperties();
    }
    EditorSceneManager.SaveScene(currentScene);
  }
}
