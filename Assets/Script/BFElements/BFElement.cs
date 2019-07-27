using UnityEngine;
using System.Collections.Generic;
using System;

public class BFElement : MonoBehaviour{
  BFElementListener listener = new BFElementListener();
  void Start() {
    listener.owner = this;
    listener.attachListener(GameManager.get.eventManager, EventHook.preMove);
    listener.attachListener(GameManager.get.eventManager, EventHook.postMove);
  }
  public class BFElementListener: EventListener {
    public BFElement owner;
    public override void onEvent(Draconia.Event e) {
      if (e.hook == EventHook.preMove) {
        owner.onPreMove(e.sender);
      } else if (e.hook == EventHook.postMove) {
        owner.onPostMove(e.sender);
      }
    }
  }
  protected virtual void onPreMove(BattleCharacter c) {

  }
  protected virtual void onPostMove(BattleCharacter c) {

  }
}
