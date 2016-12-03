using UnityEngine;

public abstract class ActiveSkill : Skill {
  public override void activate(Character target) {
    target.takeDamage(calculateDamage(self, target));
    additionalEffects(target);
  }
  public abstract int calculateDamage(Character source, Character target);
  public virtual void additionalEffects(Character target) {

  }
}
