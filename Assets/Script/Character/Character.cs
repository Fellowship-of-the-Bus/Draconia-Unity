
using UnityEngine;
using System.Collections.Generic;
using System;

public class Character : EventManager {
  TypeMap<List<Effect>> effects = new TypeMap<List<Effect>>();
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

  public int previewDamage;
  public int PreviewDamage{
    get { return Math.Min(previewDamage, curHealth); }
    set { previewDamage = value; }
  }

  public BaseMoveAI moveAI = new BasicMoveAI();
  public BaseAttackAI attackAI = new BasicAttackAI();

  public float moveTolerance = 1.0f;

  public string[] skillSet;

  new void Start() {
    base.Start();
    skills = new SkillTree(this);

    setSkills();

    curHealth = attr.maxHealth;
    moveAI.owner = this;
    attackAI.owner = this;

    applyPassives();
  }

  void setSkills() {
    foreach (string skillName in skillSet) {
      ActiveSkill skill;

      if (skillName == "Punch") {
        skill = new Punch();
      } else if (skillName == "Ranged") {
        skill = new Ranged();
      } else if (skillName == "Aoe") {
        skill = new TestAoe();
      } else if (skillName == "Knockback") {
        skill = new Knockback();
      } else if (skillName == "Cripple") {
        skill = new Cripple();
      } else if (skillName == "KillingBlow") {
        skill = new KillingBlow();
      } else if (skillName == "WarCry") {
        skill = new WarCry();
      }  else if (skillName == "SkullBash") {
        skill = new SkullBash();
      } else if (skillName == "Puncture") {
        skill = new Puncture();
      } else {
        Debug.Log("Skill not recognized");
        skill = new Punch();
      }

      skill.level = 1;
      skill.self = this;
      equippedSkills.Add(skill);
    }
  }

  void Update() {
    bool debugMode = true;
    if (debugMode) {
      equippedSkills = new List<ActiveSkill>();
      setSkills();
    }
    // rotate overhead UI (health bar) to look at camera
    Transform ui = gameObject.transform.Find("UI");
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
      effects.Add(effect, new List<Effect>());
    }
    effect.onApply(this);
    List<Effect> l = effects.Get(effect);

    //if list is empty
    if (l.Count == 0) {
      effect.onActivate();
      l.Add(effect);
      return;
    }
    //find max level of effects in list
    Effect maxEffect = null;
    foreach (Effect e in l) {
      if (maxEffect == null) {
        maxEffect = e;
      } else if (e.isGreaterThan(maxEffect)) {
        maxEffect = e;
      }
    }

    //if newly applied effect is the highest level
    //activate it and deactivate the highest leveled one.
    if (effect.isGreaterThan(maxEffect)) {
      effect.onActivate();
      maxEffect.onDeactivate();
    }
    l.Add(effect);
  }


  public void removeEffect(Effect effect) {
    List<Effect> l = effects.Get(effect);
    if (l.Count == 0) {
      return;
    }

    l.Remove(effect);

    if (l.Count == 0) {
      effect.onDeactivate();
      return;
    }

    //find max level of effects in list
    Effect maxEffect = null;
    foreach (Effect e in l) {
      if (maxEffect == null) {
        maxEffect = e;
      } else if (e.isGreaterThan(maxEffect)) {
        maxEffect = e;
      }
    }

    //if we are strongest, deactivate and activate next maximum effect
    if (effect.isGreaterThan(maxEffect)) {
      effect.onDeactivate();
      maxEffect.onActivate();
    }
  }

  public bool inRange(Character target, int range) {
    return GameManager.get.getTilesWithinRange(curTile, range).Contains(target.curTile);
  }

  public void attackWithSkill(ActiveSkill skill, List<Character> targets) {
    onEvent(new Event(this, EventHook.preAttack));
    foreach (Character c in targets) {
      int damage = skill.calculateDamage(this,c);
        Event e = new Event(this, EventHook.preDamage);
      if (damage > 0) {
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
        onEvent(new Event(this, EventHook.postAttack));
      }
    }
  }

  public void takeDamage(int damage) {
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
