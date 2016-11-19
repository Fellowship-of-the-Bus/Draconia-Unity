using UnityEngine;

public abstract class EventListener {

  public void attachListener(EventHook hook) {
    GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().eventManager.addListener(this, hook);
  }

  public void detachListener() {
    GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().eventManager.removeListener(this);
  }

  public abstract void onEvent(Event e);
}
