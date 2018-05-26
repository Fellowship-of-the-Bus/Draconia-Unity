using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

[System.Serializable]
public class GameData {
  public static GameData gameData;


  public List<Character> characters = new List<Character>();
  public Inventory inv = new Inventory();
  public Dictionary<string,string> mapProgression = new Dictionary<string,string>();

  public void newGame() {
  	String[] skills = {"Knockback", "ScorchEarth", "Punch"};
  	SkillTree skillTree;

    GameData.gameData.characters.Add(new Character("Brodric"));
    GameData.gameData.characters.Add(new Character("Sisdric"));

    inv.addEquipment(new Weapon(EquipmentClass.Bow, 2, 1));
    inv.addEquipment(new Weapon(EquipmentClass.Sword, 2, 1));

    foreach (Character c in  GameData.gameData.characters) {
   		c.attr.strength = 10;
	    c.attr.intelligence = 125;
	    c.attr.speed = 10;
	    skillTree = c.skills;
	    foreach (String skill in skills) {
	    	Type t = Type.GetType(skill);
	    	skillTree.setSkillLevel(t, 1);
	    	skillTree.equipSkill(t);
	    }
    }
  }

  public Character getCharacterWithItem(Equipment e) {
    foreach (Character c in characters) {
      if (c.gear[e.type] == e) return c;
    }
    return null;
  }

  public Character getCharacterByName(string name) {
    foreach(Character c in characters) {
      if (c.name == name) return c;
    }
    return null;
  }
}
