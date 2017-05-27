using UnityEngine;
using System.Collections.Generic;
using System;

[System.Serializable]
public class Attributes {
  public int strength = 2;
  public int intelligence = 2;
  public int speed = 2;
  public int maxHealth = 10;
  public int physicalDefense;
  public int magicDefense;
  public float healingMultiplier = 1;
  public int moveRange = 4;
  public float moveTolerance;

  public int fireResistance;
  public int iceResistance;
  public int lightningResistance;

  public static Attributes operator+(Attributes a1, Attributes a2) {
    var ret = new Attributes();
    ret.strength = a1.strength + a2.strength;
    ret.intelligence = a1.intelligence + a2.intelligence;
    ret.speed = a1.speed + a2.speed;
    ret.maxHealth = a1.maxHealth + a2.maxHealth;
    ret.physicalDefense = a1.physicalDefense + a2.physicalDefense;
    ret.magicDefense = a1.magicDefense + a2.magicDefense;
    ret.healingMultiplier = a1.healingMultiplier*a2.healingMultiplier;
    ret.moveRange = a1.moveRange + a2.moveRange;
    ret.moveTolerance = a1.moveTolerance + a2.moveTolerance;

    ret.fireResistance = a1.fireResistance + a2.fireResistance;
    ret.iceResistance = a1.iceResistance + a2.iceResistance;
    ret.lightningResistance = a1.lightningResistance + a2.lightningResistance;

    return ret;
  }

  public static Attributes operator-(Attributes a1) {

    Attributes ret = new Attributes();
    ret.strength = -a1.strength;
    ret.intelligence = -a1.intelligence;
    ret.speed = -a1.speed;
    ret.maxHealth = -a1.maxHealth;
    ret.physicalDefense = -a1.physicalDefense;
    ret.magicDefense = -a1.magicDefense;
    ret.healingMultiplier = a1.healingMultiplier == 0 ? 0 : 1/a1.healingMultiplier;
    ret.moveRange = -a1.moveRange;
    ret.moveTolerance = -a1.moveTolerance;

    ret.fireResistance = -a1.fireResistance;
    ret.iceResistance = -a1.iceResistance;
    ret.lightningResistance = -a1.lightningResistance;

    return ret;
  }
  public static Attributes operator-(Attributes a1, Attributes a2) {
    return a1 + (-a2);
  }

  public Attributes clone() {
    return MemberwiseClone() as Attributes;
  }
}
