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

  public void newGame() {
  	String[] skills = {"Knockback", "Ranged", "Punch"};
  	SkillTree skillTree;

  	GameData.gameData.characters.Add(new Character("Sisdric"));
    GameData.gameData.characters.Add(new Character("Brodric"));

    foreach (Character c in  GameData.gameData.characters) {
   		c.attr.strength = 10;
	    c.attr.intelligence = 10;
	    c.attr.speed = 10;
	    skillTree = c.skills;
	    foreach (String skill in skills) {
	    	Type t = Type.GetType(skill);
	    	skillTree.setSkillLevel(t, 1);
	    	skillTree.equipSkill(t);
	    }
    }
  }
}
