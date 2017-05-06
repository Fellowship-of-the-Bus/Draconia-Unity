using UnityEngine;
using System.Collections.Generic;

public interface HealingSkill {
  void activate(BattleCharacter target);
  int healingFormula();
}
