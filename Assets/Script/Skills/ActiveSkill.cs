using UnityEngine;
using System.Collections.Generic;
using System;

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
  public virtual BattleCharacter self {get; set;}
  private int _range;
  public int range {get {if (useWepRange) return self.weapon.range; else return _range;} set {_range = value;}}
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

  public bool canTargetSelf = false;

  bool listenOnEndturn = false;
  public int ntargets { get; set; }

  public ActiveSkill() {
    ntargets = 1;
  }

  public virtual void activate(BattleCharacter target) {
    if (this is HealingSkill) {
      if (calculateHealing(target) != 0) {
        target.takeHealing(calculateHealing(target));
      }
    } else {
      if (calculateDamage(target) != 0) {
        target.takeDamage(calculateDamage(target));
      }
    }
    if (dEle == DamageElement.fire) {
      float chance = UnityEngine.Random.value;
      if (chance < 0.3f) {
        BurnEffect debuff = new BurnEffect();
        debuff.level = level;
        debuff.duration = (int)(2*target.fireResMultiplier);
        debuff.damage = (int)System.Math.Max((int)calculateDamage(target)*0.2f, 1);
        target.applyEffect(debuff);
      }
    }
    if (dEle == DamageElement.ice) {
      float chance = UnityEngine.Random.value;
      if (chance < 0.3f) {
        SlowEffect debuff = new SlowEffect();
        //something that depends on the damage done
        debuff.level = (int)(3*calculateDamage(target)/target.maxHealth);
        debuff.duration = (int)(2*target.iceResMultiplier);
      }
    }
    if (dEle == DamageElement.lightning) {
      //apply .... at % rate
    }
    additionalEffects(target);
  }

  public virtual void activate(Tile target) {
    tileEffects(target);
  }

  public virtual int damageFormula() { return 0; }
  public virtual int calculateDamage(BattleCharacter target) {
    float heightDifference = self.curTile.getHeight() - target.curTile.getHeight();
    float multiplier = 1;
    if (range >= 1) {
      //balance here
      multiplier += heightDifference * 1f;
    }
    multiplier = Math.Max(0, multiplier);
    return (int) (target.calculateDamage(damageFormula(), dType, dEle) * multiplier);
  }

  public virtual int calculateHealing(BattleCharacter target){
    Debug.AssertFormat(this is HealingSkill, "calculateHealing called in a non-Healingskill {0}", this);
    HealingSkill heal = this as HealingSkill;
    return target.calculateHealing(heal.healingFormula());
  }
  public virtual void additionalEffects(BattleCharacter target) { }
  public virtual void tileEffects(Tile target) { }

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

  public abstract List<Tile> getTargets();

  protected List<Tile> getTargetsInRange() {
    return getTargetsInAoe(self.gameObject.transform.position, range);
  }

  protected List<Tile> getTargetsInAoe(Vector3 position, int aoe) {
    Map map = GameManager.get.map;
    List<Tile> targets = map.getTilesWithinRange(map.getTile(position), aoe);
    targets.Add(map.getTile(position));
    return targets;
  }

  // ensure that targets are valid
  public virtual void validate(List<List<Effected>> targets) {}

  // public abstract virtual SkillData calculateSkill();
}
