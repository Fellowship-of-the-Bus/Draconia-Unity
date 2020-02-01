using UnityEngine;

public abstract class Timer {
  private readonly float fireDelta;
  private float curTime = 0.0F;
  public Timer(float fireDelta) {
    this.fireDelta = fireDelta;
  }

  public void Update() {
    curTime += Time.deltaTime;
    if (curTime > fireDelta) {
      if (Fire()) {
        curTime = 0.0f;
      }
    }
  }

  protected abstract bool Fire();
}