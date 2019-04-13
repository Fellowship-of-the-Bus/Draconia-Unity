using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class CharacterGenerator {

  public static readonly int STR_BASE = 80;

  public static readonly int INT_BASE = 80;

  public static readonly int SPEED_BASE = 50;

  public static readonly int HEALTH_BASE = 250;

  public static readonly int PDEF_BASE = 30;

  public static readonly int MDEF_BASE = 30;

  public static readonly int MRANGE_BASE = 4;

  public static readonly int TRAIT_MIN = 2;
  public static readonly int TRAIT_MAX = 3;

  private static string[] lines;

  static CharacterGenerator() {
     lines = File.ReadAllLines("Assets/Resources/names.txt");
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

  //Return the amount of exp necessary to go from level 1 to the given level.
  private static int expToLevel(int level) {
    return 100 * (level-1);
  }

  private static List<Trait> generateTraits() {
    return Trait.TraitFactory.getRandomTraits(Random.Range(TRAIT_MIN, TRAIT_MAX+1));
  }

  private static string generateName() {
    int index = Random.Range(0, lines.Length);
    return lines[index];
  }

  public static Character generateCharacter(int level) {
    Character character = new Character();

    character.name = generateName();

    character.attr = generateBaseAttributes();

    character.gainExp(expToLevel(level));

    character.traits = generateTraits();

    return character;
  }

  public static Character generateBrodric() {
    Character character = new Character();
    character.name = "Brodric";
    character.attr = generateBaseAttributes();
    //give additional stats unique to brodric
    character.attr.strength += 5;
    character.attr.physicalDefense += 3;
    character.attr.maxHealth += 25;
    //give some traits?
    character.traits = new List<Trait>() {new Trait(TraitName.strPlus), new Trait(TraitName.maxHPPlus)};
    return character;
  }

  public static Character generateSisdric() {
    Character character = new Character();
    character.name = "Sisdric";
    character.attr = generateBaseAttributes();
    //give additional stats unique to brodric
    character.attr.intelligence += 5;
    character.attr.magicDefense += 3;
    character.attr.maxHealth += 25;
    //give some traits?
    character.traits = new List<Trait>() {new Trait(TraitName.intPlus), new Trait(TraitName.mDefPlus)};
    return character;
  }
}
