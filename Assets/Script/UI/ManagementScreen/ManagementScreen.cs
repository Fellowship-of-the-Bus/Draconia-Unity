using UnityEngine;

public class ManagementScreen : MonoBehaviour {
  public Transform content;
  public CanvasGroup currentView;

  void Start() {
    for (int i = 0; i < content.childCount; ++i) {
      CanvasGroup group = content.GetChild(i).GetComponent<CanvasGroup>();
      if (i == 0) {
        show(group);
      } else {
        hide(group);
      }
    }
  }

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
