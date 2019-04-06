using UnityEngine;
using System.Collections.Generic;
using System;

public enum TraitName{
  strPlus, intPlus, speedPlus, maxHPPlus, pDefPlus, mDefPlus, mvRangePlus,
}

[System.Serializable]
public class Trait {
  public static class TraitFactory {
    public static List<TraitName> traitNames = new List<TraitName>((IEnumerable<TraitName>)Enum.GetValues(typeof(TraitName)));
    public static List<Trait> getRandomTraits(int n) {
      List<int> randomNumbers = new List<int>();
      int numTraits = traitNames.Count;
      if (n > numTraits) {
        Channel.game.Log("Trying to get too many Traits, stopping");
        return new List<Trait>();
      }
      while (randomNumbers.Count < n) {
        int rand = UnityEngine.Random.Range(0, numTraits);
        if (randomNumbers.Contains(rand)) {
          continue;
        } else {
          randomNumbers.Add(rand);
        }
      }
      List<Trait> ret = new List<Trait>();
      foreach (int i in randomNumbers) {
        ret.Add(new Trait(traitNames[i]));
      }
      return ret;
    }
  }
  public TraitName type;
  public string name;
  public float strength = 1;
  public float intelligence = 1;
  public float speed = 1;
  public float maxHealth = 1;
  public float physicalDefense = 1;
  public float magicDefense = 1;
  public int moveRange = 0;

  public Trait() {} // so i can make default traits still
  public Trait(TraitName t) {
    type = t;
    switch(type) {
      case TraitName.strPlus:
        name = "Strong";
        strength = 1.1f;
        break;
      case TraitName.intPlus:
        name = "Intelligent";
        intelligence = 1.1f;
        break;
      case TraitName.speedPlus:
        name = "Quick";
        speed = 1.1f;
        break;
      case TraitName.maxHPPlus:
        name = "Resilient";
        maxHealth = 1.1f;
        break;
      case TraitName.pDefPlus:
        name = "Tough";
        physicalDefense = 1.1f;
        break;
      case TraitName.mDefPlus:
        name = "Warded??";
        magicDefense = 1.1f;
        break;
      case TraitName.mvRangePlus:
        name = "fleet??";
        moveRange = 1;
        physicalDefense = 0.95f;
        magicDefense = 0.95f;
        break;
    }
  }

  public static Trait operator+(Trait a1, Trait a2) {
    Trait ret = a1.clone();
    ret.strength = ret.strength + a2.strength - 1;
    ret.intelligence = ret.intelligence + a2.intelligence - 1;
    ret.speed = ret.speed + a2.speed - 1;
    ret.maxHealth = ret.maxHealth + a2.maxHealth - 1;
    ret.physicalDefense = ret.physicalDefense + a2.physicalDefense - 1;
    ret.magicDefense = ret.magicDefense + a2.magicDefense - 1;
    ret.moveRange = ret.moveRange + a2.moveRange;
    return ret;
  }

  public Attributes applyTrait(Attributes baseAttr) {
    Attributes ret = baseAttr.clone();
    ret.strength = (int)(ret.strength * strength);
    ret.intelligence = (int)(ret.intelligence * intelligence);
    ret.speed = (int)(ret.speed * speed);
    ret.maxHealth = (int)(ret.maxHealth * maxHealth);
    ret.physicalDefense = (int)(ret.physicalDefense * physicalDefense);
    ret.magicDefense = (int)(ret.magicDefense * magicDefense);
    ret.moveRange += moveRange;
    return ret;
  }

  public Trait clone() {
    return MemberwiseClone() as Trait;
  }
}
