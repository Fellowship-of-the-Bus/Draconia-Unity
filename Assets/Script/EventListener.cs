using UnityEngine;

public abstract class EventListener : MonoBehaviour {

  public void attachListener(EventHook hook) {
    GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().eventManager.addListener(this, hook);
  }

  public void detachListener() {
    GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().eventManager.removeListener(this); 
  }
  
  public abstract void onEvent(EventHook hook);
}