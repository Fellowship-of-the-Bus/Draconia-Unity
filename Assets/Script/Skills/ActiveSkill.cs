using UnityEngine;

public abstract class ActiveSkill : Skill {
  public abstract int calculateDamage(Character source, Character target);
}
