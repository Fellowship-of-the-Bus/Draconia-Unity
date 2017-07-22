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

    Equipment e = new Weapon(EquipmentClass.Sword, 1, 1);
    e.attr.strength = 1;
    GameData.gameData.inv.addEquipment(e);
    c.equip(e);

    e = new Armour(EquipmentClass.Shield, 1);
    e.attr.intelligence = 1;
    e.attr.healingMultiplier = 0.9f;
    GameData.gameData.inv.addEquipment(e);
    c.equip(e);

    c = GameData.gameData.characters[1];

    e = new Armour(EquipmentClass.Robe, 1);
    e.attr.intelligence = 2;
    GameData.gameData.inv.addEquipment(e);
    c.equip(e);

    e = new Weapon(EquipmentClass.Axe, 1, 1);
    e.attr.strength = 2;
    GameData.gameData.inv.addEquipment(e);

    e = new Armour(EquipmentClass.Leather, 1);
    e.attr.intelligence = 3;
    GameData.gameData.inv.addEquipment(e);

    e = new Weapon(EquipmentClass.Bow, 3, 1);
    e.attr.strength = 2;
    GameData.gameData.inv.addEquipment(e);

    e = new Weapon(EquipmentClass.Bow, 4, 2);
    e.attr.strength = 2;
    GameData.gameData.inv.addEquipment(e);

    e = new Weapon(EquipmentClass.Axe, 1, 1);
    e.attr.strength = 2;
    GameData.gameData.inv.addEquipment(e);

    SceneManager.LoadSceneAsync("OverWorld");
  }
}
