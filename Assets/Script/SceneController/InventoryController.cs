using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class InventoryController: MonoBehaviour {

  Inventory inv = GameData.gameData.inv;



  public void back() {
    SceneManager.LoadSceneAsync ("OverWorld");
  }
}
