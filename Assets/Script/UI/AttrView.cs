using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Reflection;


public class AttrView: MonoBehaviour {

  public Dictionary<string, Text> attrDict = new Dictionary<string,Text>();
  public GameObject text;

  void Start() {
    foreach(FieldInfo f in typeof(Attributes).GetFields()) {
      Text t = Instantiate(text, gameObject.transform).GetComponent<Text>();
      attrDict.Add(f.Name, t);
    }
  }

  public void updateAttr(Attributes attr) {
    foreach(FieldInfo f in typeof(Attributes).GetFields()) {
      attrDict[f.Name].text = f.Name + " " + f.GetValue(attr);
    }
  }

}
