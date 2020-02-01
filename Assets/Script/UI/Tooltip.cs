using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
  private float hoverTime = 0f;
  private bool mouseOver = false;
  private bool tooltipShown = false;
  [TextArea]
  public string tiptext = "Missing tooltip!";
  private float hoverThreshold = 0.25f;

  void Update() {
    if (mouseOver && hoverTime <= hoverThreshold) {
      hoverTime += Time.deltaTime;
      if (hoverTime >= hoverThreshold) {
        tooltipShown = true;
        if (showTip()) {
          Tipbox.get.gameObject.SetActive(showTip());
          setTipbox();
        }
      }
    }

    if (tooltipShown) {
      Tipbox.get.setPosition();
      setTipbox();
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
    Tipbox.get.gameObject.SetActive(false);
  }

  protected virtual void setTipbox() {
    Tipbox.get.textElement.text = tiptext;
  }
}
