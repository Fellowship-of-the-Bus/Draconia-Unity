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
        tooltip.setItem(e);
        InvCharSelect charSelect = InvCharSelect.get;
        tooltip.updateColour();
        eqIndex++;
      }
    }
  }

  public ItemTooltip getTooltipWithEquipment(Equipment e) {
    if (e == null) return null;
    int index = groupedEquipment[e.equipmentClass].IndexOf(e);
    if (index == -1) return null;
    return scrollAreas[(int)e.equipmentClass].transform.GetChild(index).gameObject.GetComponent<ItemTooltip>();
  }

  public static InvItemSelect get { get; set; }
}
