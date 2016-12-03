using UnityEngine;

public abstract class EventListener {

  public void attachListener(EventManager manager, EventHook hook) {
    manager.addListener(this, hook);
  }

  public void detachListener(EventManager manager) {
    manager.removeListener(this);
  }

  public abstract void onEvent(Event e);
}
