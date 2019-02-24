using UnityEngine;

public abstract class EventListener {
  public virtual void attachListener(EventManager manager, EventHook hook) {
    manager.addListener(this, hook);
  }

  public virtual void detachListener(EventManager manager) {
    manager.removeListener(this);
  }

  public abstract void onEvent(Draconia.Event e);
}
