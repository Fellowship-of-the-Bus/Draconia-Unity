using UnityEngine;
using System.Collections.Generic;

public enum DamageType {
  physical,
  magical,
  none
}

public enum DamageElement {
  ice,
  fire,
  lightning,
  none
}

public abstract class ActiveSkill : EventListener, Skill {
  public const int InfiniteCooldown = -2;

  public int level {get; set;}
  public virtual Character self {get; set;}
  public int range {get; set;}
  public bool useWepRange {get; set;}
  public bool useLos {get; set;}
  public string name {get; set;}
  public int maxCooldown {get; set;}
  private bool[] usableWeapon = new bool[3] { true, true, true };
  //number of turns before usable
  public int curCooldown = 0;
  public bool targetsTiles = false;
  public virtual string tooltip { get { return "Skill Missing Tooltip!"; }}

  public DamageType dType = DamageType.physical;
  public DamageElement dEle = DamageElement.none;

  bool listenOnEndturn = false;
  public int ntargets { get; set; }

  public ActiveSkill() {
    ntargets = 1;
  }

  public virtual void activate(Character target) {
    if (this is HealingSkill) {
      if (calculateHealing(target) != 0) {
        target.takeHealing(calculateHealing(target));
      }
    } else {
      if (calculateDamage(target) != 0) {
        target.takeDamage(calculateDamage(target));
      }
    }
    additionalEffects(target);
  }

  public virtual void activate(Tile target) {
    tileEffects(target);
  }

  public virtual int damageFormula() { return 0; }
  public virtual int calculateDamage(Character target) {
    return target.calculateDamage(damageFormula(), dType, dEle);
  }

  public virtual int calculateHealing(Character target){
    Debug.AssertFormat(this is HealingSkill, "calculateHealing called in a non-Healingskill {0}", this);
    HealingSkill heal = this as HealingSkill;
    return target.calculateHealing(heal.healingFormula());
  }
  public virtual void additionalEffects(Character target) { }
  public virtual void tileEffects(Tile target) { }

  public abstract List<GameObject> getTargets();

  public virtual bool canUse() {
    return curCooldown == 0 && usableWeapon[(int)self.weapon.kind];
  }

  protected void requireMelee() {
    usableWeapon[(int)Weapon.kinds.Ranged] = false;
  }

  protected void requireWeapon(Weapon.kinds k) {
    for (int i = 0; i < usableWeapon.Length; i++) {
      usableWeapon[i] = i == (int)k;
    }
  }

  bool attachedListener = false;
  public virtual void setCooldown() {
    if (!attachedListener) {
      base.attachListener(self, EventHook.endTurn);
      attachedListener = true;
    }
    curCooldown = maxCooldown + 1;
  }

  public override sealed void onEvent(Event e) {
    if (curCooldown != 0 && e.hook == EventHook.endTurn) {
      curCooldown -= 1;
      if (listenOnEndturn) {
        trigger(e);
      }
    }
    if (e.hook != EventHook.endTurn) {
      trigger(e);
    }
  }

  protected virtual void trigger(Event e) {}

  public sealed override void attachListener(EventManager e, EventHook hook) {
    if (hook == EventHook.endTurn) {
      listenOnEndturn = true;
    }
    base.attachListener(e, hook);
  }

  protected List<GameObject> tileTargetting() {
    GameManager gm = GameManager.get;
    List<Tile> tiles = gm.getTilesWithinRange(self.curTile, range);
    List<GameObject> targets = new List<GameObject>();
    foreach (Tile t in tiles) {
      targets.Add(t.gameObject);
    }
    targets.Add(self.curTile.gameObject);
    return targets;
  }

  protected List<GameObject> getTargetsInAoe(Vector3 position, int aoe) {
    GameManager gm = GameManager.get;
    List<Tile> tiles = gm.getTilesWithinRange(gm.getTile(position), aoe);
    List<GameObject> targets = new List<GameObject>();
    foreach (Tile t in tiles) {
        targets.Add(t.gameObject);
    }
    targets.Add(gm.getTile(position).gameObject);
    return targets;
  }
}
