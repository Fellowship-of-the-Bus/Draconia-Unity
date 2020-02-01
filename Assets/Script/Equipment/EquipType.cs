using System;
using UnityEngine;

// fake enum: we are doing this instead of just casting to allow implicit casts...
[System.Serializable]
public class EquipType {
  readonly int type;
  public readonly static EquipType weapon = new EquipType(0);
  public readonly static EquipType armour = new EquipType(1);

  public EquipType(int i) {
    type = i;
  }

  public static implicit operator int(EquipType x) {
    return x.type;
  }

  public override string ToString() {
    if (this == weapon) {
      return "Weapon";
    } else if (this == armour) {
      return "Armour";
    } else {
      Debug.LogErrorFormat("ToString: Invalid equipment: {0}", type);
      return "InvalidEquipment";
    }
  }

  //assume we are comparing non-null types.
  public static bool operator==(EquipType a, EquipType b) {
    return a.type == b.type;
  }
  public static bool operator!=(EquipType a, EquipType b) {
    return a.type != b.type;
  }

  public override bool Equals(object other) {
    return other is EquipType && ((EquipType)other).type == type;
  }

  public override int GetHashCode() {
    return type;
  }
}
