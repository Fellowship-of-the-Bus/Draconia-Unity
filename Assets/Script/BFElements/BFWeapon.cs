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

  public class BFSkillFactory {
    public static ActiveSkill getSkill(BFWeapon weapon) {
      ActiveSkill skill = null;
      switch(weapon.type) {
        case BFWeaponType.ballista:
          var newSkill = new UseBallista();
          newSkill.weapon = weapon;
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
    if (character.curTile == activationTile && numUses != 0) {
      addSkill(character);
    }
  }

  private void removeSkill(BattleCharacter character) {
    character.equippedSkills[BattleCharacter.numPermSkills] = null;
    GameManager.get.updateSkillButtons();
  }
  private void addSkill(BattleCharacter character) {
    if (character.equippedSkills.Count == BattleCharacter.numPermSkills){
      character.equippedSkills.Add(BFSkillFactory.getSkill(this));
    } else {
      character.equippedSkills[BattleCharacter.numPermSkills] = BFSkillFactory.getSkill(this);
    }
    GameManager.get.updateSkillButtons();
  }

}
