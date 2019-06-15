using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour {
  public static string nextScene;
  public static string loadingScene = "LoadingScreen";

  public GameObject loadingBar;
  public Canvas canvas;
  public CanvasGroup group;
  public float secondsToFade;

  private AsyncOperation async;
  private Scene newScene;
  private bool loadingFinished = false;

  void Start() {
    async = SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);
    // Run the garbage collector now to clean up the previous scene, to make GC less likely during the next scene
    System.GC.Collect();
    // Don't let the Scene activate until you allow it to
    async.allowSceneActivation = false;
    loadingFinished = false;
    StartCoroutine(updateLoadingScreen());
  }

  IEnumerator updateLoadingScreen() {
    while (! async.allowSceneActivation) {
      // Output the current progress
      loadingBar.transform.localScale = new Vector3(async.progress, 1f, 1f);

      // Check if the load has finished
      if (async.progress >= 0.9f) {
        // delay switching scenes until saving/loading is complete
        async.allowSceneActivation = ! SaveLoad.active;
      }
      yield return null;
    }

    // move loading bar now that scene is mostly loaded
    loadingBar.transform.localScale = new Vector3(.95f, 1f, 1f);

    while (! loadingFinished) {
      // don't set active scene until all Awake and Start functions have been called
      yield return null;
    }

    // loading has completely finished
    loadingBar.transform.localScale = new Vector3(1f, 1f, 1f);
    yield return null;

    // fade out the loading canvas
    int nFramesToFade = (int)(secondsToFade * Options.FPS);
    for (int i = 0; i < nFramesToFade; ++i) {
      group.alpha -= 1f/nFramesToFade;
      yield return null;
    }

    canvas.enabled = false;
    SceneManager.SetActiveScene(newScene);
    SceneManager.UnloadSceneAsync(loadingScene);
  }

  public static void load(string scene) {
    LoadingScreen.nextScene = scene;
    SceneManager.LoadSceneAsync(loadingScene);
  }

  void OnEnable() {
    SceneManager.sceneLoaded += OnLevelFinishedLoading;
  }

  void OnDisable() {
    SceneManager.sceneLoaded -= OnLevelFinishedLoading;
  }

  void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
    if (scene.name == loadingScene) return;
    Debug.AssertFormat(scene.name == nextScene, "expected to load {0}, was {1}", nextScene, scene.name);
    loadingFinished = true;
    newScene = scene;
  }
}
