using UnityEngine;
using System.Collections.Generic;
using System;

public class TraitFactory : MonoBehaviour {

  public static TraitFactory get = null;
  public TextAsset traitInfo;
  void Awake() {
    if (get != null) {
      Destroy(gameObject);
      return;
    }
    get = this;
    DontDestroyOnLoad(gameObject);
    buildTraits();
  }
  private void buildTraits() {
    string[] lines = traitInfo.text.Split(new string[]{ "\n" }, System.StringSplitOptions.RemoveEmptyEntries);
    for (int i = 1; i < lines.Length; i++) {
      makeTrait(lines[i]);
    }
  }
  private void makeTrait(string info) {
    string[] fields = info.Split(',');
    AttrTraitName attrName;
    SpecTraitName specName;
    UniqueTraitName uniqueName;
    Trait t = new Trait();
    if (Enum.TryParse(fields[0], true, out attrName )) {
      attrTraits[attrName] = t;
    } else if (Enum.TryParse(fields[0], true, out specName)) {
      specTraits[specName] = t;
    } else if (Enum.TryParse(fields[0], true, out uniqueName)) {
      uniqueTraits[uniqueName] = t;
    } else {
      Channel.game.Log("Invalid trait name in traitInfo file");
    }
    t.name = fields[1];
    t.description = fields[2];
    t.attr.strength += int.Parse(fields[3])/100f;
    t.attr.intelligence += int.Parse(fields[4])/100f;
    t.attr.speed += int.Parse(fields[5])/100f;
    t.attr.maxHealth += int.Parse(fields[6])/100f;
    t.attr.physicalDefense += int.Parse(fields[7])/100f;
    t.attr.magicDefense += int.Parse(fields[8])/100f;
    t.attr.moveRange += int.Parse(fields[9]);
    t.attr.healingMultiplier += int.Parse(fields[10])/100f;
    t.spec.expGain += int.Parse(fields[11])/100f;
    int curIndex = 12;
    int num_wep = int.Parse(fields[curIndex]);
    curIndex++;
    for (int i = 0; i < num_wep; i++) {
      EquipmentClass e;
      if (Enum.TryParse(fields[curIndex + 2*i], true, out e)) {
        t.spec.wepSpec[e] += int.Parse(fields[curIndex + 2*i + 1])/100f;
      } else {
        Channel.game.Log("Invalid Equipment type in traitInfo file");
      }
    }
    curIndex += 2 * num_wep;
    int num_enemy = int.Parse(fields[curIndex]);
    curIndex++;
    for (int i = 0; i < num_enemy; i++) {
      EnemyType e;
      if (Enum.TryParse(fields[curIndex + 2*i], true, out e)) {
        t.spec.enemySpec[e] += int.Parse(fields[curIndex + 2*i + 1])/100f;
      } else {
        Channel.game.Log("Invalid EnemyType in traitInfo file");
      }
    }
    curIndex += 2 * num_enemy;
    int num_element = int.Parse(fields[curIndex]);
    curIndex++;
    for (int i = 0; i < num_element; i++) {
      DamageElement e;
      if (Enum.TryParse(fields[curIndex + 2*i], true, out e)) {
        t.spec.elementSpec[e] += int.Parse(fields[curIndex + 2*i + 1])/100f;
      } else {
        Channel.game.Log("Invalid Equipment type in traitInfo file");
      }
    }
  }
  [HideInInspector]
  public List<AttrTraitName> attrTraitNames = new List<AttrTraitName>((IEnumerable<AttrTraitName>)Enum.GetValues(typeof(AttrTraitName)));
  [HideInInspector]
  public List<SpecTraitName> specTraitNames = new List<SpecTraitName>((IEnumerable<SpecTraitName>)Enum.GetValues(typeof(SpecTraitName)));
  [HideInInspector]
  public List<UniqueTraitName> uniqueTraitNames = new List<UniqueTraitName>((IEnumerable<UniqueTraitName>)Enum.GetValues(typeof(UniqueTraitName)));
  public Dictionary<AttrTraitName, Trait> attrTraits = new Dictionary<AttrTraitName, Trait>();
  public Dictionary<SpecTraitName, Trait> specTraits = new Dictionary<SpecTraitName, Trait>();
  public Dictionary<UniqueTraitName, Trait> uniqueTraits = new Dictionary<UniqueTraitName, Trait>();

  public List<Trait> getRandomTraits(int numAttrTrait, int numSpecTrait) {
    List<int> attrRandomNumbers = new List<int>();
    List<int> specRandomNumbers = new List<int>();
    int attrTraitCount = attrTraitNames.Count;
    int specTraitCount = specTraitNames.Count;
    if (numAttrTrait > attrTraitCount || numSpecTrait > specTraitCount) {
      Channel.game.Log("Trying to get too many Traits, stopping");
      return new List<Trait>();
    }
    while (attrRandomNumbers.Count < numAttrTrait) {
      int rand = UnityEngine.Random.Range(0, attrTraitCount);
      if (attrRandomNumbers.Contains(rand)) {
        continue;
      } else {
        attrRandomNumbers.Add(rand);
      }
    }
    while (specRandomNumbers.Count < numSpecTrait) {
      int rand = UnityEngine.Random.Range(0, specTraitCount);
      if (specRandomNumbers.Contains(rand)) {
        continue;
      } else {
        specRandomNumbers.Add(rand);
      }
    }
    List<Trait> ret = new List<Trait>();
    foreach (int i in attrRandomNumbers) {
      ret.Add(attrTraits[attrTraitNames[i]]);
    }
    foreach (int i in specRandomNumbers) {
      ret.Add(specTraits[specTraitNames[i]]);
    }
    return ret;
  }

  public Trait getTrait(AttrTraitName a) {
    return attrTraits[a];
  }
  public Trait getTrait(SpecTraitName s) {
    return specTraits[s];
  }
  public Trait getTrait(UniqueTraitName u) {
    return uniqueTraits[u];
  }
}
