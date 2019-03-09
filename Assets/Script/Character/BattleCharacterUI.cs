using UnityEngine;

public class BattleCharacterUI : MonoBehaviour {
  public CustomText name;
  public HealthBarManager healthBars;

  void Update() {
    // rotate overhead UI (health bar) to look at camera
    transform.rotation = Camera.main.transform.rotation; // Take care about camera rotation

    // scale health on health bar to match current HP values
    updateLifeBars();
  }

  public void updateLifeBars(int change = 0) {
    healthBars.update(change);
  }
}
