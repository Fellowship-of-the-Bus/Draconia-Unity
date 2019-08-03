using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class HealthBarManager : MonoBehaviour {
  public GameObject healthBar;
  public GameObject damageBar;
  public GameObject healingBar;
  public CustomText textDisplay;

  private BattleCharacter character;
  private int offset;

  private void updateBar(GameObject bar, float health) {
    health = Math.Max(Math.Min(health, character.maxHealth), 0);
    Vector3 scale = bar.transform.localScale;
    scale.x = Math.Max(Math.Min((float)health/character.maxHealth,1),0);
    bar.transform.localScale = scale;
    textDisplay.text = ((int)health).ToString() + " / " + character.maxHealth.ToString();
  }

  public void update(int change = 0) {
    updateBar(damageBar, character.curHealth);
    updateBar(healthBar, character.curHealth);
    updateBar(healingBar, character.curHealth);
    if (change <= 0) {
      updateBar(healthBar, character.curHealth + change);
    } else {
      updateBar(healingBar, character.curHealth + change);
    }
  }

  public void updatePreview() {
    update(character.PreviewChange);
  }

  private IEnumerator animate(Action callback) {
    int NUM_STEPS = Options.FPS;
    float TIME_PER_STEP = 1/(NUM_STEPS * 1f);
    // if (offset > 0) {
    //   updateBar(healingBar, character.curHealth + offset);
    // } else {
    //   updateBar(healthBar, character.curHealth + offset);
    // }
    for(int i = 0; i < NUM_STEPS; i++) {
      float cur = Mathf.Lerp(character.curHealth, character.curHealth + offset, i * TIME_PER_STEP);
      if (offset > 0) {
        updateBar(healthBar, cur);
      } else {
        updateBar(damageBar, cur);
        updateBar(healingBar, cur);
      }
      yield return new WaitForSeconds(TIME_PER_STEP);
    }
    callback();
    update();
  }

  public void animateToNeutral(int change, Action callback) {
    offset = change;
    GameManager.get.waitFor(GameManager.get.StartCoroutine(animate(callback)));
  }

  public void setCharacter(BattleCharacter character) {
    this.character = character;
    if (character) update();
  }
}
