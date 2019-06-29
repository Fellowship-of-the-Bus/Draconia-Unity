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
    if (get != null) {
      Destroy(gameObject);
      return;
    }
    get = this;
    names = namesFile.text.Split(new string[]{ "\n" }, System.StringSplitOptions.RemoveEmptyEntries);
    DontDestroyOnLoad(gameObject);
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

  public static Character generateCharacter(int level) {
    Character character = new Character();

    character.name = generateName();

    character.attr = generateBaseAttributes();

    character.setLevel(level);

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
    character.traits = new List<Trait>() {TraitFactory.get.getTrait(UniqueTraitName.brodric)};
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
    character.traits = new List<Trait>() {TraitFactory.get.getTrait(UniqueTraitName.sisdric)};
    return character;
  }
}
