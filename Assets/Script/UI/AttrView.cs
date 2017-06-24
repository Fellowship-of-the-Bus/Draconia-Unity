using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Reflection;


public class AttrView: MonoBehaviour {

  public Dictionary<string, Text> attrDict = new Dictionary<string,Text>();
  public GameObject text;

  void Awake() {
    initFields();
  }

  bool onceOnly = true;
  public void initFields() {
    if (!onceOnly) return;
    foreach(FieldInfo f in typeof(Attributes).GetFields()) {
      Text t = Instantiate(text, gameObject.transform).GetComponent<Text>();
      attrDict.Add(f.Name, t);
    }
    onceOnly = false;
  }

  public void updateAttr(Attributes attr) {
    initFields();
    foreach(FieldInfo f in typeof(Attributes).GetFields()) {
      if (attrDict.ContainsKey(f.Name)) {
        attrDict[f.Name].text = f.Name + " " + f.GetValue(attr);
      } else {
        Debug.Log(f.Name);
        throw(new Exception());
      }
    }
  }

}