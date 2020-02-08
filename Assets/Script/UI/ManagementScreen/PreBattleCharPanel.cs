using UnityEngine;
using UnityEngine.UI;

public class PreBattleCharPanel : MonoBehaviour {
  public CharPanel charPanel;
  public Button selectButton;

  void OnEnable() {
    selectButton.onClick.AddListener(onSelectButtonClick);
  }

  void OnDisable() {
    selectButton.onClick.RemoveListener(onSelectButtonClick);
  }

  private void onSelectButtonClick() {
    CharIntoLevel.get.addCharacter(charPanel.character);
  }
}
