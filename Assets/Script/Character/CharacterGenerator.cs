using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class CharacterGenerator {

  // private static readonly int STR_MIN = 5;
  // private static readonly int STR_MAX = 10;

  // private static readonly int INT_MIN = 5;
  // private static readonly int INT_MAX = 10;

  private static readonly int SPEED_MIN = 5;
  private static readonly int SPEED_MAX = 10;

  // private static readonly int HEALTH_MIN = 20;
  // private static readonly int HEALTH_MAX = 30;

  private static readonly int TRAIT_MIN = 2;
  private static readonly int TRAIT_MAX = 3;

  private static string[] lines;

  static CharacterGenerator() {
     lines = File.ReadAllLines("Assets/Resources/names.txt");
  }

  private static Attributes generateBaseAttributes() {
    Attributes attr = new Attributes();

    //UnityEngine.Random.value;
    attr.strength = 80;
    attr.intelligence = 80;
    attr.speed = Random.Range(SPEED_MIN, SPEED_MAX+1);
    attr.maxHealth = 250;
    attr.physicalDefense = 30;
    attr.magicDefense = 30;

    attr.moveRange = 4;

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

}
