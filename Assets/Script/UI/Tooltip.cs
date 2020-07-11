using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
  private float hoverTime = 0f;
  private bool mouseOver = false;
  [TextArea]
  public string tiptext = "Missing tooltip!";
  private float hoverThreshold = 0.25f;

  void Update() {
    if (mouseOver && hoverTime <= hoverThreshold) {
      hoverTime += Time.deltaTime;
      if (hoverTime >= hoverThreshold) {
        show();
      }
    }
  }

  public void show() {
    if (showTip()) {
      Tipbox.get.gameObject.SetActive(true);
      setTipbox();
    }
  }

  public void hide() {
    Tipbox.get.gameObject.SetActive(false);
  }

  protected virtual bool showTip() {
    return true;
  }

  public virtual void OnPointerEnter(PointerEventData eventData) {
    mouseOver = true;
  }

  public virtual void OnPointerExit(PointerEventData eventData) {
    hide();
    mouseOver = false;
    hoverTime = 0f;
  }

  protected virtual void setTipbox() {
    Tipbox.get.textElement.text = tiptext;
  }
}
