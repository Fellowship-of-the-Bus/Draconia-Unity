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
      //break links if necessary
      if (linkedTo != null) {
        linkedTo.linkedTo = null;
        linkedTo = null;
      }
      return;
    }
    setTipbox();
  }


  public void updateColour() {
    if (inCharacterView){
      return;
    }
    //green = unequipped
    //red = equipped to selected character
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
    InvCharSelect inv = InvCharSelect.get;
    if (inCharacterView) {
      if (equip == null) return;
      equip.equippedTo.unEquip(equip);

      linkedTo.updateColour();
      linkedTo.linkedTo = null;
      linkedTo = null;
      equip = null;
    } else {
      //disallow equipping other characters items for now
      if (equip.equippedTo != null) return;
      var charEquip = inv.items[equip.type].equip;
      var tooltip = inv.items[equip.type];
      if (charEquip != null) {
        charEquip.equippedTo.unEquip(charEquip);
        inv.items[equip.type].linkedTo.updateColour();
        //break previous link
        tooltip.linkedTo.linkedTo = null;
      }


      //set new link
      tooltip.linkedTo = this;
      linkedTo = tooltip;
      inv.selectedPanel.c.equip(equip);
      tooltip.setItem(equip);
      updateColour();
      tooltip.updateColour();
    }
    inv.updateAttrView();
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
