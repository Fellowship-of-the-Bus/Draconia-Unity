using UnityEngine;

public abstract class Effects : MonoBehaviour {
  public abstract void onApply(Character c);
  public abstract void onRemove(Character c);
  public abstract void activate(Character c);
}