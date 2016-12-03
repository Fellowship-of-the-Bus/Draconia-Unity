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
    applyPassives();

    setSkills();

    curHealth = attr.maxHealth;
    moveAI.owner = this;
    attackAI.owner = this;
  }

  void setSkills() {
    foreach (string skillName in skillSet) {
      ActiveSkill skill;

      if (skillName == "Punch") {
        skill = new PunchSkill();
      } else if (skillName == "Ranged") {
        skill = new RangedSkill();
      } else if (skillName == "Aoe") {
        skill = new TestAoeSkill();
      } else if (skillName == "Knockback") {
        skill = new Knockback();
      } else if (skillName == "Cripple") {
        skill = new CrippleSkill();
      } else if (skillName == "KillingBlow") {
        skill = new KillingBlowSkill();
      } else if (skillName == "WarCry") {
        skill = new WarCrySkill();
      } else {
        Debug.Log("Skill not recognized");
        skill = new PunchSkill();
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
    int maxLevel = 0;
    Effect maxEffect = null;
    foreach (Effect e in l) {
      if (e.level > maxLevel) {
        maxLevel = e.level;
        maxEffect = e;
      }
    }

    //if newly applied effect is the highest level
    //activate it and deactivate the highest leveled one.
    if (effect.level > maxLevel) {
      effect.onActivate();
      maxEffect.onDeactivate();
    }
    l.Add(effect);
  }

  public void attackWithSkill(int index, List<Character> targets) {
    onEvent(new Event(this, EventHook.preAttack));
    foreach (Character c in targets) {
      Event e = new Event(this, EventHook.preDamage);
      c.onEvent(e);
      if (e.finishAttack) {
        equippedSkills[index].activate(c);
        onEvent(new Event(this, EventHook.postAttack));
      }
    }
  }

  public void takeDamage(int damage) {
    curHealth -= damage;
    if (curHealth <= 0) {
      ActionQueue.get.remove(gameObject);
      gameObject.SetActive(false);
      curTile.occupant = null;
    }
    onEvent(new Event(this, EventHook.postDamage));
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
