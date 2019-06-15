using UnityEngine;

public abstract class DurationEffect : Effect {
  public int duration = -1;
  bool listenOnEndturn = false;

  public override string tooltip { get {
    return tooltipHeader + tooltipDescription +
      "\nTurns remaining: " + duration;
  }}

  //when this is removed from owner
  public sealed override void remove() {
    base.remove();
    if (owner) base.detachListener(owner);
    if (ownerTile) base.detachListener(ownerTile);
  }

  public override void apply(Effected c) {
    base.apply(c);
    base.attachListener(c, EventHook.endTurn);
  }

  //when this loses effect (due to shadowed by higher level skill)
  public sealed override void deactivate() {
    onDeactivateEffects();
    onDeactivateListeners();
  }


  public sealed override void onEvent(Draconia.Event e) {
    Debug.Assert(duration != 0);
    //sender == null means sent by global game manager
    if (e.hook == EventHook.endTurn && shouldDecrement(e)) {
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

  public virtual bool shouldDecrement(Draconia.Event e) {
    return e.sender != null;
  }

  //All stat changes when deactivated
  protected virtual void onDeactivateEffects() {

  }
  //remove any listeners when deactivated
  protected virtual void onDeactivateListeners() {

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
    listenOnEndturn = false;
  }
}
