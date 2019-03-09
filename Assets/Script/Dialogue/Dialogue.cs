using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;


//Dialogue, consider disabling player control to more or less disable the map when activated.
public class Dialogue : MonoBehaviour {
  public List<DialogueFragment> dialogues;
  public CustomText textBox;
  public Button nextButton;
  public Action onExit;

  void Awake() {
    gameObject.SetActive(false);
  }

  public void setOnExit(Action f) {
    onExit = f;
  }

  int currentFragment = 0;
  int charactersPerFrame = 1;
  public void goToNextFrame() {
    if (currentFragment >= dialogues.Count) {
      //end of dialogue, consider closing the dialogue instead of doing nothing.
      skipDialogue();
    } else if (dialogues[currentFragment].hasNextFrame) {
      string text = dialogues[currentFragment].getNextFrameText();
      StartCoroutine(displayText(text));
      // //check last frame in last fragment
      // if (!dialogues[currentFragment].hasNextFrame && currentFragment == dialogues.Count - 1 ) {
      //   nextButton.interactable = false;
      // }
    } else {
      currentFragment += 1;
      goToNextFrame();
    }
  }

  public IEnumerator displayText(string text) {
    // no next frame
    if (!dialogues[currentFragment].hasNextFrame && currentFragment == dialogues.Count - 1 ) {
      nextButton.GetComponentInChildren<Text>().text = "End";
      gameObject.transform.Find("skipButton").gameObject.SetActive(false);
    }
    nextButton.interactable = false;
    int curChar = 0;
    while (curChar <= text.Length) {
      textBox.text = text.Substring(0, curChar);
      yield return new WaitForSeconds(1/Options.FPS);
      curChar += charactersPerFrame;
    }
    nextButton.interactable = true;
  }

  public void skipDialogue() {
    //reset dialogue state?
    gameObject.SetActive(false);
    onExit();
  }

  public void loadDialogue(List<DialogueFragment> d) {
    dialogues = d;
    currentFragment = 0;
    gameObject.SetActive(true);
    nextButton.gameObject.SetActive(true);
    nextButton.interactable = true;
    nextButton.GetComponentInChildren<Text>().text = "Next";
    gameObject.transform.Find("skipButton").gameObject.SetActive(true);
    goToNextFrame();
  }

}
