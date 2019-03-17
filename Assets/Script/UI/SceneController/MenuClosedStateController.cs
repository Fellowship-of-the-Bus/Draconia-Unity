using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attached to the Closed state on The Menu Animator, to ensure that closing animation fully
// plays before the menu is set inactive

public class MenuClosedStateController : StateMachineBehaviour {
  override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    animator.gameObject.SetActive(false);
  }

  override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    animator.gameObject.SetActive(true);
  }
}
