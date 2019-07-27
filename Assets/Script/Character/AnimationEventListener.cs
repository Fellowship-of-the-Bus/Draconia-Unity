using UnityEngine;
using System.Collections;

public class AnimationEventListener : MonoBehaviour {

  public BattleCharacter battleCharacter;

  public void continueSkill(AnimationEvent e) {
    battleCharacter.doFinishSkill();
  }
}
