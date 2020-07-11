using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Tipbox : MonoBehaviour {
  public static Tipbox get;

  public Text textElement;
  public RectTransform rectTrans;

  void Awake() {
    if (!Singleton.makeSingleton(ref get, this)) return;
  }

  public void setPosition(Vector2 position) {
    float width = rectTrans.rect.width;
    float height = rectTrans.rect.height;
    rectTrans.position = tipPosition(position, width, height);
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

  private void onPointerMove(InputAction.CallbackContext context) {
    setPosition(context.ReadValue<Vector2>());
  }

  void OnEnable() {
    InputSystem.AddCallback(actions => actions.Default.MousePosition, onPointerMove);
  }

  void OnDisable() {
    InputSystem.RemoveCallback(actions => actions.Default.MousePosition, onPointerMove);
  }

  public void hide() {
    gameObject.SetActive(false);
  }
}
