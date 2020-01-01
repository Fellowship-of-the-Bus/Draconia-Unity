using UnityEngine;
using System.Collections;

public class AnimationEventListener : MonoBehaviour {

  public BattleCharacter battleCharacter;

  public void continueSkill(AnimationEvent e) {
    battleCharacter.doFinishSkill();
  }

  public void attachBowstring(AnimationEvent e) {
    battleCharacter.attachBowstring();
  }

  public void attachArrow(AnimationEvent e) {
    battleCharacter.attachArrow();
  }
}
