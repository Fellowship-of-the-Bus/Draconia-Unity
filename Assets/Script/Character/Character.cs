using System;
using System.Collections.Generic;

[System.Serializable]
public class Character {
  public SkillTree skills = new SkillTree();
  public Attributes attr = new Attributes();
  public string name = "";
  public List<Trait> _traits;
  public List<Trait> traits {
    get {return _traits;}
    set {
      _traits = value;
      totalTraits = new Trait();
      foreach (Trait tr in traits) {
        totalTraits = totalTraits + tr;
      }
    }
  }
  public Trait totalTraits = new Trait();

  public Gear gear = new Gear(null,null);

  //gain experience when using a skill and when killing enemy.
  public int curLevel = 1;
  //[HideInInspector]
  public int curExp = 0;
  //debug purposes only
  public int expAtNextLevel = 0;

  //difference of exp threshold of next level and current level
  public int nextLevelExpDifference {
    get {
      return expAtLevel(curLevel+1) - expAtLevel(curLevel);
    }
  }

  //exp given to killer on death
  //assuming ~10 kills gives a level up (took exp needed to levelup, divided by 10 and interpolated)
  //then took the coefficients to be integers
  //Current setting:
  //Maybe this should not be constant at all levels? Maybe should be like 6 kills at level 1, scaling to 10 at level 50?
  private const int killsAtLevel1 = 4;
  private const int killsAtLevel50 = 10;
  private const int bossMultiplier = 2;
  public int expGivenOnKill {
    get {
      // return 6*curLevel*curLevel - 19*curLevel + 140;
      //5 at level 1 to 10 at level 50 and beyond:
      float numKillPerLevelup = killsAtLevel1 + curLevel*(killsAtLevel50-killsAtLevel1)/50.0f;
      if (curLevel > 50) {
        numKillPerLevelup = killsAtLevel50;
      }
      int rawExp = (int)((expAtLevel(curLevel+1) - expAtLevel(curLevel))/numKillPerLevelup);
      if (GameManager.get.boss != null && GameManager.get.boss.baseChar == this) {
        return bossMultiplier*rawExp;
      } else {
        return rawExp;
      }
    }
  }

  public Character(string name): this() {
    this.name = name;
  }

  public Attributes totalAttr {
    get {
      return totalTraits.applyTrait(attr) + gearAttr();
    }
  }


  public Character() {
    traits = new List<Trait>();
    attr.maxHealth = CharacterGenerator.HEALTH_BASE;
    attr.moveRange = CharacterGenerator.MRANGE_BASE;
  }

  public void equip(Equipment e) {
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

  // Interpolated experience points like (level, exp) : (2,1000), (3, 2250) etc:
  // rounded coefficients to nice numbers afterwards.
  public int expAtLevel(int level) {
    if (level <= 1){
      return 0;
    }

    level = level - 1;
    return 20*level*level*level - 50*level*level + 1250*level - 200;
  }

  public int expToLevelUp() {
    return expToLevel(curLevel + 1);
  }

  public int expToLevel(int level) {
    return expAtLevel(level) - curExp;
  }

  public float percentageToNextLevel() {
    return (float)(curExp - expAtLevel(curLevel))/nextLevelExpDifference;
  }

  //requires l > curLevel or l == curLevel and curExp == expAtLevel(level)
  public void setLevel(int level) {
    if (! (level > curLevel || curExp == expAtLevel(level))) {
      Channel.game.Log("character.set_level() called trying to lower level/remove exp, may infinite loop");
    }
    gainExp(expToLevel(level), false);
  }

  // for use in the debug button
  public void setLevelUp() {
    gainExp(expToLevelUp(), false);
  }

  //returns maxHP gain
  public int gainExp(int amount, bool applyExpTrait = true) {
    if (applyExpTrait) {
      amount = (int)(amount * (1+totalTraits.spec.expGain));
    }
    curExp += amount;
    int newLevel = curLevel;
    while(expAtLevel(newLevel+1) <= curExp){
      newLevel += 1;
    }
    expAtNextLevel = expAtLevel(newLevel + 1);
    int levelsToGain = newLevel - curLevel;
    curLevel = newLevel;
    skills.gainLevels(levelsToGain);
    return gainStats(levelsToGain);
  }

  public static readonly int STR_GAIN = 2;
  public static readonly int INT_GAIN = 2;
  public static readonly int SPEED_GAIN = 8;
  public static readonly int HEALTH_GAIN = 60;
  public static readonly int PDEF_GAIN = 5;
  public static readonly int MDEF_GAIN = 5;
  //gain stats functionp
  //returns maxHp gain
  public int gainStats(int levels) {
    attr.strength += STR_GAIN * levels;
    attr.intelligence += INT_GAIN * levels;
    attr.speed += SPEED_GAIN * levels;
    attr.maxHealth += HEALTH_GAIN * levels;
    attr.physicalDefense += PDEF_GAIN * levels;
    attr.magicDefense += MDEF_GAIN * levels;
    return HEALTH_GAIN*levels;
  }
}
