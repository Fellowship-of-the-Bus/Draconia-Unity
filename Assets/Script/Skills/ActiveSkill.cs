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

  //experience gained when used
  public int expGainUse = 10;

  private bool[] usableWeapon = new bool[3] { true, true, true };
  private bool unarmed = true;

  //[0] = targets allies, [0] = targets enemies
  protected bool[] targets = new bool[2] { true, true };
  //number of turns before usable
  public int curCooldown = 0;
  public bool targetsTiles = false;

  public DamageType dType = DamageType.physical;
  public DamageElement dEle = DamageElement.none;

  public ProjectileType projectileType = ProjectileType.None;

  // Tooltip variables
  protected string tooltipRange { get {
    string displayRange = range == 0 ? "Self" : range.ToString();
    return "Range: " + displayRange + "\n";
  }}
  protected string tooltipDamageColor { get {
    if (dEle ==  DamageElement.ice) {
      return "cyan";
    }
    if (dEle ==  DamageElement.fire) {
      return "#ff7000ff";
    }
    return "red";
  }}
  protected string tooltipDamage { get { return "<color=" + tooltipDamageColor + ">" + damageFormula().ToString() + "</color>"; }}
  protected string tooltipHealing { get {
    HealingSkill heal = this as HealingSkill;
    return "<color=lime>" + heal.healingFormula().ToString() + "</color>";
  }}
  public virtual string tooltipHeader { get { return "<b>" + name + "</b>\n" + tooltipRange; }}
  protected virtual string tooltipDescription { get { return "Skill Missing Tooltip!"; }}
  public virtual string tooltip { get { return tooltipHeader + tooltipDescription; }}



  public bool canTargetSelf = false;

  bool listenOnEndturn = false;
  public int ntargets { get; set; }

  public virtual string animation { get { return "Attack"; }}

  public ActiveSkill() {
    ntargets = 1;
  }

  public virtual void activate(BattleCharacter target) {
    if (!canTarget(target.curTile)) return;
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
  public virtual int calculateDamage(BattleCharacter target, Tile attackOrigin = null) {
    if (attackOrigin == null) {
      attackOrigin = self.curTile;
    }

    float heightDifference = attackOrigin.getHeight() - target.curTile.getHeight();
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
    if (self.weapon != null){
      return curCooldown == 0 && usableWeapon[(int)self.weapon.kind];
    } else {
      return curCooldown == 0 && unarmed;
    }
  }

  protected void requireMelee() {
    usableWeapon[(int)Weapon.Kinds.Ranged] = false;
  }

  protected void requireWeapon(Weapon.Kinds k) {
    for (int i = 0; i < usableWeapon.Length; i++) {
      usableWeapon[i] = i == (int)k;
    }
  }

  protected void targetAlly(bool b) {
    targets[0] = b;
  }
  protected void targetEnemy(bool b) {
    targets[1] = b;
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

  // Get the set of tiles that are allowed to be targeted
  // using the current tile as the origin
  public List<Tile> getTargets() {
    return getTargets(self.curTile);
  }

  // Get the set of tiles that are allowed to be targeted
  public virtual List<Tile> getTargets(Tile posn) {
    return getTargetsInRange(posn);
  }

  // Get the set of tiles that are within targetting range
  protected List<Tile> getTargetsInRange(Tile posn) {
    return getTargetsInAoe(posn, range, true);
  }

  // Overload of getTargetsInAoe
  // Allowing position instead of tile as argument
  public List<Tile> getTargetsInAoe(Vector3 position, int aoe, bool heightAdvantage = false) {
    Map map = GameManager.get.map;
    return getTargetsInAoe(map.getTile(position), aoe, heightAdvantage);
  }

  // Get the tiles that will be affected by an aoe skill targeting position
  protected List<Tile> getTargetsInAoe(Tile position, int aoe, bool heightAdvantage = false) {
    Map map = GameManager.get.map;
    List<Tile> targets = map.getTilesWithinRange(position, aoe, heightAdvantage);
    targets.Add(position);
    targets = new List<Tile>(targets.Filter((x) => canTarget(x)));
    return targets;
  }

  // ensure that targets are valid
  public virtual void validate(List<Tile> targets) {}

  // public abstract virtual SkillData calculateSkill();

  public bool canTarget(Tile other) {
    return !other.occupied() || ((other.occupant.team == self.team && targets[0]) || (other.occupant.team != self.team && targets[1]));
  }

}
