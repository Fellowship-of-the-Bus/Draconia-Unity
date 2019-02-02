using UnityEngine;
using System.IO;

public class CharacterGenerator {

  private static readonly int STR_MIN = 5;
  private static readonly int STR_MAX = 10;

  private static readonly int INT_MIN = 5;
  private static readonly int INT_MAX = 10;

  private static readonly int SPEED_MIN = 5;
  private static readonly int SPEED_MAX = 10;

  private static readonly int HEALTH_MIN = 20;
  private static readonly int HEALTH_MAX = 30;

  private static Attributes generateBaseAttributes() {
    Attributes attr = new Attributes();

    //UnityEngine.Random.value;
    attr.strength = Random.Range(STR_MIN, STR_MAX+1);
    attr.intelligence = Random.Range(INT_MIN, INT_MAX+1);
    attr.speed = Random.Range(SPEED_MIN, SPEED_MAX+1);
    attr.maxHealth = Random.Range(HEALTH_MIN, HEALTH_MAX+1);

    return attr;
  }

  //Return the amount of exp necessary to go from level 1 to the given level.
  private static int expToLevel(int level) {
    return 100 * (level-1);
  }



  private static string generateName() {
    var lines = File.ReadAllLines("Assets/Resources/names.txt");
    int index = Random.Range(0, lines.Length);
    return lines[index];
  }

  public static Character generateCharacter(int level) {
    Character character = new Character();

    character.name = generateName();

    character.attr = generateBaseAttributes();

    character.gainExp(expToLevel(level));

    return character;
  }

}
