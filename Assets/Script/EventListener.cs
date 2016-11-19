using UnityEngine;

public abstract class EventListener {

  public void attachListener(EventHook hook) {
    EventManager.get.eventManager.addListener(this, hook);
  }

  public void detachListener() {
    EventManager.get.removeListener(this);
  }

  public abstract void onEvent(Event e);
}
