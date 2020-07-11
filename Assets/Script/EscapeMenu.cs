using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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

  public static EscapeMenu get { get; private set; }

  void Start() {
    EscapeMenu temp = get;
    if (!Singleton.makeSingleton(ref temp, this)) return;
    get = temp;

    InputSystem.AddCallback((InputActions actions) => actions.Default.EscapeMenu, onEscapeMenuTriggered);
    transitionScenes("MainMenu");

    // ensure that referenced external objects are not deleted
    GameObject.DontDestroyOnLoad(saveBrowser.gameObject);
    GameObject.DontDestroyOnLoad(loadBrowser.gameObject);
    GameObject.DontDestroyOnLoad(optionsMenu.gameObject);
  }

  private void onEscapeMenuTriggered(InputAction.CallbackContext context) {
    menuCanvas.enabled = ! menuCanvas.enabled;
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
