using UnityEngine;
using System.Collections.Generic;

public interface HealingSkill {
  void activate(Character target);
  int healingFormula();
}
