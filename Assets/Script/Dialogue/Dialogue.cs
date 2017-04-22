using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class Dialogue : MonoBehaviour {
  public List<DialogueFragment> dialogues;
  public Text textBox;

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
  public void goToNextFrame() {
    if (currentFragment >= dialogues.Count) {
      return;
    }
    if (dialogues[currentFragment].hasNextFrame) {
      string text = dialogues[currentFragment].getNextFrameText();
      textBox.text = text;
    } else {
      currentFragment += 1;
      goToNextFrame();
    }
  }

  public void skipDialogue() {
    //reset dialogue state?
    gameObject.SetActive(false);
  }

  public void loadDialogue(List<DialogueFragment> d) {
    dialogues = d;
    goToNextFrame();
  }

}
