using UnityEngine;
using System.Collections.Generic;
using System;

public class Character : MonoBehaviour {
  Dictionary<int, List<Effect>> effects = new Dictionary<int, List<Effect>>();
  //inventory
  //skill tree
  public SkillTree skills = null;
  //stats
  public Attributes attr;

  public List<Skill> equippedSkills = new List<Skill>();

  public Tile curTile = null;

  public int curHealth;
  // TODO: Maintain current action bar amount
  public float curAction = 0;
  public float nextMoveTime = 0f;
  public int moveRange = 4;
  public int speed = 5;

  void Start() {
    skills = new SkillTree(this);
    applyPassives();

    Skill punch = new PunchSkill();
    punch.level = 1;
    punch.self = this;
    equippedSkills.Add(punch);

    curHealth = attr.maxHealth;
  }

  void Update() {
    //Debug.Log(attr.strength.ToString());
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
    // TODO: Replace 1000 with max action constant and use attr.speed
    return nextMoveTime = time + ((1000f - curAction) / speed);
  }

  public void applyEffect(Effect effect) {
    if (!effects.ContainsKey(effect.id)) {
      effects.Add(effect.id, new List<Effect>());
    }
    effect.onApply(this);
    List<Effect> l = effects[effect.id];

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
      GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().actionQueue.remove(gameObject);
      gameObject.SetActive(false);
      curTile.occupant = null;
    }
  }

  public bool isAlive() {
    return curHealth > 0;
  }
}
