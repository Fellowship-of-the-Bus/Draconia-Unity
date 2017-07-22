using UnityEngine;
using UnityEngine.UI;
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
      panel = (GameObject)Resources.Load("OpToggle");
    }
    public override void init(GameObject o) {
      label = o.transform.Find("Label").gameObject.GetComponent<Text>();
      label.text = name;
      o.GetComponent<Toggle>().isOn = val;

    }
  }
  [System.Serializable]
  public class OpSlideField : OpField {
    public int val;
    public int min;
    public int max;
    public Text slideVal;
    public Slider slider;
    public OpSlideField(int val, int min, int max, string name) : base(name) {
      this.val = val;
      this.min = min;
      this.max = max;
      panel = (GameObject)Resources.Load("OpSlider");
    }
    public override void init(GameObject o) {
      label = o.transform.Find("Label").gameObject.GetComponent<Text>();
      slideVal = o.transform.Find("Value").gameObject.GetComponent<Text>();
      label.text = name;
      slider = o.transform.Find("Slider").gameObject.GetComponent<Slider>();
      slider.maxValue = max;
      slider.minValue = min;
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
    public readonly static OptionType test1 = new OptionType(1);
    public readonly static OptionType test2 = new OptionType(2);
    public readonly static OptionType test3 = new OptionType(3);
    public readonly static OptionType test4 = new OptionType(4);
    public readonly static OptionType test5 = new OptionType(5);

    public OptionType(int i) {
      type = i;
    }

    public static implicit operator int(OptionType x) {
      return x.type;
    }
  }



  public static OpField[] displayedOptions = new OpField[] {
                                                    new OpToggleField(true, "Display Animations"),
                                                    new OpToggleField(true, "Display Animations1"),
                                                    new OpToggleField(true, "Display Animations2"),
                                                    new OpSlideField(10, 5,15,"Display Animations3"),
                                                    new OpSlideField(5,5,15, "Display Animations4"),
                                                    new OpSlideField(60,30,75, "Display Animations5")};
  //Visible fields
  public static bool displayAnimation {
    get { return ((OpToggleField)displayedOptions[OptionType.disAni]).val;}
  }

  //internal use fields
  public static int FPS = 60;
  public static bool debugMode = true;
}
