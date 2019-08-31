using UnityEngine;
using System.Collections.Generic;
using System;

public class BFElement : MonoBehaviour{
  BFElementListener listener = new BFElementListener();
  protected void Start() {
    listener.owner = this;
    listener.attachListener(GameManager.get.eventManager, EventHook.preMove);
    listener.attachListener(GameManager.get.eventManager, EventHook.postMove);
    Tile tile = GameManager.get.map.getTile(transform.position);
    BFIdEffect e = new BFIdEffect();
    e.element = this;
    e.stackable = true;
    tile.applyEffect(e);
  }

  public class BFIdEffect : Effect {
    public BFElement element;
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

  public virtual void init() {

  }
  protected virtual void onPreMove(BattleCharacter character) {

  }
  protected virtual void onPostMove(BattleCharacter character) {

  }
}
