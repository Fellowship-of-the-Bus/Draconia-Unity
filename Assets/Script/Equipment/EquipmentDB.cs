using UnityEngine;

public class EquipmentDB : MonoBehaviour {
  public WeaponData[] weapons;
  public ArmourData[] armour;
  public static EquipmentDB get;

  void Awake() {
    if (get != null) {
      Destroy(gameObject);
      return;
    }
    get = this;
    DontDestroyOnLoad(gameObject);
  }

  public ItemData find(Equipment e) {
    if (e.type == EquipType.weapon) {
      return findWeapon(e.guid);
    } else if (e.type == EquipType.armour) {
      return findArmour(e.guid);
    } else {
      return null;
    }
  }

  public WeaponData findWeapon(SerializableGuid guid) {
    return weapons.Find((x) => pred(guid, x));
  }

  public ArmourData findArmour(SerializableGuid guid) {
     return armour.Find((x) => pred(guid, x));
  }

  private bool pred(SerializableGuid guid, ItemData data) {
    return guid.Equals(data.guid);
  }

#if UNITY_EDITOR  // needed to hook up equipment without a GUID
  public ItemData slowFind(Equipment e) {
    if (e == null) {
      return null;
    } else if (e.type == EquipType.weapon) {
      return weapons.Find((x) => slowPred((Weapon)e, x));
    } else if (e.type == EquipType.armour) {
      return armour.Find((x) => slowPred((Armour)e, x));
    } else {
      return null;
    }
  }

  private bool slowPred(Weapon equipment, WeaponData data) {
    return data.kind == equipment.kind
      && data.tier == equipment.tier
      && data.equipmentClass == equipment.equipmentClass;
  }

  private bool slowPred(Armour equipment, ArmourData data) {
    return data.tier == equipment.tier
      && data.equipmentClass == equipment.equipmentClass;
  }
#endif
}
