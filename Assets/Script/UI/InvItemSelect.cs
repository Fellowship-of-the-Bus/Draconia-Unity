using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Linq;

public class InvItemSelect: MonoBehaviour {
  // prefabs
  public GameObject equipPanel;
  public GameObject scrollView;
  public GameObject tabButton;

  // actual objects
  private GameObject currentScrollView; // currently visible scroll view
  private List<GameObject> scrollAreas = new List<GameObject>(); // content areas
  public Transform tabs;

  public ItemTooltip[] materials;
  private int numMaterials;
  private Dictionary<EquipmentClass, List<Equipment>> groupedEquipment;

  private EquipmentClass equipmentClass;
  private GameObject visibleScrollArea {
    get { return scrollAreas[(int)equipmentClass]; }
  }
  private List<Equipment> visibleEquipment {
    get { return groupedEquipment.safeGet(equipmentClass); }
  }

  void Awake() {
    groupedEquipment = GameData.gameData.inv.equipments.GroupBy(x => x.equipmentClass).ToDictionary(gdc => gdc.Key, gdc => gdc.ToList());

    // add tabs that control which equipment is visible
    foreach (EquipmentClass ec in Enum.GetValues(typeof(EquipmentClass))) {
      if (ec == EquipmentClass.Unarmed) continue;
      // create scroll view for each kind of equipment
      GameObject newScrollView = Instantiate(scrollView, transform, false);
      newScrollView.SetActive(false);  // all scroll views are inactive at the start
      newScrollView.transform.SetSiblingIndex((int)ec);
      GameObject scrollArea = newScrollView.transform.Find("Viewport/Content").gameObject;
      scrollAreas.Add(scrollArea);
      // create tab button for each kind of equipment
      GameObject tbutton = Instantiate(tabButton, tabs, false);
      Text buttonName = tbutton.transform.Find("Text").GetComponent<Text>();
      buttonName.text = ec.ToString();  // name the tab button
      // hide/unhide on tab button press
      Button b = tbutton.GetComponent<Button>();
      EquipmentClass currentEC = ec;
      b.onClick.AddListener(() => {
        currentScrollView.SetActive(false);
        newScrollView.SetActive(true);
        currentScrollView = newScrollView;
        equipmentClass = currentEC;
      });

      // fill the scroll view's content area with one equipment panel
      // for each existing equipment of that kind
      foreach (Equipment e in groupedEquipment.safeGet(ec)) {
        Instantiate(equipPanel, scrollArea.transform);
      }
    }

    get = this;
  }

  void Start() {
    numMaterials = materials.Length-1;
    foreach (EquipmentClass ec in Enum.GetValues(typeof(EquipmentClass))) {
      if (ec == EquipmentClass.Unarmed) continue;
      var scrollView = transform.GetChild((int)ec).gameObject;
      if ((int)ec == 0) {
        // set first scrollView as selected view
        currentScrollView = scrollView;
        scrollView.SetActive(true);
        equipmentClass = ec;
      }
      var scrollArea = scrollView.transform.Find("Viewport/Content").gameObject;
      int eqIndex = 0;
      foreach (Equipment e in groupedEquipment.safeGet(ec)) {
        // create an item tooltip for each piece of equipment
        var equipPanel = scrollArea.transform.GetChild(eqIndex).gameObject;
        ItemTooltip tooltip = equipPanel.GetComponent<ItemTooltip>();
        tooltip.inCharacterView = false;
        tooltip.inCombineView = false;
        tooltip.setItem(e);
        InvCharSelect charSelect = InvCharSelect.get;
        tooltip.updateColour();
        eqIndex++;
      }
    }

    //
    foreach (ItemTooltip tooltip in materials) {
      tooltip.inCharacterView = false;
      tooltip.inCombineView = true;
    }
  }

  public ItemTooltip getTooltipWithEquipment(Equipment e) {
    if (e == null) return null;
    int index = groupedEquipment[e.equipmentClass].IndexOf(e);
    if (index == -1) return null;
    return scrollAreas[(int)e.equipmentClass].transform.GetChild(index).gameObject.GetComponent<ItemTooltip>();
  }

  //called to add an item as a material
  public void addMaterial(Equipment equip) {
    //check we do not have this already
    if (isMaterial(equip)) {
      return;
    }

    //add it to an empty slot
    bool added = false;
    for(int i = 0; i < numMaterials; i++) {
      if (materials[i].equip == null) {
        materials[i].setItem(equip);
        added = true;
        break;
      }
    }

    //check we have 3 materials
    for (int i = 0; i < numMaterials; i++) {
      if (materials[i].equip == null) {
        return;
      }
    }
    //if we added the third material
    if (added) {
      generateUpgrade();
    }
  }

  public bool isMaterial(Equipment e) {
    return Array.Find(materials, (ItemTooltip t) => t.equip == e) != null;
  }

  public bool isResult(ItemTooltip tooltip) {
    return tooltip == materials[numMaterials];
  }
  //called when 3 materials are put into slots
  public void generateUpgrade() {
    //check materials are compatible
    //for now check for same tier and type (weapon, armour)
    int tier = -1;
    EquipType type = null;
    for (int i = 0; i< numMaterials; i++) {
      if (tier == -1) {
        tier = materials[i].equip.tier;
        type = materials[i].equip.type;
      } else if (!(tier == materials[i].equip.tier && type == materials[i].equip.type)) {
        return;
      }
    }

    Equipment e = GameData.gameData.inv.combine(materials[0].equip, materials[1].equip, materials[2].equip);

    //add new equipment to created slot to preview
    AttrView view = InventoryController.get.attrView;
    Text equipName = InventoryController.get.equipName;
    Text equippedTo = InventoryController.get.equippedTo;

    //make sure this does not change the attribute view
    Attributes attr = view.curAttr;
    string name = equipName.text;
    string charaName = equippedTo.text;

    materials[numMaterials].setItem(e);

    view.updateAttr(attr);
    equipName.text = name;
    equippedTo.text = charaName;
  }

  //called when actually creating item
  public void createUpgrade() {
    //remove each tooltip from the inventory view
    Equipment material1 = materials[0].equip;
    Equipment material2 = materials[1].equip;
    Equipment material3 = materials[2].equip;
    List<ItemTooltip> toDelete = new List<ItemTooltip>();
    foreach (Equipment e in GameData.gameData.inv.equipments) {
      if (e == material1 || e == material2 || e == material3) {
        toDelete.Add(getTooltipWithEquipment(e));
      }
    }
    foreach (ItemTooltip tooltip in toDelete) {
      //detach from scrollArea
      tooltip.gameObject.transform.SetParent(null);
      Destroy(tooltip.gameObject);
    }

    //remove from inventory
    deleteEquipment(material1);
    deleteEquipment(material2);
    deleteEquipment(material3);

    Equipment created = materials[numMaterials].equip;

    GameData.gameData.inv.addEquipment(created);
    groupedEquipment[created.equipmentClass].Add(created);

    //make new tooltip
    GameObject display = Instantiate(equipPanel, scrollAreas[(int)created.equipmentClass].transform);
    ItemTooltip newTooltip = display.GetComponent<ItemTooltip>();
    newTooltip.inCharacterView = false;
    newTooltip.inCombineView = false;
    newTooltip.equip = created;
    newTooltip.updateColour();
    //reset materials tooltips
    foreach (ItemTooltip tooltip in materials) {
      tooltip.setItem(null);
    }
  }

  private void deleteEquipment(Equipment e) {
    if (e.equippedTo != null) {
      // ensure character always has a weapon -- equip default
      Character c = e.equippedTo;
      Equipment def = e.getDefault();
      c.equip(def);
      if (c == InvCharSelect.get.selectedPanel.c) {
        InvCharSelect.get.items[e.type].equip = def;
      }
    }
    // update inventory and grouped inventory to remove the item
    GameData.gameData.inv.deleteEquipment(e);
    groupedEquipment[e.equipmentClass].Remove(e);
  }

  public void removeCreated() {
    materials[numMaterials].equip = null;
  }

  public static InvItemSelect get { get; set; }

}
