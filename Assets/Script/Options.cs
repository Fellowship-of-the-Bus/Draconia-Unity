using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Options {
  [System.Serializable]
  public abstract class OpField {
    public GameObject panel;
    public Text label;
    protected string name;
    public OpField(string name) {
      this.name = name;
    }
    public abstract void init(GameObject o);
  }
  [System.Serializable]
  public class OpToggleField : OpField {
    public bool val;
    public OpToggleField(bool val, string name) : base(name) {
      this.val = val;
      panel = (GameObject)Resources.Load("UI/Options/OpToggle");
    }
    public override void init(GameObject o) {
      label = o.transform.Find("Label").gameObject.GetComponent<Text>();
      label.text = name;
      o.GetComponent<Toggle>().isOn = val;
    }
  }
  [System.Serializable]
  public class OpSliderField : OpField {
    public float val;
    public int min;
    public int max;
    public bool wholeValue;
    public Text slideVal;
    public Slider slider;
    public OpSliderField(float val, int min, int max, string name, bool wholeValue = true) : base(name) {
      this.val = val;
      this.min = min;
      this.max = max;
      this.wholeValue = wholeValue;
      panel = (GameObject)Resources.Load("UI/Options/OpSlider");
    }
    public override void init(GameObject o) {
      label = o.transform.Find("Label").gameObject.GetComponent<Text>();
      slideVal = o.transform.Find("Value").gameObject.GetComponent<Text>();
      label.text = name;
      slider = o.transform.Find("Slider").gameObject.GetComponent<Slider>();
      slider.maxValue = max;
      slider.minValue = min;
      slider.wholeNumbers = wholeValue;
      slider.value = val;
      slideVal.text = val.ToString();
    }
  }

  //To add a user visible field, add a static field to optionType
  //add default value to displayedOptions
  //add property for it
  //note 'enum' value order must be same as order in array

  //To add a user invisible field (for internal use only)
  //make a static variable
  [System.Serializable]
  public class OptionType {
    readonly int type;
    public readonly static OptionType disAni = new OptionType(0);
    public readonly static OptionType gridTransparency = new OptionType(1);

    public OptionType(int i) {
      type = i;
    }

    public static implicit operator int(OptionType x) {
      return x.type;
    }
  }



  public static OpField[] displayedOptions = new OpField[] {//Display Animation should default to true in real game
    new OpToggleField(true, "Display Animations"),
    new OpSliderField(1f,0,1, "Grid Transparency",false)
  };
  //Visible fields
  public static bool displayAnimation {
    get { return ((OpToggleField)displayedOptions[OptionType.disAni]).val;}
  }
  public static float gridTransparency {
    get { return ((OpSliderField)displayedOptions[OptionType.gridTransparency]).val;}
  }

  public class TextOptions {
    public Font font;
    public delegate void FontChangedEvent(TMP_FontAsset newFont);
    public delegate void FontSizeChangedEvent(int newSize);
    public event FontChangedEvent onFontChange;
    public event FontSizeChangedEvent onSizeChange;
  }
  public static TextOptions Text = new TextOptions();

  //internal use fields
  public static int FPS = 60;
  public static bool debugMode = true;
}
