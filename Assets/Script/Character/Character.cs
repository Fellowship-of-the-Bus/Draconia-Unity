using UnityEngine;
using System.Collections.Generic;
using System;

public class Character : MonoBehaviour {
  Dictionary<int, List<Effect>> effects = new Dictionary<int, List<Effect>>();
  //inventory
  //skill tree
  public SkillTree skills = null;
  //stats
  public Attributes attr = new Attributes();
  List<Skill> equippedSkills = new List<Skill>();

  public int curHealth;
  public int moveRange = 4;

  void Start() {
    skills = new SkillTree(this);
    applyPassives();
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

    //if newly applyed effect is the highest level
    //activate it and deactivate the highest leveled one.
    if (effect.level > maxLevel) {
      effect.onActivate();
      maxEffect.onDeactivate();
    }
    l.Add(effect);
  }

}
