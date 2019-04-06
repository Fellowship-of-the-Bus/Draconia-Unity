using UnityEngine;
using System;
using System.Collections;

[System.Serializable]
public class HealthBarManager {
  public GameObject healthBar;
  public GameObject damageBar;
  public GameObject healingBar;

  private BattleCharacter character;
  private int offset;

  private void updateBar(GameObject bar, int health) {
    Vector3 scale = bar.transform.localScale;
    scale.x = Math.Max(Math.Min((float)health/character.maxHealth,1),0);
    bar.transform.localScale = scale;
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
    int NUM_STEPS = 10;
    float TIME_PER_STEP = 0.1f;
    // if (offset > 0) {
    //   updateBar(healingBar, character.curHealth + offset);
    // } else {
    //   updateBar(healthBar, character.curHealth + offset);
    // }
    for(int i = 0; i < NUM_STEPS; i++) {
      int cur = (int)Mathf.Lerp(character.curHealth, character.curHealth + offset, i * TIME_PER_STEP);
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
