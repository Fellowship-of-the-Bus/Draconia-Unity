using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
  protected float hoverTime = 0f;
  protected bool mouseOver = false;
  protected bool tooltipShown = false;
  public string tiptext = "Missing tooltip!";
  public GameObject tipbox;
  protected float hoverThreshold = 0.25f;
  protected float width {
    get {
      return rectTrans.rect.width;
    }
  }
  protected float height {
    get {
      return rectTrans.rect.height;
    }
  }
  protected RectTransform rectTrans;

  void Start() {
    if (tipbox == null) {
      tipbox = GameManager.get.tooltip;
    }
    rectTrans = tipbox.GetComponent<RectTransform>();
  }

  void Update() {
    if (mouseOver && hoverTime <= hoverThreshold) {
      hoverTime += Time.deltaTime;
      if (hoverTime >= hoverThreshold) {
        tooltipShown = true;
        if (showTip()) {
          tipbox.SetActive(showTip());
          setTipbox();
        }
      }
    }

    if (tooltipShown) {
      tipbox.transform.position = tipPosition(Input.mousePosition);
      if (tipbox.transform.TransformPoint(tipbox.transform.position).x - width  < 0) {
        tipbox.transform.Translate(new Vector3(width,0,0));
      }
      if (tipbox.transform.position.y + height/2 > Screen.height) {
        tipbox.transform.Translate(new Vector3(0,-height,0));
      }
    }
  }

  protected virtual bool showTip() {
    return true;
  }

  public virtual void OnPointerEnter(PointerEventData eventData) {
    mouseOver = true;
  }

  public virtual void OnPointerExit(PointerEventData eventData) {
    tooltipShown = false;
    mouseOver = false;
    hoverTime = 0f;
    tipbox.SetActive(false);
  }

  public Vector3 tipPosition(Vector3 mousePosition) {
    Rect tipDimensions = tipbox.GetComponent<RectTransform>().rect;
    Vector3 mouseRelative = mousePosition;

    return new Vector3(mouseRelative.x - tipDimensions.width / 2, mouseRelative.y + tipDimensions.height / 2, 0);
  }

  protected virtual void setTipbox() {
    tipbox.GetComponentsInChildren<Text>()[0].text = tiptext;
  }
}
