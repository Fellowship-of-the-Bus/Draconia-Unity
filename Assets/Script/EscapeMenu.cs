using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EscapeMenu : MonoBehaviour {
  public Button overworldButton;
  public Button mainMenuButton;
  public Canvas menuCanvas;

  private readonly string escapeAxis = "EscapeMenu";
  private bool isAxisDown = false;

  public static EscapeMenu get { get; private set; }

  void Start() {
    if (get != null) {
      Destroy(gameObject);
      return;
    }
    get = this;
    DontDestroyOnLoad(gameObject);

    transitionScenes("MainMenu");
  }

  void Update() {
    if (!LoadingScreen.active) {
      if (! isAxisDown && Input.GetButtonDown(escapeAxis)) {
        menuCanvas.enabled = ! menuCanvas.enabled;
        isAxisDown = true;
      } else if (isAxisDown && Input.GetButtonUp(escapeAxis)) {
        isAxisDown = false;
      }
    }
  }

  public void transitionScenes(string newScene) {
    mainMenuButton.gameObject.SetActive(newScene != "MainMenu");
    overworldButton.gameObject.SetActive(newScene != "MainMenu" && newScene != "OverWorld");
  }

  public void hide() {
    get.menuCanvas.enabled = false;
  }

  public void resumeClicked() {
    hide();
  }

  public void optionsClicked() {
    Debug.Log("optionsClicked");
  }

  public void loadGameClicked() {
    Debug.Log("loadGameClicked");
  }

  public void returnToOverworldClicked() {
    LoadingScreen.load("OverWorld");
  }

  public void returnToMainMenuClicked() {
    LoadingScreen.load("MainMenu");
  }

  public void quitGameClicked() {
    Application.Quit();
  }
}
