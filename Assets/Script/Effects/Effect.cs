using UnityEngine;

public abstract class Effect : EventListener {
  public int id = 0;
  public Character owner = null;
  public int level = 0;
  public void onApply(Character c) {
    owner = c;
  }
  public abstract void onRemove();
  public abstract void onActivate();
  public abstract void onDeactivate();
  public override void onEvent(MonoBehaviour sender, EventHook hook) {

  }
  public abstract void activate();
}
