using UnityEngine;
using System.Collections.Generic;

public abstract class PassiveSkill : Effect, Skill {
  public int level;
  int Skill.level { get { return level; } set { level = value; } }

  public BattleCharacter self {get; set;}
  // Character used when outside of map
  public Character character {get; set;}

  protected Attributes attributes {
    get {
      if (self != null) {
        return self.totalAttr;
      } else {
        return character.totalAttr;
      }
    }
  }

  public int range {get; set;}
  public bool useLos {get; set;}

  public override string tooltip { get {
    return tooltipHeader + tooltipDescription + "\n\n<b>Passive</b>";
  }}

  public void activate(BattleCharacter target) {
    this.caster = self;
    target.applyEffect(this);
  }
}
