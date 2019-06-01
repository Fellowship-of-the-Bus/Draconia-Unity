using UnityEngine;
using UnityEngine.UI;

public class ActionQueueElem : MonoBehaviour, PoolableObject {
  public Text text;
  public Button button;

  public void OnPoolInitialize() {}
  public void OnPoolRelease() {}
}
