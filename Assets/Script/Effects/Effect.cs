using UnityEngine;

public abstract class Effect : EventListener {
  public Character owner = null;
  public int level = 0;
  public void onApply(Character c) {
    Debug.AssertFormat(level != 0, "Level was not set in effect: {0}", this); 
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
