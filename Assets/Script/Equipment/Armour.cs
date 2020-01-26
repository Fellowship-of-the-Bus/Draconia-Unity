using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;
using System.Runtime.Serialization;

[System.Serializable]
public class Armour : Equipment {
  [FormerlySerializedAs("newEquipmentClass")]
  public EquipmentClass equipmentClass;

  public enum EquipmentClass {
    Shield, Metal, Robe, Leather, Unarmed
  }

  public ArmourData armourData { 
    get { return (ArmourData)itemData; }
    set { armourData = value; }
  }

  public static Armour defaultArmour {
    get { return new Armour(EquipmentDB.get.armour[0]); }
  }

  public override Equipment getDefault() { return defaultArmour; }

  public Armour(ArmourData armourData) {
    this.itemData = armourData;
  }

  protected override void refresh() {
    base.refresh();
    this.equipmentClass = armourData.equipmentClass;
    this.type = EquipType.armour;
  }

  public override string name() {
    return tier.ToString() + " " + equipmentClass;
  }

  // C# deserialization finished
  [OnDeserialized]
  private void onPostDeserialize(StreamingContext context) {
    this.guid.onPostDeserialize(context); // ensure guid is deserialized first
    this.itemData = EquipmentDB.get.findArmour(guid);
  }


#if UNITY_EDITOR
  // this constructor is necessary because of unity serialization shenanigans.
  // Without this definition, unity default-initializes the type when creating
  // an object to display in the inspector. Since Armour has a custom inspector that
  // uses the equipment type, it must be set in this default constructor, which unity
  // calls in editor even though it's private. If this constructor doesn't exist, 
  // every Armour thinks its a Weapon and the item data cannot be found in the 
  // equipment database.
  private Armour() {
    this.type = EquipType.armour;
  }
#endif
}
