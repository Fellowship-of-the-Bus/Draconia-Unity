using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Options {
  [Serializable]
  public struct OptionData {
    public float gridTransparency;
    public bool displayAnimation;
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

  [Serializable]
  public struct TextOptions {
    public Font font;
    public delegate void FontChangedEvent(TMP_FontAsset newFont);
    public delegate void FontSizeChangedEvent(int newSize);
    public event FontChangedEvent onFontChange;
    public event FontSizeChangedEvent onSizeChange;
  }
  public static TextOptions Text = new TextOptions();

  // internal use fields - not serialized
  public static int FPS = 60;
  public static bool debugMode = true;
}
