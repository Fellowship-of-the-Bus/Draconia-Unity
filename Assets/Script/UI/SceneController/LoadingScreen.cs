﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour {
  public static string nextScene;

  private AsyncOperation async;
  public GameObject loadingBar;

  void Start() {
    async = SceneManager.LoadSceneAsync(nextScene);
    // Don't let the Scene activate until you allow it to
    // async.allowSceneActivation = false;
  }

  void Update() {
    //Output the current progress
    loadingBar.transform.localScale = new Vector3(async.progress, 1f, 1f);

    // // Check if the load has finished
    // if (async.progress >= 0.9f) {
    //     // Change the Text to show the Scene is ready
    //     loadingBar.transform.localScale = new Vector3(1f, 1f, 1f);
    //     // Activate the Scene
    //     // async.allowSceneActivation = true;
    // }
  }
}