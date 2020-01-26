using UnityEngine;

public class EquipmentDB : MonoBehaviour {
  public WeaponData[] weapons;
  public ArmourData[] armour;
  public static EquipmentDB get;

  void Awake() {
    if (!Singleton.makeSingleton(ref get, this)) return;
  }

  public ItemData find(Equipment e) {
    if (e.guid.isValid()) {
      return findValidEquipment(e);
    } else {
      return findInvalidEquipment(e);
    }
  }

  public WeaponData findWeapon(SerializableGuid guid) {
    return weapons.Find((x) => pred(guid, x));
  }

  public ArmourData findArmour(SerializableGuid guid) {
     return armour.Find((x) => pred(guid, x));
  }

  public WeaponData defaultWeapon {
    get { return weapons[0]; }
  }

  public ArmourData defaultArmour {
    get { return armour[0]; }
  }

  private bool pred(SerializableGuid guid, ItemData data) {
    return guid.Equals(data.guid);
  }

  private ItemData findValidEquipment(Equipment e) {
    // assumes a valid guid
    if (e.type == EquipType.weapon) {
      return findWeapon(e.guid);
    } else if (e.type == EquipType.armour) {
      return findArmour(e.guid);
    } else {
      return null;
    }
  }

  private ItemData findInvalidEquipment(Equipment e) {
    // somehow equipment has an invalid guid
    // at runtime this is an error, but in editor
    // we should correct this
#if UNITY_EDITOR
    if (!Application.IsPlaying(gameObject)) {
      Debug.LogWarningFormat(gameObject, "Trying to find equipment with invalid GUID; correcting to default");
      if (e.type == EquipType.weapon) {
        WeaponData defaultValue = defaultWeapon;
        e.guid = defaultValue.guid;
        return defaultValue;
      } else if (e.type == EquipType.armour) {
        ArmourData defaultValue = defaultArmour;
        e.guid = defaultValue.guid;
        return defaultValue;
      } else {
        return null;
      }
    }
#endif
    Debug.LogErrorFormat(gameObject, "Attempting to find an equipment with an invalid GUID");
    return null;
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
