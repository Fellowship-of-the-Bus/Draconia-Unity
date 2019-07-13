using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class ActionMeterController : MonoBehaviour {
  public Image actionDisplay;
  public Image previewDisplay;

  private BattleCharacter character;
  private float displayedAction = 0;

  private void setDisplay(Image display, float amount) {
    float fillAmount = Math.Max(Math.Min(amount / BattleCharacter.maxAction, 1), 0);
    display.fillAmount = fillAmount;
  }

  public void reset() {
    setDisplay(actionDisplay, character.curAction);
    setDisplay(previewDisplay, 0);
    displayedAction = character.curAction;
  }

  public IEnumerator animate() {
    int NUM_STEPS = Options.FPS;
    float TIME_PER_STEP = 1/(NUM_STEPS * 1f);
    float targetAmount = character.curAction;

    for(int i = 0; i < NUM_STEPS; i++) {
      if (character.curAction != targetAmount) {
        break;
      }

      float cur = Mathf.Lerp(displayedAction, targetAmount, i * TIME_PER_STEP);
      setDisplay(actionDisplay, cur);
      yield return new WaitForSeconds(TIME_PER_STEP);
    }
    reset();
  }

  public void previewChange(float change) {
    if (change > 0) {
      setDisplay(previewDisplay, displayedAction + change);
    } else {
      setDisplay(actionDisplay, displayedAction - change);
      setDisplay(previewDisplay, displayedAction);
    }
  }

  public void setCharacter(BattleCharacter character) {
    this.character = character;
    if (character) reset();
  }
}
