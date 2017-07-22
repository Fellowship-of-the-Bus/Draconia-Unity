using System;

[System.Serializable]
public class Character {


  public SkillTree skills = new SkillTree();
  public Attributes attr = new Attributes();
  public string name = "";
  public Equipment[] gear = new Equipment[]{null,null};

  //gain experience when using a skill and when killing enemy.
  public int curLevel = 1;
  public int maxLevel = 100;
  public int curExp = 0;
  public int maxExp = 100;

  //exp given to killer on death
  public int expGiven = 30;

  public Character(string name): this() {
    this.name = name;
  }

  public Attributes totalAttr { get { return attr + gearAttr(); } }


  public Character() {
    attr.maxHealth = 10;
    attr.speed = 2;
    attr.moveRange = 4;
    equip(Weapon.defaultWeapon);
    equip(Armour.defaultArmour);
  }

  public void unEquip(Equipment e) {
    if (e == null) return;
    e.equippedTo = null;
    gear[e.type] = null;
  }

  public void equip(Equipment e) {
    unEquip(gear[e.type]);
    gear[e.type] = e;
    e.equippedTo = this;
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
