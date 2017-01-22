using UnityEngine;

public abstract class DurationEffect : Effect {
  public int duration = -1;
  bool listenOnEndturn = false;

  //when this is removed from owner
  public override void onRemove() {
    onDeactivate();
    if (owner) base.detachListener(owner);
    if (ownerTile) base.detachListener(ownerTile);
  }

  public override void onApply(Effected c) {
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
    //sender == null means sent by global game manager
    if (e.hook == EventHook.endTurn && e.sender != null) {
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
    //ensure we have enturn hook on our "parent"(the tile or character this is attached to)
    EventManager c = null;
    if (owner) c = owner;
    if (ownerTile) c = ownerTile;
    if (c != null) {
      base.attachListener(c, EventHook.endTurn);
    }
  }
}
