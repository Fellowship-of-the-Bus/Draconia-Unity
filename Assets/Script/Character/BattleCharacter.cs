using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;

public class BattleCharacter : Effected {
  public Character baseChar = new Character();
  public new string name {
    get { return baseChar.name; }
    set { baseChar.name = value; }
  }

  //inventory
  //skill tree
  public SkillTree skills {
    get { return baseChar.skills; }
  }

  //base stats + permanent stat passives
  public Attributes attr {
    get { return baseChar.attr; }
  }
  //change in stats due to buffs/debuffs
  public Attributes attrChange = new Attributes();
  //Sum of stats from equipments
  public Attributes attrEquip {
    get {if (weapon != null ) return weapon.attr; else return new Attributes();}
  }
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

  public Tile curTile = null;

  public int curHealth;
  public float maxAction = 1000f;
  public float curAction = 0;

  //0 = player, 1 = enemy, 2 = ally
  public int team = 0;

  private int previewDamage;
  public int PreviewDamage{
    get { return Math.Min(previewDamage, curHealth); }
    set { previewDamage = value; }
  }

  private int previewHealing;
  public int PreviewHealing{
    get { return Math.Min(previewHealing, maxHealth - curHealth); }
    set { previewHealing = value; }
  }

  public BaseAI ai = new BasicAI();

  public LinkedList<Effect> allEffects = new LinkedList<Effect>();

  public bool levitating = false;

  public override void applyEffect(Effect effect) {
    base.applyEffect(effect);
    allEffects.AddLast(effect);
  }

  public override void removeEffect(Effect effect) {
    base.removeEffect(effect);
    allEffects.Remove(effect);
  }
  List<String> prevSkillSet = new List<String>();


  public Animator animator;
  public GameObject lifebar;
  GameObject redlifebar;

  void Start(){
    init();
  }
  bool called = false;
  public void init() {
    if (called) return;
    called = true;
    equippedSkills = skills.getActives(this);
    if (Options.debugMode && equippedSkills.IsEmpty()) {
      setSkills();
      prevSkillSet = new List<String>(skillSet.Select(x => x.name));
    }

    curHealth = maxHealth;
    ai.owner = this;

    applyPassives();

    ui = transform.Find("UI");
    animator = gameObject.transform.Find("Model").gameObject.GetComponent<Animator>();
    lifebar = ui.Find("Health Bar/Health").gameObject;
    redlifebar = ui.Find("Health Bar/HealthRed").gameObject;
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
        Debug.Log("Skill not recognized");
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
    BattleCharacter c = GetComponent<BattleCharacter>();
    foreach (SkillData data in passiveSet) {
      PassiveSkill skill = null;
      foreach (Type t in Assembly.GetExecutingAssembly().GetTypes()) {
        if (t.IsSubclassOf(Type.GetType("PassiveSkill")) && t.FullName == data.name) {
          skill = (PassiveSkill)Activator.CreateInstance(t);
          skill.level = data.level;
          skill.self = c;
          skill.activate(c);
        }
      }
    }
    foreach (PassiveSkill passive in skills.getPassives(c)) {
      passive.activate(c);
    }
  }

  void OnValidate() {
    init();
    if (Options.debugMode) {
      setSkills();
    }
  }

  private Transform ui;
  void Update() {
    // rotate overhead UI (health bar) to look at camera
    ui.rotation = Camera.main.transform.rotation; // Take care about camera rotation

    // scale health on health bar to match current HP values
    updateLifeBar(lifebar);
    updateLifeBar(redlifebar);
  }


  public float calcMoveTime(float time, int turns = 1) {
    return time + ((maxAction - curAction) / speed) + ((turns - 1) * (maxAction / speed));
  }

  public bool inRange(BattleCharacter target, int range) {
    return GameManager.get.map.getTilesWithinRange(curTile, range).Contains(target.curTile);
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

    if (animator) GameManager.get.waitFor(animator, "Attack", () => finishSkill(skill, target, targets));
    else finishSkill(skill, target, targets);
    return true;
  }

  void finishSkill(ActiveSkill skill, Tile target, List<Effected> targets) {
    int expGained = getExpGained(skill, null);
    List<BattleCharacter> cTargets = new List<BattleCharacter>();
    List<Tile> tTargets = new List<Tile>();
    foreach(Effected e in targets) {
      if (e is BattleCharacter) cTargets.Add(e as BattleCharacter);
      else tTargets.Add(e as Tile);
    }
    skill.setCooldown();
    Event preSkillEvent = new Event(this, EventHook.preSkill);
    preSkillEvent.skillUsed = skill;
    onEvent(preSkillEvent);
    if (skill is HealingSkill) {
      HealingSkill hSkill = skill as HealingSkill;
      foreach (BattleCharacter c in cTargets) {
        onEvent(new Event(this, EventHook.preHealing));
        int amount = skill.calculateHealing(c);
        if (amount > 0) c.onEvent(new Event(this, EventHook.preHealed));
        hSkill.activate(c);
        if (amount > 0) c.onEvent(new Event(this, EventHook.postHealed));
        Event postHealingEvent = new Event(this, EventHook.postHealing);
        postHealingEvent.healingDone = amount;
        postHealingEvent.healTarget = c;
        onEvent(postHealingEvent);
      }
    } else {
      foreach (BattleCharacter t in cTargets) {
        onEvent(new Event(this, EventHook.preAttack));
        var c = t;
        int damage = skill.calculateDamage(c);
        Event preDamageEvent = new Event(this, EventHook.preDamage);
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
              Event postDamageEvent = new Event(this, EventHook.postDamage);
              postDamageEvent.damageTaken = damage;
              c.onEvent(postDamageEvent);
            }
          } else {
            expGained += getExpGained(skill, c);
          }
          Event postAttackEvent = new Event(this, EventHook.postAttack);
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
    Event postSkill = new Event(this, EventHook.postSkill);
    postSkill.targets = targets;
    postSkill.skillUsed = skill;
    onEvent(postSkill);

    baseChar.gainExp(expGained);
  }

  void floatingText(int val, Color colour) {
    GameObject ngo = Instantiate(GameManager.get.text) as GameObject;
    ngo.transform.SetParent(ui, false);
    Text txt = (Text)ngo.GetComponent<Text>();
    txt.text = val.ToString();
    txt.color = colour;
    var phys = ngo.AddComponent<Rigidbody>();
    phys.useGravity = false;
    phys.velocity = new Vector3(0, 1f);
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

  public void takeDamage(int damage) {
    if (curHealth <= 0) return;
    floatingText(damage, Color.red);
    curHealth -= damage;
    if (curHealth <= 0) {
      Event e = new Event(this, EventHook.preDeath);
      onEvent(e);
      if (e.preventDeath) {
        curHealth = 1;
      } else {
        onDeath();
      }
    }
  }

  public void takeHealing(int amount) {
    floatingText(amount, Color.green);
    curHealth = Math.Min(maxHealth, curHealth + amount);
  }

  public void onDeath() {
    ActionQueue.get.remove(gameObject);

    onEvent(new Event(this, EventHook.postDeath));
    gameObject.SetActive(false);
    curTile.occupant = null;

    // remove all effects on death
    foreach (LinkedListNode<Effect> n in new NodeIterator<Effect>(allEffects)) {
      removeEffect(n.Value);
    }
  }

  //kill = null => get base exp for using skill
  //get = character => get exp for killing it
  public int getExpGained(ActiveSkill skill, BattleCharacter killed) {
    int exp;
    if (killed != null) {
      //TODO: scale exp based on level difference
      exp = killed.baseChar.expGiven;
    } else {
      exp = skill.expGainUse;
    }
    return exp;
  }

  public bool isAlive() {
    return curHealth > 0;
  }

  public void updateLifeBar(GameObject lifebar) {
    updateLifeBar(lifebar, curHealth);
  }

  public void updateLifeBar(GameObject lifebar, int health) {
    Vector3 scale = lifebar.transform.localScale;
    scale.x = (float)health/maxHealth;
    lifebar.transform.localScale = scale;
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
    Quaternion angle = Quaternion.FromToRotation(new Vector3(-1, 0, 0), dir);
    gameObject.transform.Find("Model").rotation = angle;
  }

  private Attributes totalAttr { get { return attr + attrChange + attrEquip; } }

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
