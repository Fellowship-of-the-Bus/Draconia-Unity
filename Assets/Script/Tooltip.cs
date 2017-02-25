using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
  float hoverTime = 0f;
  bool mouseOver = false;
  bool tooltipShown = false;
  public string tiptext = "Missing tooltip!";
  GameObject tipbox;

  void Update () {
    if (mouseOver && hoverTime < 1f) {
      hoverTime += Time.deltaTime;
      if (hoverTime >= 1f) {
        tooltipShown = true;
        tipbox = GameObject.Instantiate(Resources.Load("Tooltip"), Vector3.zero, Quaternion.identity) as GameObject;
        tipbox.transform.SetParent(gameObject.transform, false);
        tipbox.GetComponentsInChildren<Text>()[0].text = tiptext;
      }
    } else if (tooltipShown) {
      tipbox.transform.localPosition = tipPosition(Input.mousePosition);
      tipbox.GetComponent<CanvasGroup>().alpha = 1;
    }
  }

  public void OnPointerEnter(PointerEventData eventData) {
    mouseOver = true;
  }

  public void OnPointerExit(PointerEventData eventData) {
    GameObject.Destroy(tipbox);
    tooltipShown = false;
    mouseOver = false;
    hoverTime = 0f;
  }

  Vector3 tipPosition(Vector3 mousePosition) {
    Rect tipDimensions = tipbox.GetComponent<RectTransform>().rect;
    Vector3 mouseRelative = mousePosition - gameObject.transform.position;

    return new Vector3(mouseRelative.x - tipDimensions.width / 2, mouseRelative.y + tipDimensions.height / 2, 0);
  }
}