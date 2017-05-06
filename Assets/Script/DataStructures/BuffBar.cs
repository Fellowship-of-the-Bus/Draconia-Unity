using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Threading;
using System.Linq;

public class BuffBar {

  GameObject bar;
  GameObject button;

  public BuffBar(GameObject b, GameObject buttonPrefab) {
    get = this;
    bar = b;
    button = buttonPrefab;
  }

  public void update(BattleCharacter selected) { //Call every time selection changes
    foreach (Transform child in bar.transform) {
      if (child.gameObject) GameObject.Destroy(child.gameObject);
    }
    float offset = 0;
    foreach (Effect e in selected.getEffects()) {
      GameObject b = GameObject.Instantiate(button, bar.transform) as GameObject;
      b.transform.localPosition = new Vector3(offset,25,0);
      b.GetComponentsInChildren<Text>()[0].text = e.GetType().Name;
      offset += button.GetComponent<RectTransform>().rect.width;
      if (e is DurationEffect) {
        DurationEffect de = e as DurationEffect;
        b.AddComponent<Tooltip>();
        b.GetComponent<Tooltip>().tiptext = "Turns remaining: " + de.duration;
      }
      if (offset > bar.GetComponent<RectTransform>().rect.width) break;
    }
  }

  public static BuffBar get { get; private set; }
}
