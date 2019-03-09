using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomText : MonoBehaviour {
  public TextMeshProUGUI theText;
  public bool updateFont;
  public bool updateSize;

  void OnEnable() {
    if (updateFont) Options.Text.onFontChange += onFontChange;
    if (updateSize) Options.Text.onSizeChange += onSizeChange;
  }

  void OnDisable() {
    if (updateFont) Options.Text.onFontChange -= onFontChange;
    if (updateSize) Options.Text.onSizeChange -= onSizeChange;
  }

  void onFontChange(TMP_FontAsset newFont) {
    font = newFont;
  }

  void onSizeChange(int newSize) {
    fontSize = newSize;
  }

  // provide some accessors so we don't have direct references to TextMeshPro throughout the code

  public string text {
    get { return theText.text; }
    set { theText.text = value; }
  }

  public Color color {
    get { return theText.color; }
    set { theText.color = value; }
  }

  public TMP_FontAsset font {
    get { return theText.font; }
    set { theText.font = value; }
  }

  public float fontSize {
    get { return theText.fontSize; }
    set { theText.fontSize = value; }
  }
}
