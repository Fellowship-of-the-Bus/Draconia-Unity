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

  public Tooltip tooltip;
  public Button button;
  public Image image;

  private bool showTip() {
    return equipment != null && !equipment.isDefaultEquipment;
  }

  private void setTipbox(Equipment equipment) {
    if (equipment == null) return;
    if (equipment.isDefaultEquipment) tooltip.gameObject.SetActive(false);
    // equipName.text = equipment.name();
    // if (equipment.image != null) {
    //   image.sprite = equipment.image;
    // }
  }

  public void updateColor(Color c) {
    gameObject.GetComponent<Image>().color = c;
  }
}
