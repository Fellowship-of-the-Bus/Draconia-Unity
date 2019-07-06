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
    Vector3 scale = expBar.transform.localScale;
    scale.x = Math.Max(Math.Min((float)newChar.curExp/newChar.nextLevelExp,1),0);
    expBar.transform.localScale = scale;
	}
}
