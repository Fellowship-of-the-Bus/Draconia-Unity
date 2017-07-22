using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemTooltipSimple : Tooltip {
  protected Equipment _equip;
  public Equipment equip {
    get { return _equip;}
    set { setItem(value);}
  }
  public AttrView attrView;
  public Text equipName;
  public Text equippedTo;

  // must override Start so that Tooltip Start is not called.
  void Start() {}

  protected bool onlyOnce = true;
  virtual public void init() {
    if (!onlyOnce) return;
    rectTrans = tipbox.GetComponent<RectTransform>();
    onlyOnce = false;
  }

  //also need to set the image eventually and colour
  public void setItem(Equipment e) {
    init();
    _equip = e;
    setTipbox();
  }

  protected override bool showTip() {
    init();
    return equip != null && !equip.isDefaultEquipment;
  }

  protected override void setTipbox() {
    if (equip == null) return;
    if (equip.isDefaultEquipment) tipbox.SetActive(false);
    attrView.updateAttr(equip.attr);
    equipName.text = equip.name();
    if (equip.equippedTo != null) {
      equippedTo.text = "Equipped To: " + equip.equippedTo.name;
    } else {
      equippedTo.text = "Equipped To: No one.";
    }
  }
}
