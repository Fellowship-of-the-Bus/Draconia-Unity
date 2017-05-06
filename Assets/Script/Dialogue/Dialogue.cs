using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

public class Dialogue : MonoBehaviour {
  public List<DialogueFragment> dialogues;
  public Text textBox;
  public Button nextButton;

  void Start() {
    gameObject.SetActive(false);
  }

  public void startDialogue() {
    gameObject.SetActive(true);
    List<DialogueFragment> d = new List<DialogueFragment>();
    d.Add(new DialogueFragment(null, "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."));
    d.Add(new DialogueFragment(null, "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."));

    loadDialogue(d);
  }
  int currentFragment = 0;
  int charactersPerFrame = 1;
  int FPS = 60;
  public void goToNextFrame() {
    if (currentFragment >= dialogues.Count) {
      return;
    }
    if (dialogues[currentFragment].hasNextFrame) {
      string text = dialogues[currentFragment].getNextFrameText();
      StartCoroutine(displayText(text));
    } else {
      currentFragment += 1;
      goToNextFrame();
    }
  }

  public IEnumerator displayText(string text) {
    nextButton.interactable = false;
    int curChar = 0;
    while (curChar < text.Length) {
      textBox.text = text.Substring(0, curChar);
      yield return new WaitForSeconds(1/FPS);
      curChar += charactersPerFrame;
    }
    nextButton.interactable = true;
  }

  public void skipDialogue() {
    //reset dialogue state?
    gameObject.SetActive(false);
  }

  public void loadDialogue(List<DialogueFragment> d) {
    dialogues = d;
    currentFragment = 0;
    goToNextFrame();
  }

}
