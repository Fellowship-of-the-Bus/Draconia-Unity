using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public abstract class Equipment : IEquatable<Equipment> {
  public enum Tier {
    Crude, Simple, Sturdy, Quality, Flawless, Enchanted
  }

  public SerializableGuid guid;
  public Attributes attr = new Attributes();
  public Tier tier;

  [NonSerialized]
  public ItemData itemData;
  public EquipType type;

  public Sprite image { get { return itemData.image; } }

  public GameObject model { get { return itemData.model; } }

  public string tooltip { get { return itemData.tooltip; } }

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

  public override bool Equals(object obj) {
    return Equals(obj as Equipment);
  }

  public bool Equals(Equipment other) {
    if (object.ReferenceEquals(other, null)) return false;
    return guid == other.guid;
  }

  public override int GetHashCode() {
    return guid.GetHashCode();
  }

  public static bool operator==(Equipment a, Equipment b) {
    if (object.ReferenceEquals(a, null)) {
      return object.ReferenceEquals(b, null);
    } 
    return a.Equals(b);
  }

  public static bool operator!=(Equipment a, Equipment b) {
      return !(a == b);
  }
}
