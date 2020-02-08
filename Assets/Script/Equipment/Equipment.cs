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
  private ItemData data;
  public ItemData itemData {
    get { return data; }
    set { 
      if (value != null) {
        data = value;
        refresh();
      } else {
        Debug.LogErrorFormat("Setting ItemData to null is not allowed: {0}", name());
      }
    }
  }
  public EquipType type;

  public Sprite image { get { return itemData.image; } }

  public GameObject model { get { return itemData.model; } }

  public string tooltip { get { return name() + "\n" + itemData.tooltip; } }

  protected virtual void refresh() {
    guid = itemData.guid;
    // attr = itemData.attr;
    tier = itemData.tier;
  }
  
  public bool isDefaultEquipment { get { return Equals(getDefault()); } }

  // return the default of the same type as e
  public abstract Equipment getDefault();

  public abstract string name();
  
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
