using UnityEngine;
using System.Collections.Generic;
using System;

public enum BFWeaponType {
  ballista,
}

public class BFWeapon : BFElement {
  public Tile activationTile;
  public int numUses;
  public BFWeaponType type;
  public int baseDamage;

  public class BFSkillFactory {
    public static ActiveSkill getSkill(BFWeapon weapon) {
      ActiveSkill skill = null;
      switch(weapon.type) {
        case BFWeaponType.ballista:
          var newSkill = new UseBallista();
          newSkill.weapon = weapon;
          newSkill.baseDamage = weapon.baseDamage;
          skill = newSkill;
          break;
      }
      skill.self = weapon.activationTile.occupant;
      return skill;
    }
  }
  protected override void onPreMove(BattleCharacter character) {
    if (character.curTile == activationTile) {
      removeSkill(character);
    }
  }
  protected override void onPostMove(BattleCharacter character) {
    if (character.curTile == activationTile) {
      addSkill(character);
    }
  }

  private void removeSkill(BattleCharacter character, bool temp = false) {
    character.equippedSkills[BattleCharacter.numPermSkills] = null;
    if (!temp) {
      GameManager.get.updateSkillButtons();
    }
  }
  private void addSkill(BattleCharacter character, bool temp = false) {
    ActiveSkill skill = BFSkillFactory.getSkill(this);
    skill.self = character;
    if (character.equippedSkills.Count == BattleCharacter.numPermSkills){
      character.equippedSkills.Add(skill);
    } else {
      character.equippedSkills[BattleCharacter.numPermSkills] = skill;
    }
    if (!temp) {
      GameManager.get.updateSkillButtons();
    }
  }

  public void addSkillTemp(BattleCharacter character) {
    addSkill(character, true);
  }
  public void removeSkillTemp(BattleCharacter character) {
    removeSkill(character, true);
  }

}
