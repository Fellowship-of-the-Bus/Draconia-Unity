using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Reflection;


public class AttrView: MonoBehaviour {

  public Dictionary<string, Text> attrDict = new Dictionary<string,Text>();
  public GameObject text;
  public Attributes curAttr;

  void Awake() {
    initFields();
  }

  bool onceOnly = true;
  public void initFields() {
    if (!onceOnly) return;
    foreach(FieldInfo f in typeof(Attributes).GetFields()) {
      Text t = Instantiate(text, transform).GetComponent<Text>();
      attrDict.Add(f.Name, t);
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
        attrDict[f.Name].text = f.Name + " " + value;
        attrDict[f.Name].enabled = !value.Equals(f.GetValue(baseAttr));
      } else {
        throw(new Exception(f.Name));
      }
    }
  }

}
