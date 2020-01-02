using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Items/WeaponData", order = 100)]
public class WeaponData : ItemData {
  public Weapon.EquipmentClass equipmentClass;
  public int range;
  public Weapon.Kind kind;
}
