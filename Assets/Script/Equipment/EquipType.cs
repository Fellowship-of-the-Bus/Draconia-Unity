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
}
