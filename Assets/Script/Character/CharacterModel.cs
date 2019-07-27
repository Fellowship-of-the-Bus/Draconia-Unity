using UnityEngine;

public enum CharacterType {
  Human,
  Lizard,
  Snake
}

public class CharacterModel : MonoBehaviour {
  public GameObject model;
  public Animator animator;
  public Transform leftHand;
  public Transform rightHand;
  public CharacterType characterType;
  public Transform portraitCameraPosition;

  public GameObject weapon;
  private bool bowstringAttached;
  private Transform bowstring;
  private Transform grip;

  public void attachBowstring() {
    if (bowstring == null) bowstring = weapon.transform.findRecursive("String");
    if (grip == null) grip = weapon.transform.findRecursive("Grip");
    bowstring.SetParent(leftHand, false);
    bowstringAttached = true;
  }

  public void detachBowstring() {
    if (!bowstringAttached) return;
    bowstring.SetParent(grip, false);
  }
}
