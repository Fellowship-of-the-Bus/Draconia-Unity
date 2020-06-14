using System;
using System.IO;
using System.Runtime.Serialization;
using UnityEngine;
using TMPro;

public class Options {
  [Serializable]
  public struct OptionData {
    public TextOptions text;
    public float gridTransparency;
    public bool displayAnimation;
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
    public string fontName;
    public float fontSize;
    public delegate void FontChangedEvent(TMP_FontAsset newFont);
    public delegate void FontSizeChangedEvent(float newSize);
    public static FontChangedEvent onFontChange;
    public static FontSizeChangedEvent onSizeChange;
    public TMP_FontAsset font {
      get {
        // TODO: hook in font lookup logic based on fontName and fontSize
        return TMP_Settings.defaultFontAsset;
      }
    }
  }
  public static ref TextOptions Text {
    get { return ref instance.text; }
  }

  private static OptionData init() {
    OptionData data = new OptionData();
    SaveDataOperation operation = SaveLoad.loadOptions(onLoadFailure);
    return data;
  }

  private static bool onLoadFailure(Exception ex) {
    if (ex is SerializationException) {
      Debug.LogWarningFormat("Failed to load options because file is corrupt. Falling back on default values.\n{0}", ex);
      initWithDefault();
      return true;
    } else if (ex is FileNotFoundException) {
      SaveLoad.channel.LogWarning("Failed to load options because file does not exist. Failling back on default values.");
      initWithDefault();
      return true;
    }
    return false;
  }

  private static void initWithDefault() {
    instance = new OptionData();
    instance.gridTransparency = 0.25f;
    instance.displayAnimation = true;
    instance.text = initTextOptions();
    SaveLoad.saveOptions();
  }

  private static TextOptions initTextOptions() {
    TextOptions text = new TextOptions();
    return text;
  }

  // internal use fields - not serialized
  public static int FPS = 60;
  public static bool debugMode = true;
}
