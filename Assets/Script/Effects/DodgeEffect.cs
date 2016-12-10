using UnityEngine;

public class DodgeEffect : Effect {
  public override void onActivate() {
    attachListener(owner, EventHook.preDamage);
  }
  public override void onDeactivate() {
    detachListener(owner);
  }

  public override void additionalEffect(Event e) {
    float chance = Random.value;
    if (chance < 0.1*level) {
      e.finishAttack = false;
//      Debug.Log("dodged: " + e.finishAttack);
    } else {
//      Debug.Log("did not dodge: " + e.finishAttack);
    }

  }
}
