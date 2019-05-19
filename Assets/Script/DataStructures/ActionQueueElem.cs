using UnityEngine;
using UnityEngine.UI;

class ActionQueueElem : MonoBehaviour, PoolableObject {
  public Text text;
  public Button button;

  public void OnPoolInitialize() {}
  public void OnPoolRelease() {}
}
