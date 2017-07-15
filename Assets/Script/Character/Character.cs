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
    equip(new Weapon("Unarmed", Weapon.kinds.Blunt, 1, 1));
    equip(new Armour("Unarmed", Armour.ArmourKinds.Leather, 1));
  }

  public void unEquip(Equipment e) {
    e.equippedTo = null;
    gear[e.type] = null;
  }

  public void equip(Equipment e) {
    if (gear[e.type] != null){
      gear[e.type].equippedTo = null;
    }
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
