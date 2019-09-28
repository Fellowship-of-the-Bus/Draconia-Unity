using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemTooltip : ItemTooltipSimple, IPointerClickHandler {
  //if false it is in the list of equipments
  //if true it is on character
  public bool inCharacterView = false;

  override public void init() {
    if (!onlyOnce) return;
    attrView = OldManagementScreen.InventoryController.get.attrView;
    equipName = OldManagementScreen.InventoryController.get.equipName;
    equippedTo = OldManagementScreen.InventoryController.get.equippedTo;
    tipbox = OldManagementScreen.InventoryController.get.tooltip;
    base.init();
  }

  public void OnPointerClick(PointerEventData eventData) {
    if (eventData.button == PointerEventData.InputButton.Left){
      onButtonClick();
    }
  }

  public void updateColour() {
    if (inCharacterView){
      return;
    }
    List<Color> colors = new List<Color>();
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
      var tooltip = inv.items[equip.type] as ItemTooltip;
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
}
