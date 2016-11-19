using UnityEngine;

public abstract class EventListener {

  public void attachListener(EventHook hook) {
    GameManager.get().eventManager.addListener(this, hook);
  }

  public void detachListener() {
    GameManager.get().eventManager.removeListener(this);
  }

  public abstract void onEvent(Event e);
}
