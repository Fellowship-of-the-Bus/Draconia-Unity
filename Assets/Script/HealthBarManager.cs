using UnityEngine;
using System;

[System.Serializable]
public class HealthBarManager {
  public GameObject healthBar;
  public GameObject damageBar;
  public GameObject healingBar;

  private BattleCharacter character;

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

  public void setCharacter(BattleCharacter character) {
    this.character = character;
    if (character) update();
  }
}
