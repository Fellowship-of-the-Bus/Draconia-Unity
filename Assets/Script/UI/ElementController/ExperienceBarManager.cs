using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class ExperienceBarManager : MonoBehaviour {
  public GameObject experienceBar;
  public CustomText textDisplay;

  private Character character;
  private int displayedXP = -1;

  private void updateBar(GameObject bar, int xp) {
    if (xp != displayedXP) {
      int max = character.nextLevelExpDifference;

      Vector3 scale = bar.transform.localScale;
      scale.x = Math.Max(Math.Min((float)xp / max, 1), 0);
      bar.transform.localScale = scale;
      textDisplay.text = xp.ToString() + " / " + max.ToString();
      displayedXP = xp;
    }
  }

  public void update() {
    updateBar(experienceBar, character.curExp - character.expAtLevel(character.curLevel));
  }

  public void setCharacter(Character character) {
    this.character = character;
    update();
  }
}
