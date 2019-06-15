using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Linq;

public class BuffBar : MonoBehaviour {

  public GameObject button;

  void Start() {
    get = this;
  }

  public void update(BattleCharacter selected) { //Call every time selection changes
    foreach (Transform child in transform) {
      if (child.gameObject) GameObject.Destroy(child.gameObject);
    }
    float offset = 0;
    foreach (Effect eff in selected.getEffects()) {
      GameObject buff = GameObject.Instantiate(button, new Vector3 (0,0,0), Quaternion.identity, transform) as GameObject;

      // TODO: Stop using the types. Support images that aren't for skills.
      if (SkillList.get.skillImages.ContainsKey(eff.GetType())) {
        buff.GetComponent<Image>().sprite = SkillList.get.skillImages[eff.GetType()];
      }

      offset += button.GetComponent<RectTransform>().rect.width;
      buff.AddComponent<Tooltip>();
      buff.GetComponent<Tooltip>().tiptext = eff.tooltip;
    }
  }

  public static BuffBar get { get; private set; }
}
