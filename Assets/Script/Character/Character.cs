using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character {


  public SkillTree skills = new SkillTree();
  public Attributes attr = new Attributes();
  public string name = "";

  [System.Serializable]
  public class Gear {
    public Weapon weapon;
    public Armour armour;

    public Equipment this[int i] {
      get {
        if (i == EquipType.weapon) return weapon;
        if (i == EquipType.armour) return armour;
        return null;
      }
      set {
        if (i == EquipType.weapon) weapon = value as Weapon;
        if (i == EquipType.armour) armour = value as Armour;
      }
    }

    public Gear(Weapon w, Armour a) {
      weapon = w;
      armour = a;
    }

    public IEnumerator<Equipment> GetEnumerator()
    { //Needs to return in same order as EquipType orders the corresponding values
        yield return weapon;
        yield return armour;
    }

  }

  public Gear gear = new Gear(null,null);

  //gain experience when using a skill and when killing enemy.
  public int curLevel = 1;
  public const int maxLevel = 100;
  [HideInInspector]
  public int curExp = 0;
  [HideInInspector]
  public int maxExp = 100;

  //exp given to killer on death
  public int expGiven = 30;

  public Character(string name): this() {
    this.name = name;
  }

  public Attributes totalAttr { get { return attr + gearAttr(); } }


  public Character() {
    attr.maxHealth = 25;
    attr.moveRange = 4;
    equip(Weapon.defaultWeapon);
    equip(Armour.defaultArmour);
  }

  public void unEquip(Equipment e) {
    if (e == null) return;
    gear[e.type] = null;
  }

  public void equip(Equipment e) {
    unEquip(gear[e.type]);
    gear[e.type] = e;
  }
  public Attributes gearAttr() {
    Attributes attr = new Attributes();
    foreach (Equipment e in gear) {
      if (e != null) {
        attr += e.attr;
      }
    }
    return attr;
  }

  public void gainExp(int amount) {
    curExp += amount;
    int levelsToGain = curExp/maxExp;
    curExp = curExp % maxExp;
    if (levelsToGain == 0 ) {
      return;
    }
    levelsToGain = (int)Math.Min(levelsToGain, maxLevel - curLevel);
    curLevel += levelsToGain;
    gainStats(levelsToGain);
    skills.gainLevels(levelsToGain);
  }

  //gain stats functionp
  public void gainStats(int levels) {

  }
}
