using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : MonoBehaviour {
  private Equipment _equipment;
  public Equipment equipment {
    get { return _equipment; }
    set { 
      _equipment = value;
      setTipbox(_equipment);
    }
  }
  public CharSelect charSelect;
  public AttrView attrView;
  public Text equipName;
  public Text equippedTo;
  public Tooltip tooltip;
  public GameObject tipbox;

  private bool showTip() {
    return equipment != null && !equipment.isDefaultEquipment;
  }

  private void setTipbox(Equipment equipment) {
    if (equipment == null) return;
    if (equipment.isDefaultEquipment) tooltip.gameObject.SetActive(false);
    attrView.updateAttr(equipment.attr);
    equipName.text = equipment.name();
    if (equipment.equippedTo != null) {
      equippedTo.text = "Equipped To: " + equipment.equippedTo.name;
    } else {
      equippedTo.text = "Equipped To: No one.";
    }
  }

  public void setTooltip(InventoryController controller) {
    attrView = controller.attrView;
    equipName = controller.equipName;
    equippedTo = controller.equippedTo;
    tipbox = controller.tooltip.tipbox;
  }

  public void updateColor() {
    Color c;
    if (equipment.isEquipped()) {
      c = Color.green;
    } else {
      c = Color.white;
    }
    gameObject.GetComponent<Image>().color = c;
  }
}
