using UnityEngine;
using System.Collections.Generic;
using System;

public enum AttrTraitName{
  strPlus, intPlus, speedPlus, maxHPPlus, pDefPlus, mDefPlus, mvRangePlus,
  strPluspDefMinus, strPlusmDefMinus, strPlusHPMinus,
  intPlusmDefMinus, intPluspDefMinus, intPlusHPMinus,
  speedPlusHPMinus, speedPlusDefMinus,
  pDefPlusSpeedMinus, pDefPlusDmgMinus,
  mDefPlusSpeedMinus, mDefPlusDmgMinus,
  healingPlus,
}
public enum SpecTraitName{
  swordPlus,
  bowPlus,
  axePlus,
  staffPlus,
  humanSlayer,
  lizardSlayer,
  chameleonSlayer,
  snakeSlayer,
  dragonSlayer,
  icePlus,
  firePlus,
  lightningPlus,
  elementPlus,
  expGainPlus,
}
public enum UniqueTraitName{
  brodric, sisdric
}

[System.Serializable]
public class Trait {
  [System.Serializable]
  public class TraitAttr {
    public float strength = 0;
    public float intelligence = 0;
    public float speed = 0;
    public float maxHealth = 0;
    public float physicalDefense = 0;
    public float magicDefense = 0;
    public int moveRange = 0;
    public float healingMultiplier = 0;
  }
  [System.Serializable]
  public class TraitSpec {
    public static List<Weapon.EquipmentClass> equips = new List<Weapon.EquipmentClass>((IEnumerable<Weapon.EquipmentClass>)Enum.GetValues(typeof(Weapon.EquipmentClass)));
    public static List<EnemyType> enemies = new List<EnemyType>((IEnumerable<EnemyType>)Enum.GetValues(typeof(EnemyType)));
    public static List<DamageElement> elements = new List<DamageElement>((IEnumerable<DamageElement>)Enum.GetValues(typeof(DamageElement)));
    public float expGain = 0;
    public Dictionary<Weapon.EquipmentClass, float> wepSpec = new Dictionary<Weapon.EquipmentClass, float>();
    public Dictionary<EnemyType, float> enemySpec = new Dictionary<EnemyType, float>();
    public Dictionary<DamageElement, float> elementSpec = new Dictionary<DamageElement, float>();
    public TraitSpec() {
      foreach (Weapon.EquipmentClass e in equips) {
        wepSpec[e] = 0;
      }
      foreach (EnemyType e in enemies) {
        enemySpec[e] = 0;
      }
      foreach (DamageElement e in elements) {
        elementSpec[e] = 0;
      }
    }
  }
  public string name;
  public string description;
  public TraitAttr attr = new TraitAttr();
  public TraitSpec spec = new TraitSpec();

  public Trait(string name = "") {
    this.name = name;
  }

  public static Trait operator+(Trait a1, Trait a2) {
    Trait ret = new Trait();
    ret.attr.strength = a1.attr.strength + a2.attr.strength;
    ret.attr.intelligence = a1.attr.intelligence + a2.attr.intelligence;
    ret.attr.speed = a1.attr.speed + a2.attr.speed;
    ret.attr.maxHealth = a1.attr.maxHealth + a2.attr.maxHealth;
    ret.attr.physicalDefense = a1.attr.physicalDefense + a2.attr.physicalDefense;
    ret.attr.magicDefense = a1.attr.magicDefense + a2.attr.magicDefense;
    ret.attr.moveRange = a1.attr.moveRange + a2.attr.moveRange;
    ret.attr.healingMultiplier = a1.attr.healingMultiplier + a2.attr.healingMultiplier;

    ret.spec.expGain = a1.spec.expGain + a2.spec.expGain;
    foreach (var e in TraitSpec.equips) {
      ret.spec.wepSpec[e] = a1.spec.wepSpec[e] + a2.spec.wepSpec[e];
    }
    foreach (var e in TraitSpec.enemies) {
      ret.spec.enemySpec[e] = a1.spec.enemySpec[e] + a2.spec.enemySpec[e];
    }
    foreach (var e in TraitSpec.elements) {
      ret.spec.elementSpec[e] = a1.spec.elementSpec[e] + a2.spec.elementSpec[e];
    }
    return ret;
  }

  public Attributes applyTrait(Attributes baseAttr) {
    Attributes ret = baseAttr.clone();
    ret.strength += (int)(ret.strength * attr.strength);
    ret.intelligence += (int)(ret.intelligence * attr.intelligence);
    ret.speed += (int)(ret.speed * attr.speed);
    ret.maxHealth += (int)(ret.maxHealth * attr.maxHealth);
    ret.physicalDefense += (int)(ret.physicalDefense * attr.physicalDefense);
    ret.magicDefense += (int)(ret.magicDefense * attr.magicDefense);
    ret.moveRange += attr.moveRange;
    ret.healingMultiplier += attr.healingMultiplier;
    return ret;
  }
}
