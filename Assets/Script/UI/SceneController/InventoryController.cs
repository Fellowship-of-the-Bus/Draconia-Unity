using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class InventoryController: MonoBehaviour {

  //Inventory inv = GameData.gameData.inv;

  public AttrView attrView;
  public Text equipName;
  public Text equippedTo;
  public GameObject tooltip;

  void Awake() {
    get = this;
  }

  public void back() {
    LoadingScreen.load("OverWorld");
  }

  public static InventoryController get { get; set; }
}
