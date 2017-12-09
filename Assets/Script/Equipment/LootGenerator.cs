using UnityEngine;
using System;
using System.Collections.Generic;

public class LootGenerator {
  public static LootGenerator get {get; set;}
  //mapName => always dropped loot items
  public Dictionary<string, List<Equipment>> guaranteed = new Dictionary<string, List<Equipment>>();
  static bool shouldInit = true;
  public static void init() {
    if (!LootGenerator.shouldInit) return;
    LootGenerator.shouldInit = false;
    get = new LootGenerator();

    //add items in guaranteed loot items
    List<Equipment> l = new List<Equipment>();
    l.Add(new Weapon(EquipmentClass.Sword, 1, 1, Weapon.Kinds.Melee));
    get.guaranteed.Add("Map1", l);
  }

  public List<Equipment> getLoot(string mapName) {
    List<Equipment> loot;
    if (!guaranteed.ContainsKey(mapName)) {
      loot = new List<Equipment>();
    } else {
      loot = guaranteed[mapName];
    }
    loot.AddRange(genRandomLoot(mapName));
    return new List<Equipment>(loot.Map(x => x.clone()));
  }

  public List<Equipment> genRandomLoot(string mapName) {
    // some sort of random generation algorithm ...
    return new List<Equipment>();
  }

}
