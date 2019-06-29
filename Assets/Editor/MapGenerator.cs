using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

using System.IO;

using System.Collections.Generic;

[InitializeOnLoad]
static class MapGenerator {

  /* File Format for tiles !symbolheight
   * height default = 1
   * start default false (! at front of string)
   * should have a tree (t at front of string)
   * ^ For floating block, with next character base block, optional height next !! optional height ends with space.
   * - for unpathable
   * example !D2.5 for start tile dirt height 2.5
   * example D for non-startTile height 1
   * example !tD2.5 for start tile dirt height 2.5 that contains a tree.
   * example ^OD3 for a 1 tile dirt block at height 3 with a water block at base height
   * example ^O2 D3 for a 2 tile dirt block at height 3 with a water block at base height. Note the space
  */

  static Dictionary<char, Object> cubes = new Dictionary<char, Object>();
  static GameObject board;

  static MapGenerator() {
    cubes.Add('G', Resources.Load("Map/Tiles/Grass"));
    cubes.Add('D', Resources.Load("Map/Tiles/Dirt"));
    cubes.Add('S', Resources.Load("Map/Tiles/Stone"));
    cubes.Add('W', Resources.Load("Map/Tiles/Wall"));
    cubes.Add('O', Resources.Load("Map/Tiles/Water"));
    cubes.Add('M', Resources.Load("Map/Tiles/Mud"));
    cubes.Add('P', Resources.Load("Map/Tiles/Wood"));
  }

  static Object treeModel = Resources.Load("Map/Doodads/Tree");



  [MenuItem("Modify Map/Generate Terrain...")]
  private static void selectFileWrapper() {
    selectFile();
  }
  //file should be just the name of the map
  private static void selectFile(string file = "") {
    string fileName;
    string sceneName;
    if (file == "") {
      fileName = EditorUtility.OpenFilePanel("Select Map File", Application.dataPath, "csv");
      sceneName = fileName.Remove(fileName.Length-4);
      sceneName = "Assets/Scene/maps/"+sceneName.Substring(sceneName.LastIndexOf("/")+1)+".unity";
    } else {
      fileName = "Assets/maps/"+file+".csv";
      sceneName = "Assets/Scene/maps/"+file+".unity";
    }
    Scene currentScene = EditorSceneManager.OpenScene(sceneName);
    board = GameObject.Find("Board");
    if (board == null) {
      board = GameObject.Find("Board(Clone)");
    }
    if (board != null) {
      Object.DestroyImmediate(board);
    }
    UnityEngine.Random.InitState(0);
    int lineNum = 0;
    if (string.IsNullOrEmpty(fileName)) {
      Channel.editor.Log("Bad file name: " + fileName);
      return;
    }
    board = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Map/Board"));
    var lines = File.ReadAllLines(fileName);
    lineNum = lines.Length;
    foreach (string s in lines) {
      lineNum--;
      generateRow(s, lineNum);
    }
    EditorSceneManager.SaveScene(currentScene);
  }

  private static void generateRow(string s, int lineNum) {
    GameObject row = new GameObject("Row");
    string[] tiles = s.Split(',');
    int index = 0;
    foreach (string tile in tiles) {
      string t = tile;
      bool startTile = false;
      bool hasTree = false;
      bool floating = false;
      char baseBlockSymbol = ' ';
      float floatScale = 0;
      bool unpathable = false;
      //should replace all these ors with a set contains eventually.
      while (t[0] == '!' || t[0] == 't' || t[0] == '^' || t[0] == '-') {
        if (t[0] == '!') {
          startTile = true;
          t = t.Substring(1);
        } else if (t[0] == 't') {
          hasTree = true;
          unpathable = true;
          t = t.Substring(1);
        } else if (t[0] == '^') {
          floating = true;
          baseBlockSymbol = t[1];
          t = t.Substring(2);
          int split = t.IndexOf(' ');
          if (split != -1) {
            floatScale = float.Parse(t.Substring(0,split));
            t = t.Substring(split+1);
          }
        } else if (t[0] == '-') {
          unpathable = true;
          t = t.Substring(1);
        }
      }
      char tileSymbol = t[0];
      Debug.AssertFormat(cubes.ContainsKey(tileSymbol), "bad tile string " + tileSymbol);
      t = t.Substring(1);
      float height = 1;
      if (t.Length != 0) height = float.Parse(t);
      var o = (GameObject)PrefabUtility.InstantiatePrefab(cubes[tileSymbol]);
      o.transform.SetParent(row.transform);
      o.transform.position = new Vector3(0,0,index);
      //set startTile
      Tile newTile = o.GetComponent<Tile>();
      // if (newTile == null) {
        newTile = o.transform.GetChild(0).GetComponent<Tile>();
      // }
      newTile.startTile = startTile;
      //set height
      if (floating) {
        if (floatScale == 0){
          newTile.additionalHeight = height-1;
          o.transform.position = new Vector3(0, height-1, index);
        } else {
          o.transform.position = new Vector3(0, height-floatScale, index);
          o.transform.localScale = new Vector3(1,floatScale,1);
          newTile.additionalHeight = height-floatScale;
        }
        var baseBlock = (GameObject)PrefabUtility.InstantiatePrefab(cubes[baseBlockSymbol]);
        baseBlock.transform.SetParent(row.transform);
        baseBlock.transform.position = new Vector3(0,0,index);
        baseBlock.transform.GetChild(0).tag = "Untagged";
      } else {
        o.transform.localScale = new Vector3(1,height,1);
      }
      if (hasTree) {
        var tree = (GameObject)PrefabUtility.InstantiatePrefab(treeModel);
        tree.transform.SetParent(o.transform);
        float rand = 0.05f - UnityEngine.Random.value/10.0f;
        tree.transform.localScale = new Vector3(tree.transform.localScale.x,
                                        tree.transform.localScale.y + rand / o.transform.localScale.y, tree.transform.localScale.z);
        tree.transform.localPosition = new Vector3(0,1f,0);
      }
      if (unpathable) {
        newTile.movePointSpent = 2*Tile.unpathableCost;
      }

      index++;
    }
    row.transform.position = new Vector3(lineNum,0,0);
    row.transform.SetParent(board.transform);
  }


  [MenuItem("Modify Map/Generate Terrain All...")]
  private static void generateAllMaps() {
    DirectoryInfo dir = new DirectoryInfo("Assets/maps");
    FileInfo[] info = dir.GetFiles("*.csv");
    string prevScene = EditorSceneManager.GetActiveScene().path;
    foreach (FileInfo f in info) {
      string name = f.Name.Remove(f.Name.Length-4);
      Channel.editor.Log("Generating map for: " + name);
      selectFile(name);
    }
    EditorSceneManager.OpenScene(prevScene);
  }
  [MenuItem("Modify Map/Generate Terrain Current")]
  private static void generateCurrentMap() {
    string fileName = EditorSceneManager.GetActiveScene().path;
    fileName = fileName.Remove(fileName.Length-6);
    fileName = fileName.Substring(fileName.LastIndexOf("/")+1);
    selectFile(fileName);
  }

}
