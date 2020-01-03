using UnityEngine;
using System;

[Serializable]
public abstract class Equipment {
  public enum Tier {
    Crude, Simple, Sturdy, Quality, Flawless, Enchanted
  }

  public SerializableGuid guid;
  public Attributes attr = new Attributes();
  public Tier tier;

  //could be null if not equipped.
  [Obsolete("Equipment.equippedTo is deprecated. Equipment is being deduplicated.")]
  public Character equippedTo {
    get {
      return GameData.gameData.getCharacterWithItem(this);
    }
  }

  public bool isDefaultEquipment { get { return Equals(getDefault()); } }

  // return the default of the same type as e
  public abstract Equipment getDefault();

  public abstract string name();
  
  [Obsolete("Equipment.isEquipped is deprecated. Equipment is being deduplicated.")]
  public bool isEquipped() {
     return equippedTo != null;
  }

  public Equipment clone() {
    return MemberwiseClone() as Equipment;
  }

  public EquipType type;
}
