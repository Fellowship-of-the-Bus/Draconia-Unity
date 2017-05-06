
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
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
    get {return weapon.attr;}
  }
  public Weapon weapon {
    get { return baseChar.weapon; } 
  }

  // Allow setting skills in editor
  public string[] skillSet;

  public List<ActiveSkill> equippedSkills = new List<ActiveSkill>();

  public Tile curTile = null;

  public int curHealth;
  public float maxAction = 1000f;
  public float curAction = 0;
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

  public void init() {
    setSkills();

    curHealth = maxHealth;
    ai.owner = this;

    applyPassives();
    equippedSkills = skills.getActives(this);

    ui = gameObject.transform.Find("UI");
  }

  void setSkills() {
    int i = 0;
    foreach (string skillName in skillSet) {
      ActiveSkill skill = null;
      bool invalidSkill = true;
      ;
      foreach (Type t in Assembly.GetExecutingAssembly().GetTypes()) {
        if (t.IsSubclassOf(Type.GetType("ActiveSkill")) && t.FullName == skillName) {
          skill = (ActiveSkill)Activator.CreateInstance(t);
        }
        if (equippedSkills.Count > i && t.FullName == skillName && t == equippedSkills[i].GetType()) {
          skill = null;
          invalidSkill = false;
        }
      }
      if (skill == null && invalidSkill) {
        Debug.Log("Skill not recognized");
        skill = new Punch();
      }
      if (skill != null) {
        skill.level = 1;
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

  private Transform ui;

  void Update() {
    bool debugMode = true;
    if (debugMode) {
      //equippedSkills = new List<ActiveSkill>();
      setSkills();
    }
    // rotate overhead UI (health bar) to look at camera
    ui.rotation = Camera.main.transform.rotation; // Take care about camera rotation

    // scale health on health bar to match current HP values
    GameObject lifebar = ui.Find("Health Bar/Health").gameObject;
    updateLifeBar(lifebar);
  }

  public void applyPassives() {
    BattleCharacter c = GetComponent<BattleCharacter>();
    foreach (PassiveSkill passive in skills.getPassives(c)) {
      passive.activate(c);
    }
  }

  public float calcMoveTime(float time, int turns = 1) {
    return time + ((maxAction - curAction) / speed) + ((turns - 1) * (maxAction / speed));
  }

  public bool inRange(BattleCharacter target, int range) {
    return GameManager.get.map.getTilesWithinRange(curTile, range).Contains(target.curTile);
  }

  public void attackWithSkill(ActiveSkill skill, List<Effected> targets) {
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
      foreach (BattleCharacter target in cTargets) {
        onEvent(new Event(this, EventHook.preAttack));
        var c = target;
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
          }
          Event postAttackEvent = new Event(this, EventHook.postAttack);
          postAttackEvent.damageTaken = damage;
          postAttackEvent.attackTarget = c;
          postAttackEvent.skillUsed = skill;
          onEvent(postAttackEvent);
        }
      }
    }

    foreach (Tile target in tTargets) {
      skill.activate(target);
    }
    Event postSkill = new Event(this, EventHook.postSkill);
    postSkill.targets = targets;
    postSkill.skillUsed = skill;
    onEvent(postSkill);
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
    GameManager.get.characters[team].Remove(gameObject);

    gameObject.SetActive(false);
    curTile.occupant = null;
    onEvent(new Event(this, EventHook.postDeath));

    // remove all effects on death
    foreach (LinkedListNode<Effect> n in new NodeIterator<Effect>(allEffects)) {
      removeEffect(n.Value);
    }
  }

  public bool isAlive() {
    return curHealth > 0;
  }

  public void updateLifeBar(GameObject lifebar) {
    Vector3 scale = lifebar.transform.localScale;
    scale.x = (float)curHealth/maxHealth;
    lifebar.transform.localScale = scale;
  }

  public void updateActionBar(float timePassed) {
    curAction = Math.Min(curAction + speed*timePassed, maxAction);
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
