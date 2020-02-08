using UnityEngine;
using UnityEngine.UI;

public class Tipbox : MonoBehaviour {
  public static Tipbox get;

  public Text textElement;  
  public RectTransform rectTrans;

  void Awake() {
    if (!Singleton.makeSingleton(ref get, this)) return;
  }

  public void setPosition() {
    Vector3[] corners = new Vector3[4];
    rectTrans.GetWorldCorners(corners);
    float width = corners[2].x - corners[0].x;
    float height = corners[1].y - corners[0].y;

    rectTrans.position = tipPosition(Input.mousePosition, width, height);
    if (rectTrans.TransformPoint(rectTrans.position).x - width  < 0) {
      rectTrans.Translate(new Vector3(width,0,0));
    }
    if (rectTrans.position.y + height/2 > Screen.height) {
      rectTrans.Translate(new Vector3(0,-height,0));
    }
  }

  private Vector3 tipPosition(Vector3 mouseRelative, float width, float height) {
    return new Vector3(mouseRelative.x - width / 2, mouseRelative.y + height / 2, 0);
  }

  public void hide() {
    gameObject.SetActive(false);
  }
}
