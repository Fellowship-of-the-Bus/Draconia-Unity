using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class TurnDialogue : BFTurnEvent{

  List<DialogueFragment> d;
  public TurnDialogue(BattleCharacter c, int t, List<DialogueFragment> dialogues) : base(c,t) {
    d = dialogues;
  }

  public override void trigger() {
    GameManager.get.dialogue.loadDialogue(d);
  }
}
