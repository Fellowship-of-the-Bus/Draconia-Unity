using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Linq;

public abstract class BaseAI {
  public BattleCharacter owner;
  protected SkillData best;
  public virtual void init() {}
  public abstract Vector3 move();

  protected struct TargetSet {
    public Tile tile;
    public List<BattleCharacter> affected;
  }

  public virtual void target() {
    if (best != null) {
      GameManager.get.attackTarget(best.targetTile);
    }
  }

  protected List<TargetSet> getTargetSets(ActiveSkill skill, List<Tile> targets, Tile userTile) {
    AoeSkill aoe = skill as AoeSkill;

    // Determine possible targets
    List<TargetSet> targetSets = new List<TargetSet>();
    if (aoe != null) {
      foreach(Tile t in targets) {
        TargetSet tSet;
        tSet.tile = t;

        List<Tile> affectedTiles = aoe.getTargetsInAoe(t.gameObject.transform.position);
        affectedTiles = new List<Tile>(affectedTiles.Filter((x) => x.occupied()));
        tSet.affected = new List<BattleCharacter>(affectedTiles.Select(x => x.occupant));
        if (affectedTiles.Contains(userTile)) {
          tSet.affected.Add(owner);
        } else {
          tSet.affected.Remove(owner);
        }

        targetSets.Add(tSet);
      }
    } else {
      List<BattleCharacter> chars = new List<BattleCharacter>(targets.Select(x => x.occupant));
      chars = new List<BattleCharacter>(chars.Filter((character) => character != null));
      foreach (BattleCharacter t in chars) {
        TargetSet tSet;
        tSet.tile = t.curTile;

        List<BattleCharacter> singleton = new List<BattleCharacter>();
        singleton.Add(t);
        tSet.affected = singleton;

        targetSets.Add(tSet);
      }
    }

    return targetSets;
  }

  protected SkillData evaluateSkillOptions(Tile tile) {
    Heap<SkillData> db = new Heap<SkillData>();
    List<BFElement.BFIdEffect> ids = tile.getEffectsOfaKind<BFElement.BFIdEffect>();
    ActiveSkill tempSkill = null;
    //only one skill giving element exists
    foreach(BFElement.BFIdEffect id in ids) {
      BFWeapon weapon = id.element as BFWeapon;
      if (weapon != null) {
        tempSkill = BFWeapon.BFSkillFactory.getSkill(weapon);
        tempSkill.self = owner;
        break;
      }
    }
    int index = 0;
    List<ActiveSkill> activeSkills = new List<ActiveSkill>(owner.equippedSkills);
    activeSkills[BattleCharacter.numPermSkills] = tempSkill;
    foreach (ActiveSkill skill in activeSkills) {
      index++;
      if (skill == null) {
        continue;
      }
      // Skip unusable skills
      if (!skill.canUse()) continue;
      List<Tile> targets = skill.getTargets(tile);
      if (targets.Count == 0) continue;

      List<TargetSet> targetCharacters = getTargetSets(skill, targets, tile);

      // Calculate net change in team health difference
      foreach (TargetSet tSet in targetCharacters) {
        List<BattleCharacter> c = tSet.affected;
        List<Effected> effected = new List<Effected>();
        int netChange = 0;

        foreach (BattleCharacter ch in c) {
          if (skill is HealingSkill) {
            int val = Math.Min(skill.calculateHealing(ch), ch.maxHealth - ch.curHealth);
            if (ch.isEnemyOf(owner)) {
              netChange -= val;
            } else {
              netChange += val;
            }
          } else {
            int val = skill.calculateDamage(ch, tile);
            if (ch.isEnemyOf(owner)) {
              netChange += val;
            } else {
              netChange -= val;
            }
          }

          effected.Add(ch);
        }

        if (netChange > 0) {
          db.add(new SkillData(this, index - 1, netChange, effected, activeSkills[index-1], tile, tSet.tile));
        }
      }
    }

    return db.getMax();
  }

  public bool willAttack() {
    return best != null;
  }
}
