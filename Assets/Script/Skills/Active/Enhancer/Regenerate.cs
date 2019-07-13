using UnityEngine;
using System.Collections.Generic;

public class Regenerate: SingleTarget, HealingSkill {

  public override string animation { get { return "ClericCast"; }}
  public override Color castColor { get { return castColorEnhancer; }}

  public Regenerate() {
    range = 3;
    useWepRange = false;
    useLos = false;
    name = "Regenerate";
    maxCooldown = 2;
    canTargetSelf = true;
    targetAlly(true);
    targetEnemy(false);
  }

  public int healingFormula() {
    return 0;
  }

  public override void additionalEffects(BattleCharacter target) {
    RegenerationEffect buff = new RegenerationEffect();
    buff.duration = (level+5)/2;
    //something about the damage needs to be addressed
    buff.effectValue = (int)System.Math.Max((int)calculateDamage(target)*(0.2f + 0.1f*level), 1);
    buff.caster = self;
    target.applyEffect(buff);


    Draconia.Event buffEvent = new Draconia.Event(self, EventHook.useBuffSkill);
    buffEvent.appliedBuff = buff;
    self.onEvent(buffEvent);
  }
}
