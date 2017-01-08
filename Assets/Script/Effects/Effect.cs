using UnityEngine;

public abstract class Effect : EventListener {
  public Character owner = null;
  public int level = 0;
  public virtual void onApply(Character c) {
    Debug.AssertFormat(level != 0, "Level was not set in effect: {0}", this);
    owner = c;
    whenApplied(c);
  }
  public virtual void whenApplied(Character c) {}
  //when this is removed from owner
  public virtual void onRemove() {}
  //when this takes effect
  public abstract void onActivate();
  //when this loses effect (due to shadowed by higher level skill)
  public abstract void onDeactivate();

  public override void onEvent(Event e) {
    additionalEffect(e);
  }

  public virtual void additionalEffect(Event e) {}

  public virtual bool isGreaterThan(Effect other) {
    return this.level > other.level;
  }

  public static bool operator >(Effect e1, Effect e2) {
    return e1.isGreaterThan(e2);
  }

  public static bool operator <(Effect e1, Effect e2) {
    return e2.isGreaterThan(e1);
  }

}
