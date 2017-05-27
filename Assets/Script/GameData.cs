using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

[System.Serializable]
public class GameData {
  public static GameData gameData;


  public List<Character> characters = new List<Character>();
  public Inventory inv = new Inventory();

}
