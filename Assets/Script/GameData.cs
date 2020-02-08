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

  public void newGame(NewGameSettings settings) {
  	String[] skills = {"Punch", "Knockback", "Knockback", "Knockback"};
  	SkillTree skillTree;

    for (int i = 0; i < settings.weapons.Length; ++i) {
      inv.addEquipment(new Weapon(settings.weapons[i]));
    }

    for (int i = 0; i < settings.armor.Length; ++i) {
      inv.addEquipment(new Armour(settings.armor[i]));
    }

    GameData.gameData.characters.Add(CharacterGenerator.generateBrodric());
    GameData.gameData.characters.Add(CharacterGenerator.generateSisdric());

    for(int i = 0; i < settings.numGeneratedCharacters; i++) {
      GameData.gameData.characters.Add(CharacterGenerator.generateCharacter(1));
    }

    foreach (Character c in  GameData.gameData.characters) {
      if (c.attr.strength == 0) c.attr.strength = 10;
      if (c.attr.intelligence == 0) c.attr.intelligence = 10;
      if (c.attr.speed == 0) c.attr.speed = 10;
      skillTree = c.skills;
      foreach (String skill in skills) {
        Type t = Type.GetType(skill);
        skillTree.setSkillLevel(t, 1);
        skillTree.equipSkill(t);
      }
    }
  }

  public Character getCharacterByName(string name) {
    foreach(Character c in characters) {
      if (c.name == name) return c;
    }
    return null;
  }
}
