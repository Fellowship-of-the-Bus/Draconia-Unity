using UnityEngine;

public abstract class DurationEffect : Effect {
  public int duration = -1;
  bool listenOnEndturn = false;

  //when this is removed from owner
  public override void onRemove() {
    onDeactivate();
    base.detachListener(owner);
  }

  public override void onApply(Character c) {
    base.onApply(c);
    base.attachListener(c, EventHook.endTurn);
  }

  //when this loses effect (due to shadowed by higher level skill)
  public override void onDeactivate() {
    onDeactivateEffects();
    onDeactivateListeners();
  }

  public sealed override void onEvent(Event e) {
    Debug.Assert(duration != 0);
    if (e.hook == EventHook.endTurn) {
      if (duration != -1) {
        duration--;
      }
      if (listenOnEndturn) {
        additionalEffect(e);
      }
    } else {
      additionalEffect(e);
    }
  }

  //All stat changes when deactivated
  public virtual void onDeactivateEffects() {

  }
  //remove any listeners when deactivated
  public virtual void onDeactivateListeners() {

  }

  public sealed override void attachListener(EventManager e, EventHook hook) {
    if (hook == EventHook.endTurn) {
      listenOnEndturn = true;
    }
    base.attachListener(e, hook);
  }

  public sealed override void detachListener(EventManager e) {
    base.detachListener(e);
    base.attachListener(e, EventHook.endTurn);
  }
}
