using UnityEngine;

public class BattleCharacterUI : MonoBehaviour {
  public new CustomText name;
  public HealthBarManager healthBars;

  void Update() {
    // rotate overhead UI (health bar) to look at camera
    transform.rotation = Camera.main.transform.rotation; // Take care about camera rotation

    // scale health on health bar to match current HP values
    healthBars.updatePreview();
  }

  public void updateLifeBars(int change = 0) {
    healthBars.update(change);
  }
}
