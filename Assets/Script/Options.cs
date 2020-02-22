using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Options {
  [Serializable]
  public struct OptionData {
    public float gridTransparency;
    public bool displayAnimation;
    public TextOptions text;
  }

  [Serializable]
  public struct TextOptions {
    public TMP_FontAsset font;
    public int fontSize;

    public delegate void FontChangedEvent(TMP_FontAsset newFont);
    public delegate void FontSizeChangedEvent(int newSize);
    public static FontChangedEvent onFontChange;
    public static FontSizeChangedEvent onSizeChange;
  }


  public static OptionData init() {
    OptionData data = new OptionData();
    data.gridTransparency = 0.25f;
    data.displayAnimation = true;
    return data;
  }
  public static OptionData instance = init();

  public static bool displayAnimation {
    get { return instance.displayAnimation; }
    set { instance.displayAnimation = value; }
  }

  public static float gridTransparency {
    get { return instance.gridTransparency; }
    set {
      instance.gridTransparency = value;
      TileMaterials.updateTransparency();
    }
  }

  public static TMP_FontAsset font {
    get { return instance.text.font; }
    set {
      instance.text.font = value;
      Options.TextOptions.onFontChange?.Invoke(value);
    }
  }

  public static int fontSize {
    get { return instance.text.fontSize; }
    set {
      instance.text.fontSize = value;
      Options.TextOptions.onSizeChange?.Invoke(value);
    }
  }

  // internal use fields - not serialized
  public static int FPS = 60;
  public static bool debugMode = true;
}
