using UnityEngine;
using UnityEngine.UI;

public class CustomText : MonoBehaviour {
  public Text text;
  public bool updateFont;
  public bool updateSize;

  void OnEnable() {
    if (updateFont) Options.Text.onFontChange += onFontChange;
    if (updateSize) Options.Text.onSizeChange -= onSizeChange;
  }

  void OnDisable() {
    if (updateFont) Options.Text.onFontChange -= onFontChange;
    if (updateSize) Options.Text.onSizeChange -= onSizeChange;
  }

  void onFontChange(Font newFont) {
    text.font = newFont;
  }

  void onSizeChange(int newSize) {
    text.fontSize = newSize;
  }
}
