using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Reflection;


public class AttrView: MonoBehaviour {
  public Dictionary<string, CustomText> attrDict = new Dictionary<string,CustomText>();
  public GameObject text;
  public Attributes curAttr;

  void Awake() {
    initFields();
  }

  bool onceOnly = true;
  public void initFields() {
    if (!onceOnly) return;
    foreach(FieldInfo f in typeof(Attributes).GetFields()) {
      GameObject attrField = Instantiate(text, transform);
      CustomText attrName = attrField.transform.Find("AttrName").GetComponent<CustomText>();
      attrName.text = f.Name;

      CustomText attrValue = attrField.transform.Find("AttrValue").GetComponent<CustomText>();
      attrDict.Add(f.Name, attrValue);
    }
    onceOnly = false;
  }

  public void updateAttr(Attributes attr) {
    initFields();
    Attributes baseAttr = new Attributes();
    curAttr = attr;
    foreach(FieldInfo f in typeof(Attributes).GetFields()) {
      if (attrDict.ContainsKey(f.Name)) {
        var value = f.GetValue(attr);
        attrDict[f.Name].text = value.ToString();
      } else {
        throw(new Exception(f.Name));
      }
    }
  }

}
