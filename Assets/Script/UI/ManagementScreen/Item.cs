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

  public Button button;
  public Image image;
  public Tooltip tooltip;

  private bool showTip() {
    return equipment != null && !equipment.isDefaultEquipment;
  }

  private void setTipbox(Equipment equipment) {
    if (equipment == null) return;
    // equipName.text = equipment.name();
    gameObject.name = equipment.name();
    if (equipment.image != null) {
      image.sprite = equipment.image;
    }
  }

  public void updateColor(Color c) {
    image.color = c;
  }
}
