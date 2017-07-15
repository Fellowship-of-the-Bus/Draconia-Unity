using System;
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
    _equip = e;
    if (e == null) {
      return;
    }
    setTipbox();
  }

  public void OnPointerClick(PointerEventData eventData) {
    if (eventData.button == PointerEventData.InputButton.Left){
      if (!inCombineView) {
        onButtonClick();
      }
    } else if (eventData.button == PointerEventData.InputButton.Middle){
      //Debug.Log("Middle click");
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
      //no equipment or default do nothing.
      if (equip == null || equip.defaultEquipment) return;
      Character c = equip.equippedTo;
      c.unEquip(equip);

      //need default value so it doesnt complain about uninit variable
      Equipment def = new Weapon();
      if (equip is Weapon) {
        def = new Weapon("Unarmed", Weapon.kinds.Blunt, 1, 1);
      } else if (equip is Armour) {
        def = new Armour("Unarmed", Armour.ArmourKinds.Leather, 1);
      }
      c.equip(def);
      if (!equip.defaultEquipment){
        InvItemSelect.get.getTooltipWithEquipment(equip).updateColour();
      }
      equip = def;
    } else {
      //disallow equipping other characters items for now
      if (equip.equippedTo != null) return;
      var charEquip = inv.items[equip.type].equip;
      var tooltip = inv.items[equip.type];
      if (charEquip != null) {
        charEquip.equippedTo.unEquip(charEquip);
        if (!charEquip.defaultEquipment) {
          InvItemSelect.get.getTooltipWithEquipment(charEquip).updateColour();
        }
      }

      inv.selectedPanel.c.equip(equip);
      tooltip.setItem(equip);
      updateColour();
      tooltip.updateColour();
    }
    inv.updateAttrView();
  }

  private void onRightClick() {
    InvItemSelect inv = InvItemSelect.get;
    //from inventory, add it to materials
    //from materials remove it
    if (!inCombineView) {
      inv.addMaterial(equip);
    } else if (inCombineView) {
      if (inv.isResult(this) && equip != null) {
        inv.createUpgrade();
      } else {
        equip = null;
        inv.removeCreated();
      }
    }

  }

  protected override bool showTip() {
    init();
    return equip != null && !equip.defaultEquipment;
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
