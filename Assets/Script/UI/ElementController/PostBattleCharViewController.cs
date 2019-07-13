using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Controls the component that is displayed for each character on the victory screen
public class PostBattleCharViewController: MonoBehaviour {
  public Text nameDisplay;
  public Image portrait;
  public GameObject expBar;

  private Character subject;

  public void setCharacter(Character newChar) {
    subject = newChar;
    nameDisplay.text = newChar.name;
    // TODO: Set the portrait

    // Set exp bar
    // TODO: Start at prebattle exp and animate to post battle exp value
    int curLevelExp = newChar.expAtLevel(newChar.curLevel);
    int nextLevelExp = newChar.expAtLevel(newChar.curLevel + 1);

    Vector3 scale = expBar.transform.localScale;
    scale.x = newChar.percentageToNextLevel();
    expBar.transform.localScale = scale;
  }
}
