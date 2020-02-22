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

public enum StatAlignment {
  strength,
  intelligence,
  none
}

public abstract class ActiveSkill : EventListener, Skill {
  public const int InfiniteCooldown = -2;

  // Skills will always do at least this percentage of their calculated damage
  private const float MINIMUM_DAMAGE_PERCENT = 0.1f;

  public int level {get; set;}
  public virtual BattleCharacter self {get; set;}

  // Character used when outside of map
  public Character character {get; set;}
  protected Attributes attributes {
    get {
      if (self != null) {
        return self.totalAttr;
      } else {
        return character.totalAttr;
      }
    }
  }
  private int _range;
  public int range {get {
    if (useWepRange) return self.weapon.range; else return _range;}
    set {_range = value;}
  }
  public bool useWepRange {get; set;}
  public bool useLos {get; set;} // Use line of sight for targeting
  public string name {get; set;}
  public int maxCooldown {get; set;}

  //experience gained when used
  private float experienceStatMultiplier = 1.2f;
  //for skills not aligned, give experience based on how much you would gain
  //if you killed a target at your current level
  private float notAlignedExpMultiplier = 0.4f;
  private int notAlignedExp() {
    return (int)(self.baseChar.expGivenOnKill*notAlignedExpMultiplier);
  }
  private int alignedExp(int statValue) {
    return (int)(experienceStatMultiplier*statValue);
  }
  protected StatAlignment alignment = StatAlignment.none;
  protected void strAligned() {
    alignment = StatAlignment.strength;
  }
  protected void intAligned() {
    alignment = StatAlignment.intelligence;
  }
  public int expGainUse {
    get {
      switch(alignment) {
        case StatAlignment.strength:
          return alignedExp(self.strength);
        case StatAlignment.intelligence:
          return alignedExp(self.intelligence);
        case StatAlignment.none:
          return notAlignedExp();
        default:
          return 0;
      }
    }
  }

  protected bool[] usableWeapon = new bool[2] { true, true };
  private bool unarmed = true;

  //[0] = targets allies, [0] = targets enemies
  protected bool[] targets = new bool[2] { true, true };
  //number of turns before usable
  public int curCooldown = 0;
  public bool targetsTiles = false;

  public DamageType dType = DamageType.none;
  public DamageElement dEle = DamageElement.none;

  // Projectile Parameters
  public ProjectileType projectileType = ProjectileType.None;
  public ProjectileMovementType projectileMoveType = ProjectileMovementType.Straight;
  public float projectileSpeed = 2f;

  // Tooltip variables
  protected string tooltipRange { get {
    string displayRange;
    if (useWepRange && self == null) {
      displayRange = "Weapon Range";
    } else {
      displayRange = range == 0 ? "Self" : range.ToString();
    }
    return "<b>Range:</b> " + displayRange + "\n";
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
  public string tooltipDamage { get { return "<color=" + tooltipDamageColor + ">" + augmentedFormula().ToString() + "</color>"; }}
  protected string tooltipHealing { get {
    HealingSkill heal = this as HealingSkill;
    return "<color=lime>" + heal.healingFormula().ToString() + "</color>";
  }}
  public virtual string tooltipHeader { get { return "<b>" + name + "</b>\n" + tooltipRange; }}
  public virtual string tooltipDescription { get { return "Skill Missing Tooltip!"; }}
  public virtual string tooltip { get { return tooltipHeader + tooltipDescription; }}
  protected string formatDamage(int value) {
    return "<color=" + tooltipDamageColor + ">" + value.ToString() + "</color>";
  }


  public bool canTargetSelf = false;

  bool listenOnEndturn = false;
  public int ntargets { get; set; }

  public virtual string animation { get { return "Attack"; }}
  public virtual Color castColor { get { return castColorNone; }}
  protected Color castColorBloodPriest = Color.red;
  protected Color castColorCleric = Color.white;
  protected Color castColorEnhancer = Color.green;
  protected Color castColorIceWizard =  new Color(0, 0.5f, 1);
  protected Color castColorPyromancer = new Color(1, 0.4f, 0);
  protected Color castColorWarlock = Color.black;
  protected Color castColorNone = Color.clear;

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
        target.takeDamage(calculateDamage(target),self);
      }
    }
    if (dEle == DamageElement.fire) {
      float chance = UnityEngine.Random.value;
      if (chance < 0.3f) {
        BurnEffect debuff = new BurnEffect();
        debuff.duration = (int)(2*target.fireResMultiplier);
        debuff.effectValue = (int)System.Math.Max((int)calculateDamage(target)*0.2f, 1);
        debuff.caster = self;
        target.applyEffect(debuff);
      }
    }
    if (dEle == DamageElement.ice) {
      float chance = UnityEngine.Random.value;
      if (chance < 0.3f) {
        SlowEffect debuff = new SlowEffect();
        //something that depends on the damage done
        debuff.effectValue = (int)(3*calculateDamage(target)/target.maxHealth);
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
  private int augmentedFormula() {
    float multiplier = 1;
    if (self != null) {
      multiplier += self.baseChar.totalTraits.spec.wepSpec[self.weapon.equipmentClass];
      multiplier += self.baseChar.totalTraits.spec.elementSpec[dEle];
    }
    return (int)(damageFormula()*multiplier);
  }
  private const float heightDifferenceMultiplier = 0.2f;
  public virtual int calculateDamage(BattleCharacter target, Tile attackOrigin = null) {
    if (attackOrigin == null) {
      attackOrigin = self.curTile;
    }

    float heightDifference = attackOrigin.getHeight() - target.curTile.getHeight();
    float multiplier = 1;
    if (range >= 1) {
      //balance here
      multiplier += heightDifference * heightDifferenceMultiplier;
      if (multiplier < 0.5f) {
        multiplier = 0.5f;
      }
    }
    multiplier = Math.Max(0, multiplier);
    float traitMultiplier = 1;
    traitMultiplier += self.baseChar.totalTraits.spec.wepSpec[self.weapon.equipmentClass];
    traitMultiplier += self.baseChar.totalTraits.spec.enemySpec[target.enemyType];
    traitMultiplier += self.baseChar.totalTraits.spec.elementSpec[dEle];
    int calculatedDamage = (int) (target.calculateDamage((int)(damageFormula() * multiplier * traitMultiplier), dType, dEle));
    int minimumDamage = (int)Mathf.Ceil(damageFormula() * MINIMUM_DAMAGE_PERCENT);
    return Math.Max(minimumDamage, calculatedDamage);
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

  // Returns whether a melee weapon is required for the skill
  public bool isMeleeRequired() {
    return !usableWeapon[(int)Weapon.Kind.Ranged];
  }

  protected void requireMelee() {
    usableWeapon[(int)Weapon.Kind.Ranged] = false;
  }

  protected void requireWeapon(Weapon.Kind k) {
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

  public override sealed void onEvent(Draconia.Event e) {
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

  protected virtual void trigger(Draconia.Event e) {}

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
    return getTargetsInAoe(posn, range);
  }

  // Overload of getTargetsInAoe
  // Allowing position instead of tile as argument
  public List<Tile> getTargetsInAoe(Vector3 position, int aoe) {
    Map map = GameManager.get.map;
    return getTargetsInAoe(map.getTile(position), aoe);
  }

  // Get the tiles that will be affected by an aoe skill targeting position
  protected List<Tile> getTargetsInAoe(Tile position, int aoe) {
    Map map = GameManager.get.map;
    List<Tile> targets = map.getTilesWithinDistance(position, aoe, isMeleeRequired());

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

  // Play the audiovisual effects for the skill
  // callback - Function that performs the gameplay elements of the skill
  public virtual void playAVEffects(Action callback, Tile target) {
    if (this.projectileType != ProjectileType.None) {
      new Projectile(self,
        this.targetsTiles ? (target as Effected) : target.occupant,
        this.projectileType,
        this.projectileMoveType,
        this.projectileSpeed,
        callback,
        self.model.projectile
      );
    } else {
      callback();
    }
  }
}
