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
}
