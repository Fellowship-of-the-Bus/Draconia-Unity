
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Reflection;

public class Character : EventManager {
  TypeMap<Heap<Effect>> effects = new TypeMap<Heap<Effect>>();
  //inventory
  //skill tree
  public SkillTree skills = null;
  //stats
  public Attributes attr;

  public List<ActiveSkill> equippedSkills = new List<ActiveSkill>();

  public Tile curTile = null;

  public int curHealth;
  public float maxAction = 1000f;
  public float curAction = 0;
  public float nextMoveTime = 0f;
  public int moveRange = 4;
  public string characterName = "";
  public int team = 0;

  private int previewDamage;
  public int PreviewDamage{
    get { return Math.Min(previewDamage, curHealth); }
    set { previewDamage = value; }
  }

  private int previewHealing;
  public int PreviewHealing{
    get { return Math.Min(previewHealing, attr.maxHealth - curHealth); }
    set { previewHealing = value; }
  }

  public BaseMoveAI moveAI = new BasicMoveAI();
  public BaseAttackAI attackAI = new BasicAttackAI();

  public float moveTolerance = 1.0f;

  public string[] skillSet;

  void Start() {
    skills = new SkillTree(this);
    setSkills();

    curHealth = attr.maxHealth;
    moveAI.owner = this;
    attackAI.owner = this;

    applyPassives();

    ui = gameObject.transform.Find("UI");
  }

  void setSkills() {
    int i = 0;
    foreach (string skillName in skillSet) {
      ActiveSkill skill = null;
      bool invalidSkill = true;;
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
    foreach (PassiveSkill passive in skills.getPassives()) {
      foreach (GameObject o in passive.getTargets()) {
        passive.activate(o.GetComponent<Character>());
      }
    }
  }

  public float calcMoveTime(float time) {
    return nextMoveTime = time + ((maxAction - curAction) / attr.speed);
  }

  public void applyEffect(Effect effect) {
    if (!effects.ContainsKey(effect)) {
      effects.Add(effect, new Heap<Effect>());
    }
    effect.onApply(this);
    Heap<Effect> l = effects.Get(effect);

    //find max level of effects in list
    Effect maxEffect = l.getMax();

    //if newly applied effect is the highest level
    //activate it and deactivate the highest leveled one.
    if (maxEffect != null && effect > maxEffect) {
      maxEffect.onDeactivate();
      effect.onActivate();
    } else if (maxEffect == null) {
      effect.onActivate();
    }
    l.add(effect);
  }


  public void removeEffect(Effect effect) {
    Heap<Effect> l = effects.Get(effect);
    Debug.Assert(l.Count != 0);

    Effect maxEffect = l.getMax();
    l.remove(effect);

    if (effect == maxEffect) {
      effect.onRemove();
      if (l.getMax() != null) {
        l.getMax().onActivate();
      }
    }
  }

  public bool inRange(Character target, int range) {
    return GameManager.get.getTilesWithinRange(curTile, range).Contains(target.curTile);
  }

  public void attackWithSkill(ActiveSkill skill, List<Character> targets) {
    skill.setCooldown();
    if (skill is HealingSkill) {
      HealingSkill hSkill = skill as HealingSkill;
      onEvent(new Event(this, EventHook.preHealing));
      foreach (Character c in targets) {
        int amount = hSkill.calculateHealing(this,c);
        if (amount > 0) c.onEvent(new Event(this, EventHook.preHealed));
        hSkill.activate(c);
        if (amount > 0) c.onEvent(new Event(this, EventHook.postHealed));
      }
      onEvent(new Event(this, EventHook.postHealing));
    } else {
      onEvent(new Event(this, EventHook.preAttack));
      foreach (Character target in targets) {
        var c = target;
        int damage = skill.calculateDamage(this,c);
        Event e = new Event(this, EventHook.preDamage);
        if (damage > 0) {
          c.onEvent(e);
        }
        if (e.newTarget != null) {
          c = e.newTarget;
          c.onEvent(e);
        }
        if (e.finishAttack) {
          skill.activate(c);
          if (c.isAlive()){
            if (damage > 0) {
              Event e2 = new Event(this, EventHook.postDamage);
              e2.damageTaken = damage;
              c.onEvent(e2);
            }
          }
          Event e3 = new Event(this, EventHook.postAttack);
          e3.damageTaken = damage;
          e3.attackTarget = c;
          onEvent(e3);
        }
      }
    }
  }

  Text makeText() {
    GameObject ngo = Instantiate(GameManager.get.text) as GameObject;
    ngo.transform.SetParent(ui, false);

    Text myText = ngo.GetComponent<Text>();
    var phys = ngo.AddComponent<Rigidbody>();
    phys.useGravity = false;
    phys.velocity = new Vector3(0, 1f);
    return myText;
  }

  public void takeDamage(int damage) {
    if (curHealth <= 0) return;
    makeText();
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
    curHealth = Math.Min(attr.maxHealth, curHealth + amount);
  }

  public void onDeath() {
    ActionQueue.get.remove(gameObject);
    gameObject.SetActive(false);
    curTile.occupant = null;
    onEvent(new Event(this, EventHook.postDeath));

  }

  public bool isAlive() {
    return curHealth > 0;
  }

  public void updateLifeBar(GameObject lifebar) {
    Vector3 scale = lifebar.transform.localScale;
    scale.x = (float)curHealth/attr.maxHealth;
    lifebar.transform.localScale = scale;
  }

  public void updateActionBar(float timePassed) {
    curAction = Math.Min(curAction + attr.speed*timePassed, maxAction);
  }
}
