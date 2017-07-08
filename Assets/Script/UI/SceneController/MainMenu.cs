using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu: MonoBehaviour {
  public FileBrowser loadBrowser;

  public void test() {
    SceneManager.LoadSceneAsync("OverWorld");
  }

  public void loadGame() {
    loadBrowser.createOptions(SaveLoad.listSaveFiles());
  }

  public void newGame() {
    GameData.gameData = new GameData();
    GameData.gameData.characters.Add(new Character("Sisdric"));
    GameData.gameData.characters.Add(new Character("Brodric"));
    Character c = GameData.gameData.characters[0];
    c.attr.strength = 200;

    Equipment e = new Weapon();
    e.attr.strength = 1;
    GameData.gameData.inv.addEquipment(e);
    e.equipmentClass = "sis weapon";
    c.equip(e);

    e = new Armour();
    e.attr.intelligence = 1;
    e.attr.healingMultiplier = 0.9f;
    GameData.gameData.inv.addEquipment(e);
    e.equipmentClass = "sis armour";
    c.equip(e);

    c = GameData.gameData.characters[1];

    e = new Armour();
    e.attr.intelligence = 2;
    GameData.gameData.inv.addEquipment(e);
    e.equipmentClass = "bro armour";
    c.equip(e);

    e = new Weapon();
    e.attr.strength = 2;
    GameData.gameData.inv.addEquipment(e);
    e.equipmentClass = "weapon";

    e = new Armour();
    e.attr.intelligence = 3;
    GameData.gameData.inv.addEquipment(e);
    e.equipmentClass = "armour";

    e = new Weapon();
    e.attr.strength = 2;
    GameData.gameData.inv.addEquipment(e);
    e.equipmentClass = "weapon material1";

    e = new Weapon();
    e.attr.strength = 2;
    GameData.gameData.inv.addEquipment(e);
    e.equipmentClass = "weapon material2";

    e = new Weapon();
    e.attr.strength = 2;
    GameData.gameData.inv.addEquipment(e);
    e.equipmentClass = "weapon material3";


    SceneManager.LoadSceneAsync("OverWorld");
  }
}
