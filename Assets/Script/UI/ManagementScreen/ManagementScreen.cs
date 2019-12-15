using UnityEngine;

public class ManagementScreen : MonoBehaviour {
  public Transform content;
  public CanvasGroup currentView;

  public void switchView(CanvasGroup newView) {
    if (currentView != newView) {
      show(newView);
      hide(currentView);
      currentView = newView;
    }
  }

  private void show(CanvasGroup group) {
    group.alpha = 1;
    group.interactable = true;
  }

  private void hide(CanvasGroup group) {
    group.alpha = 0;
    group.interactable = false;
  }
}
