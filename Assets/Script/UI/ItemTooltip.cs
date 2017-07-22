using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemTooltip : Tooltip, IPointerClickHandler {
  private Equipment _equip;
  public Equipment equip {
    get { return _equip;}
    set { setItem(value);}
  }
  public AttrView attrView;
  public Text equipName;
  public Text equippedTo;

  //if false it is in the list of equipments
  //if true it is on character
  public bool inCharacterView = false;
  public bool inCombineView = false;

  // must override Start so that Tooltip Start is not called.
  void Start() {}

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
    _equip = e;
    setTipbox();
  }

  public void OnPointerClick(PointerEventData eventData) {
    if (eventData.button == PointerEventData.InputButton.Left){
      if (!inCombineView) {
        onButtonClick();
      }
    } else if (eventData.button == PointerEventData.InputButton.Right){
      if (!inCharacterView){
        onRightClick();
      }
    }
  }

  public void updateColour() {
    if (inCharacterView){
      return;
    }
    List<Color> colors = new List<Color>();
    if (InvItemSelect.get.isMaterial(equip)) {
      // material
      colors.Add(Color.red);
    }
    if (equip.equippedTo != null) {
      // equipped
      colors.Add(Color.green);
    }
    Color c;
    if (colors.Count == 0) {
      c = Color.white;
    } else {
      c = colors.Aggregate((Color a, Color b) => a+b);
    }
    gameObject.GetComponent<Image>().color = c;
  }

  public void onButtonClick(){
    InvCharSelect inv = InvCharSelect.get;
    if (inCharacterView) {
      // unequip item from character.
      // no equipment or default do nothing
      if (equip == null || equip.isDefaultEquipment) return;
      Character c = equip.equippedTo;
      c.unEquip(equip);

      // need default value so it doesnt complain about uninit variable
      Equipment def = equip.getDefault();
      c.equip(def);
      if (!equip.isDefaultEquipment){
        InvItemSelect.get.getTooltipWithEquipment(equip).updateColour();
      }
      equip = def;
    } else {
      // equip item to character.
      // disallow equipping other characters items for now.
      if (equip.equippedTo != null) return;
      var charEquip = inv.items[equip.type].equip;
      var tooltip = inv.items[equip.type];
      if (charEquip != null) {
        charEquip.equippedTo.unEquip(charEquip);
        if (! charEquip.isDefaultEquipment) {
          InvItemSelect.get.getTooltipWithEquipment(charEquip).updateColour();
        }
      }
      inv.selectedPanel.c.equip(equip);
      tooltip.setItem(equip);
      updateColour();
    }
    inv.updateAttrView();
  }

  private void onRightClick() {
    InvItemSelect inv = InvItemSelect.get;
    Equipment curEquip = equip;
    if (inCombineView) {
      if (inv.isResult(this) && equip != null) {
        // item is combine result, create it.
        inv.createUpgrade();
      } else {
        // remove item from materials
        equip = null;
        inv.removeCreated();
        tipbox.SetActive(false);
      }
    } else {
      // from inventory, add it to materials
      inv.addMaterial(equip);
    }
    ItemTooltip tip = inv.getTooltipWithEquipment(curEquip);
    if (tip != null) {
      tip.updateColour();
    }
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
