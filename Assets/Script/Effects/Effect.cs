using UnityEngine;

public abstract class Effect : EventListener {
  public int id = 0;
  public Character owner = null;
  public int level = 0;
  public void onApply(Character c) {
    owner = c;
  }
  //when this is removed from owner
  public abstract void onRemove();
  //when this takes effect
  public abstract void onActivate();
  //when this loses effect (due to shadowed by higher level skill)
  public abstract void onDeactivate();
  public override void onEvent(Event e) {

  }
}
