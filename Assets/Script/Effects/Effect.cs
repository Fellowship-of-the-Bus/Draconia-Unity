using UnityEngine;

public abstract class Effect : EventListener {
  public Character owner = null;
  public int level = 0;
  public int duration = -1;
  public void onApply(Character c) {
    Debug.AssertFormat(level != 0, "Level was not set in effect: {0}", this);
    owner = c;
  }
  //when this is removed from owner
  public virtual void onRemove() {
  }
  //when this takes effect
  public abstract void onActivate();
  //when this loses effect (due to shadowed by higher level skill)
  public abstract void onDeactivate();
  public sealed override void onEvent(Event e) {
    Debug.Assert(duration != 0);
    if (duration != -1) {
      duration--;
    }
    additionalEffect(e);
  }

  public virtual void additionalEffect(Event e) {

  }

  public virtual bool isGreaterThan(Effect other) {
    return this.level > other.level;
  }

  //public sealed override void detachListener(EventManager e) {
  //  Debug.Log("detach")
  //  Debug.Assert(duration == -1);
  //  base.detachListener(e);
  //}
}
