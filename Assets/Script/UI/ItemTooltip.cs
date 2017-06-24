using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemTooltip : Tooltip {
  public Equipment equip;
  public AttrView attrView;
  public Text equipName;
  public Text equippedTo;

  //if false it is in the list of equipments
  //if true it is on character
  public bool inCharacterView = false;
  public ItemTooltip linkedTo;

  void Start() {
  }

  bool onlyOnce = true;
  public void init() {
    if (!onlyOnce) return;
    attrView = InventoryController.get.attrView;
    equipName = InventoryController.get.equipName;
    equippedTo = InventoryController.get.equippedTo;
    tipbox = InventoryController.get.tooltip;
    rectTrans = tipbox.GetComponent<RectTransform>();
    onlyOnce = false;
  }

  //also need to set the image eventually and colour
  public void setItem(Equipment e) {
    init();
    equip = e;
    if (e == null) {
      return;
    }
    setTipbox();
  }


  public void updateColour() {
    if (inCharacterView){
      return;
    }
    if (equip.equippedTo != null) {
      gameObject.GetComponent<Image>().color = Color.green;
    } else {
      gameObject.GetComponent<Image>().color = Color.white;
    }

  }
  float doubleClickStart = 0;
  float doubleClickInterval = 0.5f;
  public void onButtonClick(){
    if ((Time.time - doubleClickStart) < doubleClickInterval){
      onDoubleClick();
      doubleClickStart = -1;
    } else {
      doubleClickStart = Time.time;
    }
  }
  private void onDoubleClick() {
    if (inCharacterView) {
      equip.equippedTo.unEquip(equip);

      linkedTo.updateColour();
      linkedTo.linkedTo = null;
      linkedTo = null;
    } else {
      InvCharSelect inv = InvCharSelect.get;
      var charEquip = inv.items[equip.type].equip;
      charEquip.equippedTo.unEquip(charEquip);

      var tooltip = inv.items[equip.type];
      tooltip.linkedTo = this;
      linkedTo = tooltip;
      inv.selectedPanel.c.equip(equip);
      updateColour();
    }
  }

  protected override bool showTip() {
    return equip != null;
  }

  protected override void setTipbox() {
    if (equip == null) return;
    attrView.updateAttr(equip.attr);
    equipName.text = equip.name();
    if (equip.equippedTo != null) {
      equippedTo.text = "Equipped To: " + equip.equippedTo.name;
    } else {
      equippedTo.text = "Equipped To: No one.";
    }
  }
}
