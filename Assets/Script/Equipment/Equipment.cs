using UnityEngine;
using System.Collections.Generic;

public class Equipment : MonoBehaviour {
  public int strength = 0;
  public int intelligence = 0;
  public int speed = 0;
  public int health = 0;
  public int physicalDefense = 0;
  public int magicDefense = 0;
  public float healingMultiplier = 0;

  //Equipment only Values
  public int fireResistance = 0;
  public int iceResistance = 0;
  public int lightningResistance = 0;

  public void applyEquipment(Attributes a) {
    a.strength += strength;
    a.intelligence += intelligence;
    a.speed += speed;
    a.maxHealth += health;
    a.physicalDefense += physicalDefense;
    a.magicDefense += magicDefense;
    a.healingMultiplier += strength;
    a.fireResistance += fireResistance;
    a.iceResistance += iceResistance;
    a.lightningResistance += lightningResistance;
  }

  public void removeEquipment(Attributes a) {
    a.strength -= strength;
    a.intelligence -= intelligence;
    a.speed -= speed;
    a.maxHealth -= health;
    a.physicalDefense -= physicalDefense;
    a.magicDefense -= magicDefense;
    a.healingMultiplier -= strength;
    a.fireResistance -= fireResistance;
    a.iceResistance -= iceResistance;
    a.lightningResistance -= lightningResistance;
  }
}
