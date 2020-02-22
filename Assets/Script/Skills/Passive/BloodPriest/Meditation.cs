using System;

public class Meditation : PassiveSkill {
  public Meditation() {
    name = "Meditation";
  }

  public override string tooltipDescription { get {
    return "Heal <color=lime>" + healingFormula().ToString() + "</color> at the end of each turn.";
  }}

  protected override void onActivate() {
    attachListener(owner, EventHook.endTurn);
  }
  protected override void onDeactivate() {
    detachListener(owner);
  }

  protected override void additionalEffect(Draconia.Event e) {
    owner.takeHealing(healingFormula());
  }

  public int healingFormula() {
    return (int)(1*attributes.healingMultiplier);
  }
}
