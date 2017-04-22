using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

public class BasicAI : BaseAI {
  private class SkillData : IComparable<SkillData> {
    public int index;  // index in equippedSkills
    public int score;  // how good is this skill right now?
    public ActiveSkill skill;
    public List<Effected> effected;

    GameManager game = GameManager.get;
    Map map = game.map;

    public SkillData(BasicAI ai, int index, int score, List<Effected> effected) {
      this.index = index;
      this.score = score;
      this.effected = effected;
      this.skill = ai.owner.equippedSkills[index];
    }

    public int CompareTo(SkillData other) {
      return score.CompareTo(other.score);
    }
  }

  public override void target() {
    Heap<SkillData> db = new Heap<SkillData>();
    int index = 0;
    foreach (ActiveSkill skill in owner.equippedSkills) {
      int cur = index++;
      if (! skill.canUse()) continue;
      List<GameObject> targets = skill.getTargets();
      if (targets.Count == 0) continue;
      List<Character> c = new List<Character>(targets.Take(1).Select(x => x.GetComponent<Character>()));
      c = new List<Character>(c.Filter((character) => character.team != owner.team));
      List<Effected> e = new List<Effected>(c.Map(x => x as Effected));

      int damage = 0;
      foreach (Character ch in c) {
        damage += skill.calculateDamage(ch);
      }
      db.add(new SkillData(this, cur, damage, e));
    }
    SkillData best = db.getMax();
    if (best != null) {
      GameManager.get.SelectedSkill = best.index;
      owner.attackWithSkill(best.skill, best.effected);
    }
  }

  public override Vector3 move() {
    List<GameObject> characterObjects = game.players;

    int minDistance = Int32.MaxValue/2;
    foreach (GameObject o in characterObjects) {
      map.setPath(o.transform.position);
      if (map.path.Count < minDistance) {
        minDistance = map.getTile(o.transform.position).distance;
        break;
      }
    }
    Tile destination = owner.curTile;

    for (LinkedListNode<Tile> t = map.path.Last; t != map.path.First; t = t.Previous) {
      if (t.Value.distance <= owner.moveRange && !t.Value.occupied()) {
        destination = t.Value;
        break;
      }
    }
    return destination.gameObject.transform.position;
  }
}
