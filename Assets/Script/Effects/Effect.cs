using UnityEngine;

public abstract class Effect : EventListener {
  public Character owner = null;
  public Tile ownerTile = null;
  public int level = 0;

  public bool stackable = false;

  public virtual void apply(Effected e) {
    if (e is Character) apply(e as Character);
    else apply(e as Tile);
  }
  public void apply(Character c) {
    Debug.AssertFormat(level != 0, "Level was not set in effect: {0}", this);
    owner = c;
    onApply(c);
  }

  public void apply(Tile t) {
    Debug.AssertFormat(level != 0, "Level was not set in effect: {0}", this);
    ownerTile = t;
    onApply(t);
  }
  protected virtual void onApply(Character c) {}
  protected virtual void onApply(Tile t) {}
  //when this is removed from owner

  protected virtual void onRemove() {}
  public virtual void remove() {
    onRemove();
  }
  //when this takes effect
  protected virtual void onActivate() {}
  //when this loses effect (due to shadowed by higher level skill)
  protected virtual void onDeactivate() {}

  public virtual void activate() {
    onActivate();
    if (owner) owner.onEvent(new Event(owner, EventHook.activateEffect));
    else ownerTile.onEvent(new Event(ownerTile, EventHook.activateEffect));
  }

  public virtual void deactivate() {
    onDeactivate();
    if (owner) owner.onEvent(new Event(owner, EventHook.deactivateEffect));
    else ownerTile.onEvent(new Event(ownerTile, EventHook.deactivateEffect));
  }

  public override void onEvent(Event e) {
    additionalEffect(e);
  }

  protected virtual void additionalEffect(Event e) {}

  protected virtual bool isGreaterThan(Effect other) {
    return this.level > other.level;
  }

  public static bool operator >(Effect e1, Effect e2) {
    return e1.isGreaterThan(e2);
  }

  public static bool operator <(Effect e1, Effect e2) {
    return e2.isGreaterThan(e1);
  }

  public Effect clone() {
    return MemberwiseClone() as Effect;
  }

}
