using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;
using System.Collections;

public enum AIType {
  Aggressive,Basic,Buff,Sentry,None
}
public enum EnemyType {
  Human, Lizard, Chameleon, Snake, Dragon, None
}

public class BattleCharacter : Effected {
  public Character baseChar = new Character();
  public AIType aiType = AIType.None;
  public EnemyType enemyType = EnemyType.Human;
  public new string name {
    get { return baseChar.name; }
    set { baseChar.name = value; }
  }

  //inventory
  //skill tree
  public SkillTree skills {
    get { return baseChar.skills; }
  }

  public bool isNinja {
    get { return skills.getSkillLevel(typeof(Dodge)) > 0; }
  }

  //base stats + permanent stat passives
  public Attributes attr {
    get { return baseChar.totalAttr; }
  }
  //change in stats due to buffs/debuffs
  [HideInInspector]
  public Attributes attrChange = new Attributes();

  public Weapon weapon {
    get { return (Weapon)baseChar.gear[EquipType.weapon]; }
  }

  public int curExp {
    get { return baseChar.curExp;}
  }

  public int curLevel {
    get {return baseChar.curLevel;}
  }

  [System.Serializable]
  public struct SkillData {
    public string name;
    public int level;
  }

  // Allow setting skills in editor
  public SkillData[] skillSet;
  public SkillData[] passiveSet;

  public List<ActiveSkill> equippedSkills = new List<ActiveSkill>();

  [HideInInspector]
  public Tile curTile = null;

  // [HideInInspector]
  public int curHealth;
  public const float maxAction = 1000f;
  [HideInInspector]
  public float curAction = 0;

  public enum Team {
    Player, Enemy, Ally
  };
  public Team team = Team.Player;

  private int previewChange;
  public int PreviewChange{
    get { return Mathf.Clamp(previewChange, -curHealth, maxHealth - curHealth); }
    set { previewChange = value; }
  }

  public BaseAI ai = new BasicAI();

  public LinkedList<Effect> allEffects = new LinkedList<Effect>();
  [HideInInspector]
  public List<GameObject> particleEffects = new List<GameObject>();
  private ParticleSystem castingCircle;

  [HideInInspector]
  public bool levitating = false;

  public override void applyEffect(Effect effect) {
    base.applyEffect(effect);
    allEffects.AddLast(effect);
  }

  public override void removeEffect(Effect effect) {
    base.removeEffect(effect);
    allEffects.Remove(effect);
  }

  // Take a particle effects prefab and create an instance on the character
  // Returns the instance of the prefab
  public GameObject applyParticle(GameObject effect) {
    GameObject particleEffect = Instantiate(effect, this.transform);
    particleEffects.Add(particleEffect);
    return particleEffect;
  }

  // Takes an instance of a particle effect which has been applied to the character
  // Removed the given effect
  public void removeParticle(GameObject effect) {
    particleEffects.Remove(effect);
    Destroy(effect);
  }

  List<String> prevSkillSet = new List<String>();

  void Start(){
    init();
  }

  bool initCalled = false;

  public void init(bool inGame = true) {
    if (initCalled) return;
    initCalled = true;
    castingCircle = Instantiate(
      (GameObject)Resources.Load("ParticleEffects/Casting Circle"),
      this.transform
    ).GetComponent<ParticleSystem>();
    equippedSkills = skills.getActives(this);
    if (Options.debugMode && equippedSkills.IsEmpty()) {
      setSkills();
      prevSkillSet = new List<String>(skillSet.Select(x => x.name));
    }
    curHealth = maxHealth;

    ui.healthBars.setCharacter(this);
    ui.name.text = baseChar.name;

    switch (aiType) {
      case AIType.Aggressive:
      case AIType.None:
        ai = new AggressiveAI();
        break;
      case AIType.Basic:
        ai = new BasicAI();
        break;
      case AIType.Buff:
        ai = new BuffAI();
        break;
      case AIType.Sentry:
        ai = new SentryAI();
        break;
    }
    ai.owner = this;

    minimapIconRenderer.material = GameManager.get.minimapIcons[(int)team];

    applyPassives();

    if (inGame) {
      GameObject weaponModel = weapon.getModel();
      if (weaponModel) GameObject.Instantiate(weaponModel, model.rightHand);
    }
    gameObject.name = name;

  }

  void setSkills() {
    int i = 0;
    foreach (SkillData data in skillSet) {
      if (i < prevSkillSet.Count && data.name == prevSkillSet[i]) {
        equippedSkills[i].level = data.level;
        i++;
        continue;
      }
      if (i >= prevSkillSet.Count) prevSkillSet.Add(data.name);
      else prevSkillSet[i] = data.name;
      ActiveSkill skill = null;
      bool invalidSkill = true;

      foreach (Type t in Assembly.GetExecutingAssembly().GetTypes()) {
        if (t.IsSubclassOf(Type.GetType("ActiveSkill")) && t.FullName == data.name) {
          skill = (ActiveSkill)Activator.CreateInstance(t);
        }
        if (equippedSkills.Count > i && t.FullName == data.name && t == equippedSkills[i].GetType()) {
          skill = null;
          invalidSkill = false;
        }
      }
      if (skill == null && invalidSkill) {
        Channel.game.Log("Skill not recognized");
        skill = new Punch();
      }
      if (skill != null) {
        skill.level = data.level;
        skill.self = this;
        if (equippedSkills.Count > i) {
          equippedSkills[i] = skill;
        } else {
          equippedSkills.Add(skill);
        }
      }
      i++;
    }
  }

  public void applyPassives() {
    foreach (SkillData data in passiveSet) {
      PassiveSkill skill = null;
      foreach (Type t in Assembly.GetExecutingAssembly().GetTypes()) {
        if (t.IsSubclassOf(Type.GetType("PassiveSkill")) && t.FullName == data.name) {
          skill = (PassiveSkill)Activator.CreateInstance(t);
          skill.level = data.level;
          skill.self = this;
          skill.activate(this);
        }
      }
    }
    foreach (PassiveSkill passive in skills.getPassives(this)) {
      passive.activate(this);
    }
  }

  void OnValidate() {
    if (Options.debugMode && equippedSkills.Count != 0) {
      setSkills();
    }
  }

  public CharacterModel model;
  public BattleCharacterUI ui;
  public MeshRenderer minimapIconRenderer;

  public float calcMoveTime(float time, int turns = 1) {
    return time + ((maxAction - curAction) / speed) + ((turns - 1) * (maxAction / speed));
  }

  public bool inRange(BattleCharacter target, int range) {
    return GameManager.get.map.getTilesWithinRange(curTile, range, false).Contains(target.curTile);
  }

  public bool useSkill(ActiveSkill skill, List<Tile> tileTargets) {
    List<Tile> validTargets = skill.getTargets();
    List<Effected> targets = new List<Effected>();
    Tile target = tileTargets.First();

    if (skill is Portal) {
      foreach (Tile t in tileTargets) {
        targets.Add(t);
      }
    } else {
      // Check if the usage is valid
      if (!validTargets.Contains(target)
        || tileTargets.Count() != skill.ntargets) {
        return false;
      }

      AoeSkill aoe = skill as AoeSkill;
      if (aoe != null) {
        foreach (Tile t in aoe.getTargetsInAoe(target.transform.position)) {
          BattleCharacter c = t.occupant;
          if (c) targets.Add(c);
          if (aoe.effectsTiles) targets.Add(t);
        }
      } else {
        targets.Add(target.occupant);
      }
    }

    face(target.transform.position);


    if (model.animator) {
      if (skill.castColor != Color.clear) {
        var castMain = castingCircle.main;
        castMain.startColor = skill.castColor;
        castingCircle.Play();
      }

      GameManager.get.waitFor(model.animator, skill.animation,
        () => {
          castingCircle.Stop();
          skill.playAVEffects(() => {
            finishSkill(skill, target, targets);
          }, target);
        }
      );
    }
    else finishSkill(skill, target, targets);
    return true;
  }

  void finishSkill(ActiveSkill skill, Tile target, List<Effected> targets) {
    int expGained = getExpGained(skill);
    List<BattleCharacter> cTargets = new List<BattleCharacter>();
    List<Tile> tTargets = new List<Tile>();
    foreach(Effected e in targets) {
      if (e is BattleCharacter) cTargets.Add(e as BattleCharacter);
      else tTargets.Add(e as Tile);
    }
    skill.setCooldown();
    Draconia.Event preSkillEvent = new Draconia.Event(this, EventHook.preSkill);
    preSkillEvent.skillUsed = skill;
    onEvent(preSkillEvent);
    if (skill is HealingSkill) {
      HealingSkill hSkill = skill as HealingSkill;
      foreach (BattleCharacter c in cTargets) {
        onEvent(new Draconia.Event(this, EventHook.preHealing));
        int amount = skill.calculateHealing(c);
        if (amount > 0) c.onEvent(new Draconia.Event(this, EventHook.preHealed));
        hSkill.activate(c);
        if (amount > 0) c.onEvent(new Draconia.Event(this, EventHook.postHealed));
        Draconia.Event postHealingEvent = new Draconia.Event(this, EventHook.postHealing);
        postHealingEvent.healingDone = amount;
        postHealingEvent.healTarget = c;
        onEvent(postHealingEvent);
      }
    } else {
      foreach (BattleCharacter t in cTargets) {
        onEvent(new Draconia.Event(this, EventHook.preAttack));
        var c = t;
        int damage = skill.calculateDamage(c);
        Draconia.Event preDamageEvent = new Draconia.Event(this, EventHook.preDamage);
        if (damage > 0) {
          c.onEvent(preDamageEvent);
        }
        if (preDamageEvent.newTarget != null) {
          c = preDamageEvent.newTarget;
          c.onEvent(preDamageEvent);
        }
        if (preDamageEvent.finishAttack) {
          skill.activate(c);
          if (c.isAlive()){
            if (damage > 0) {
              Draconia.Event postDamageEvent = new Draconia.Event(this, EventHook.postDamage);
              postDamageEvent.damageTaken = damage;
              c.onEvent(postDamageEvent);
            }
          }
          Draconia.Event postAttackEvent = new Draconia.Event(this, EventHook.postAttack);
          postAttackEvent.damageTaken = damage;
          postAttackEvent.attackTarget = c;
          postAttackEvent.skillUsed = skill;
          onEvent(postAttackEvent);
        }
      }
    }

    foreach (Tile t in tTargets) {
      skill.activate(t);
    }
    Draconia.Event postSkill = new Draconia.Event(this, EventHook.postSkill);
    postSkill.targets = targets;
    postSkill.skillUsed = skill;
    onEvent(postSkill);

    baseChar.gainExp(expGained);
  }

  public FloatingText damageText;
  void floatingText(int val, Color colour, Action callback) {
    damageText.animate(val, colour);
    ui.healthBars.animateToNeutral(previewChange, callback);
    GameManager.get.targetHealth.animateToNeutral(previewChange, () => {});
    GameManager.get.selectedHealth.animateToNeutral(previewChange, () => {});
    previewChange = 0;
  }

  public int calculateDamage(int rawDamage, DamageType type, DamageElement element) {
    int defense = 0;
    if (type == DamageType.physical) {
      defense = physicalDefense;
    } else if (type == DamageType.magical) {
      defense = magicDefense;
    }
    rawDamage = (int)(Math.Max(0, rawDamage - defense));
    float multiplier = 1;
    if (element == DamageElement.ice) {
      multiplier = iceResMultiplier;
    } else if (element == DamageElement.fire) {
      multiplier = fireResMultiplier;
    } else if (element == DamageElement.lightning) {
      multiplier = lightningResMultiplier;
    }
    return (int)(rawDamage*multiplier);
  }

  public int calculateHealing(int rawHeal) {
    return (int)(rawHeal*healingMultiplier);
  }

  public void takeDamage(int damage, BattleCharacter source) {
    if (curHealth <= 0) return;
    floatingText(damage, Color.red, () => {
      curHealth -= damage;
      if (curHealth <= 0) {
        Draconia.Event e = new Draconia.Event(this, EventHook.preDeath);
        onEvent(e);
        if (e.preventDeath) {
          curHealth = 1;
        } else {
          onDeath(source);
        }
      } else {
        if (model.animator) GameManager.get.waitFor(model.animator,"TakeDamage");
      }
    });
  }

  public void takeHealing(int amount) {
    floatingText(amount, Color.green, () => {
      curHealth = Math.Min(maxHealth, curHealth + amount);
    });
  }

  IEnumerator fadeOut() {

    Coroutine c = GameManager.get.waitFor(model.animator, "Death");
    yield return GameManager.get.waitUntilPopped(c);

    SkinnedMeshRenderer r = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
    List<Pair<Material,Color>> matcolors = new List<Pair<Material,Color>>();
    foreach(Material m in r.materials) {
      matcolors.Add(new Pair<Material,Color>(m,m.color));
    }
    for(int i = 0; i < 10; i++) {
      foreach(Pair<Material,Color> mc in matcolors) {
        mc.first.color = Color.Lerp(mc.second,Color.black,i*0.1f);
      }
      yield return new WaitForSeconds(0.1f);
    }
    gameObject.SetActive(false);
  }

  public void onDeath(BattleCharacter source) {
    ActionQueue.get.remove(this);
    Draconia.Event e = new Draconia.Event(this, EventHook.postDeath);
    e.killer = source;
    onEvent(e);

    if (model.animator) GameManager.get.waitFor(StartCoroutine(fadeOut()));
    else gameObject.SetActive(false);

    curTile.occupant = null;

    // remove all effects on death
    foreach (LinkedListNode<Effect> n in new NodeIterator<Effect>(allEffects)) {
      removeEffect(n.Value);
    }
  }

  public int getExpGained(ActiveSkill skill) {
    int exp = skill.expGainUse;
    return exp;
  }

  public bool isAlive() {
    return curHealth > 0;
  }

  public void updateLifeBars(int change = 0) {
    ui.updateLifeBars(change);
  }

  public void updateActionBar(float timePassed) {
    curAction = Math.Min(curAction + speed*timePassed, maxAction);
  }

  public void face(Vector3 posn) {
    // Find direction
    Vector3 dir = posn - gameObject.transform.position;
    // Remove y
    dir = new Vector3(dir.x, 0, dir.z);

    // Set facing
    // The from vector needs the 0.01f in the x in order to make the 180 degree rotation unambiguous
    Quaternion angle = Quaternion.FromToRotation(new Vector3(0.01f, 0, 1), dir);
    model.transform.rotation = angle;
  }

  private Attributes totalAttr { get { return attr + attrChange; } }

  public int strength {
    get {return (int)Math.Max(0, totalAttr.strength);}
  }
  public int intelligence {
    get {return (int)Math.Max(0, totalAttr.intelligence); }
  }
  public int speed {
    get {return (int)Math.Max(0, totalAttr.speed);}
  }
  public int maxHealth {
    get {return (int)Math.Max(0, totalAttr.maxHealth);}
  }
  public int moveRange {
    get {return (int)Math.Max(0, totalAttr.moveRange);}
  }
  public float moveTolerance {
    get {return (int)Math.Max(1, totalAttr.moveTolerance);}
  }
  public int physicalDefense {
    get {return (int)Math.Max(0, totalAttr.physicalDefense);}
  }
  public int magicDefense {
    get {return (int)Math.Max(0, totalAttr.magicDefense);}
  }
  public float healingMultiplier {
    get {return (int)Math.Max(0, totalAttr.healingMultiplier);}
  }
  public int fireResistance {
    get {return (int)Math.Min((int)Math.Max(0, totalAttr.fireResistance),100);}
  }
  public int iceResistance {
    get {return (int)Math.Min((int)Math.Max(0, totalAttr.iceResistance),100);}
  }
  public int lightningResistance {
    get {return (int)Math.Min((int)Math.Max(0, totalAttr.lightningResistance),100);}
  }
  public float fireResMultiplier {
    get {return (100f-fireResistance)/100f;}
  }
  public float iceResMultiplier {
    get {return (100f-iceResistance)/100f;}
  }
  public float lightningResMultiplier {
    get {return (100f-lightningResistance)/100f;}
  }
}
