using UnityEngine;
using UnityEngine.UI;

public class ActionBarElem : MonoBehaviour, PoolableObject {
  public CustomText text;
  public Button button;
  public Image image;

  public void OnPoolInitialize() {}
  public void OnPoolRelease() {}
}
