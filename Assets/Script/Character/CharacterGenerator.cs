using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class CharacterGenerator : MonoBehaviour {
  public static readonly int STR_BASE = 80;
  public static readonly int INT_BASE = 80;
  public static readonly int SPEED_BASE = 50;
  public static readonly int HEALTH_BASE = 250;
  public static readonly int PDEF_BASE = 30;
  public static readonly int MDEF_BASE = 30;
  public static readonly int MRANGE_BASE = 4;

  public static readonly int TRAIT_MIN = 2;
  public static readonly int TRAIT_MAX = 3;

  public TextAsset namesFile;
  private string[] names;

  public static CharacterGenerator get;

  void Awake() {
    if (!Singleton.makeSingleton(ref get, this)) return;
    names = namesFile.text.Split(new string[]{ "\n" }, System.StringSplitOptions.RemoveEmptyEntries);
  }

  private static Attributes generateBaseAttributes() {
    Attributes attr = new Attributes();

    //UnityEngine.Random.value;
    attr.strength = STR_BASE;
    attr.intelligence = INT_BASE;
    attr.speed = SPEED_BASE;
    attr.maxHealth = HEALTH_BASE;
    attr.physicalDefense = PDEF_BASE;
    attr.magicDefense = MDEF_BASE;

    attr.moveRange = MRANGE_BASE;

    return attr;
  }

  private static List<Trait> generateTraits() {
    return TraitFactory.get.getRandomTraits(1,1);//Random.Range(TRAIT_MIN, TRAIT_MAX+1));
  }

  private static string generateName() {
    int index = Random.Range(0, get.names.Length);
    return get.names[index];
  }

  private static Character generateBaseCharacter(int level) {
    Character character = new Character();
    character.attr = generateBaseAttributes();
    character.setLevel(level);
    character.equip(Weapon.defaultWeapon);
    character.equip(Armour.defaultArmour);
    return character;
  }

  public static Character generateCharacter(int level) {
    Character character = generateBaseCharacter(level);
    character.name = generateName();
    character.traits = generateTraits();
    return character;
  }

  public static Character generateBrodric() {
    Character character = generateBaseCharacter(1);
    character.name = "Brodric";
    //give additional stats unique to brodric
    character.attr.strength += 5;
    character.attr.physicalDefense += 3;
    character.attr.maxHealth += 25;
    //give some traits?
    character.traits = new List<Trait>() {TraitFactory.get.getTrait(UniqueTraitName.brodric)};
    return character;
  }

  public static Character generateSisdric() {
    Character character = generateBaseCharacter(1);
    character.name = "Sisdric";
    character.setLevel(1);
    //give additional stats unique to brodric
    character.attr.intelligence += 5;
    character.attr.magicDefense += 3;
    character.attr.maxHealth += 25;
    //give some traits?
    character.traits = new List<Trait>() {TraitFactory.get.getTrait(UniqueTraitName.sisdric)};
    return character;
  }
}
