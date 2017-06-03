using UnityEngine;
using System.Collections.Generic;
using System;

public class DialogueFragment {
  public BattleCharacter speaker;
  public string text;
  public bool hasNextFrame = true;
  public DialogueFragment(BattleCharacter c, string t) {
    speaker = c;
    text = t;
  }
  int textIndex = 0;
  int textBoxSize = 200;
  public string getNextFrameText() {
    if (text.Length < textIndex + textBoxSize) {
      hasNextFrame = false;
      textIndex = textIndex + textBoxSize;
      return text.Substring(textIndex - textBoxSize);
    } else {
      string tmp = text.Substring(textIndex, textBoxSize);
      int index = tmp.LastIndexOf(" ");
      textIndex = textIndex + index + 1;
      if(textIndex > text.Length) hasNextFrame = false;
      return tmp.Substring(0, index);
    }
  }
  public void resetFrames() {
    textIndex = 0;
    hasNextFrame = true;
  }
}