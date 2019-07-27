using UnityEngine;
using UnityEngine.UI;

public class ActionQueueElem : MonoBehaviour, PoolableObject {
  public CustomText text;
  public Button button;
  public Image image;

  public void OnPoolInitialize() {}
  public void OnPoolRelease() {}
}
