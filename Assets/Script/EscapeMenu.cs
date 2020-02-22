using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EscapeMenu : MonoBehaviour {
  public Button overworldButton;
  public Button mainMenuButton;
  public Button retryButton;
  public Canvas menuCanvas;
  public FileBrowser saveBrowser;
  public FileBrowser loadBrowser;
  public OptionController optionsMenu;

  private readonly string escapeAxis = "EscapeMenu";
  private bool isAxisDown = false;

  public static EscapeMenu get { get; private set; }

  void Start() {
    EscapeMenu temp = get;
    if (!Singleton.makeSingleton(ref temp, this)) return;
    get = temp;
    transitionScenes("MainMenu");

    // ensure that referenced external objects are not deleted
    GameObject.DontDestroyOnLoad(saveBrowser.gameObject);
    GameObject.DontDestroyOnLoad(loadBrowser.gameObject);
    GameObject.DontDestroyOnLoad(optionsMenu.gameObject);
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
    retryButton.gameObject.SetActive(newScene != "MainMenu" && newScene != "OverWorld" && newScene != "PostMap");
  }

  public void hide() {
    get.menuCanvas.enabled = false;
  }

  public void resumeClicked() {
    hide();
  }

  public void optionsClicked() {
    optionsMenu.show();
    hide();
  }

  public void loadGameClicked() {
    loadBrowser.openBrowser();
    hide();
  }

  public void returnToOverworldClicked() {
    LoadingScreen.load("OverWorld");
    hide();
  }

  public void returnToMainMenuClicked() {
    LoadingScreen.load("MainMenu");
    hide();
  }

  public void quitGameClicked() {
    Application.Quit();
  }

  public void retryClicked() {
    Scene scene = SceneManager.GetActiveScene();
    LoadingScreen.load(scene.name);
    hide();
  }
}
