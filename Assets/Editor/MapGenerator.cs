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
   * example !D2.5 for start tile dirt height 2.5
   * example D for non-startTile height 1
  */

  static Dictionary<char, Object> cubes = new Dictionary<char, Object>();
  static GameObject board;

  static MapGenerator() {
    cubes.Add('G', Resources.Load("Map/Grass"));
    cubes.Add('D', Resources.Load("Map/Dirt"));
    cubes.Add('S', Resources.Load("Map/Stone"));
    cubes.Add('W', Resources.Load("Map/Wall"));
  }



  [MenuItem("Generate Map/Generate...")]
  private static void selectFile() {
    string fileName = EditorUtility.OpenFilePanel("Select Map File", Application.dataPath, "csv");
    int lineNum = 0;
    if (!string.IsNullOrEmpty(fileName)) {
      Scene scene = EditorSceneManager.GetActiveScene();
      GameObject[] objs = scene.GetRootGameObjects();
      GameObject parent = objs[0];
      foreach(GameObject o in objs) {
        Debug.Log(o.name);
        if (o.name == "map") parent = o;
      }
      board = (GameObject)GameObject.Instantiate(Resources.Load("Map/Board"), parent.transform);
      //foreach (string s in File.ReadAllLines(fileName)) {
        //generateRow(s, lineNum);
      //  lineNum++;
      //}
      var lines = File.ReadAllLines(fileName);
      lineNum = lines.Length;
      foreach (string s in lines) {
        lineNum--;
        generateRow(s, lineNum);
      }

    }
  }

  private static void generateRow(string s, int lineNum) {
    GameObject row = new GameObject("Row");
    string[] tiles = s.Split(',');
    int index = 0;
    foreach (string tile in tiles) {
      string t = tile;
      bool startTile = false;
      if (t[0] == '!') {
        startTile = true;
        t = t.Substring(1);
      }
      char tileSymbol = t[0];
      Debug.AssertFormat(cubes.ContainsKey(tileSymbol), "bad tile string " + tileSymbol);
      t = t.Substring(1);
      float height = 1;
      if (t.Length != 0) height = float.Parse(t);
      var o = (GameObject)GameObject.Instantiate(cubes[tileSymbol], row.transform);
      o.transform.position = new Vector3(0,0,index);
      //set startTile
      o.GetComponent<Tile>().startTile = startTile;
      //set height
      o.transform.localScale = new Vector3(1,2*height-1,1);

      index++;
    }
    row.transform.position = new Vector3(lineNum,0,0);
    row.transform.SetParent(board.transform);
  }

}
