using System;

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

  //assume we are comparing non-null types.
  public static bool operator==(EquipType a, EquipType b) {
    return a.type == b.type;
  }
  public static bool operator!=(EquipType a, EquipType b) {
    return a.type != b.type;
  }

  public override bool Equals(Object other) {
    return other is EquipType && ((EquipType)other).type == type;
  }

  public override int GetHashCode() {
    return type;
  }
}
