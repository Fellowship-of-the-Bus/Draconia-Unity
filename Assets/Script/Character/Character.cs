using UnityEngine;
using System.Collections.Generic;
using System;

public class Character : MonoBehaviour {
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

  public int previewDamage;
  public int PreviewDamage{
    get { return Math.Min(previewDamage, curHealth); }
    set { previewDamage = value; }
  }

  public BaseMoveAI moveAI = new BasicMoveAI();
  public BaseAttackAI attackAI = new BasicAttackAI();

  public float heightTolerance = 1.0f;

  void Start() {
    skills = new SkillTree(this);
    applyPassives();


    ActiveSkill ranged = new RangedSkill();
    ranged.level = 1;
    ranged.self = this;
    equippedSkills.Add(ranged);

    ActiveSkill aoe = new TestAoeSkill();
    aoe.level = 1;
    aoe.self = this;
    equippedSkills.Add(aoe);

    ActiveSkill punch = new PunchSkill();
    punch.level = 1;
    punch.self = this;
    equippedSkills.Add(punch);


    curHealth = attr.maxHealth;
    moveAI.owner = this;
    attackAI.owner = this;
  }

  void Update() {
    // rotate overhead UI (health bar) to look at camera
    Transform ui = gameObject.transform.Find("UI");
    ui.rotation = Camera.main.transform.rotation; // Take care about camera rotation

    // scale health on health bar to match current HP values
    GameObject lifebar = ui.Find("Health Bar/Health").gameObject;
    updateLifeBar(lifebar);
  }

  public void applyPassives() {
    foreach (PassiveSkill passive in skills.getPassives()) {
      List<Character> targets = new List<Character>();
      foreach (GameObject o in passive.getTargets()) {
        targets.Add(o.GetComponent<Character>());
      }
      passive.activate(targets);
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

  public void selectSkill(int i) {

  }

  public void takeDamage(int damage) {
    curHealth -= damage;
    if (curHealth <= 0) {
      ActionQueue.get.remove(gameObject);
      gameObject.SetActive(false);
      curTile.occupant = null;
    }
  }

  public bool isAlive() {
    return curHealth > 0;
  }

  public void setDamageIndicator(int damage) {

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
