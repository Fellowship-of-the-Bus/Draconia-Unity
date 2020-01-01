using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class SkillList {
  public readonly static Dictionary<String, Type[][]> skillsByClass = new Dictionary<String, Type[][]>()
  {
    { "Berserker", new Type[][] {
      new Type[] { Type.GetType("WarCry") },
      new Type[] { Type.GetType("Cripple"), Type.GetType("KillingBlow") },
      new Type[] { Type.GetType("CircleSlash"), Type.GetType("Berserk") },
      new Type[] { Type.GetType("Adrenaline") }
    }},
    { "Blood Priest", new Type[][] {
      new Type[] { Type.GetType("Reflect") },
      new Type[] { Type.GetType("DarkPower"), Type.GetType("Meditation") },
      new Type[] { Type.GetType("BloodSanctuary"), Type.GetType("Transfusion") },
      new Type[] { Type.GetType("ShareLife"), Type.GetType("BloodJudgement") }
    }},
    { "Cleric", new Type[][] {
      new Type[] { Type.GetType("HealingRay") },
      new Type[] { Type.GetType("HealingTouch"), Type.GetType("HealWhenHit") },
      new Type[] { Type.GetType("HealingCircle"), Type.GetType("Purge") },
    }},
    { "Dead Shot", new Type[][] {
      new Type[] { Type.GetType("PointBlankShot"), Type.GetType("ForceShot") },
      new Type[] { Type.GetType("HookShot"), Type.GetType("PiercingShot") },
    }},
    { "Guardian", new Type[][] {
      new Type[] { Type.GetType("SkullBash") },
      new Type[] { Type.GetType("Knockback"), Type.GetType("IronSkin") },
      new Type[] { Type.GetType("Intercept"), Type.GetType("Vengeance") },
    }},
    { "Ice Wizard", new Type[][] {
      new Type[] { Type.GetType("IceSpear") },
      new Type[] { Type.GetType("IceBlock"), Type.GetType("FrostArmor") },
      new Type[] { Type.GetType("BrainFreeze"), Type.GetType("SlowAura") },
      new Type[] { Type.GetType("Blizzard") }
    }},
    { "Ninja", new Type[][] {
      new Type[] { Type.GetType("Puncture") },
      new Type[] { Type.GetType("Shuriken"), Type.GetType("Dodge") },
      new Type[] { Type.GetType("Climb") },
    }},
    { "Pyromancer", new Type[][] {
      new Type[] { Type.GetType("Fireball") },
      new Type[] { Type.GetType("FireCross"), Type.GetType("IgniteWeapon") },
      new Type[] { Type.GetType("ScorchEarth"), Type.GetType("CooldownReduction") },
      new Type[] { Type.GetType("FireStorm") }
    }},
    { "Ranger", new Type[][] {
      new Type[] { Type.GetType("SerratedShot") },
      new Type[] { Type.GetType("LegShot"), Type.GetType("BluntShot") },
      new Type[] { Type.GetType("PoisonShot"), Type.GetType("FlameShot") },
    }},
    { "Sniper", new Type[][] {
      new Type[] { Type.GetType("ArcShot") },
      new Type[] { Type.GetType("Entrench"), Type.GetType("Grapple") },
      new Type[] { Type.GetType("Volley"), Type.GetType("Sentry") },
    }},
    { "Warlock", new Type[][] {
      new Type[] { Type.GetType("Disable") },
      new Type[] { Type.GetType("LifeDrain"), Type.GetType("BloodSacrifice") },
      new Type[] { Type.GetType("ExsanguinationAura"), Type.GetType("Portal") },
      new Type[] { Type.GetType("RealityDistortion"), Type.GetType("Inspiration") }
    }},
  };

  public readonly static SkillList get = new SkillList();

  public List<Type> skills = new List<Type>();
  public Dictionary<Type, Sprite> skillImages = new Dictionary<Type, Sprite>();

  private SkillList() {
    Type skill = Type.GetType("Skill");
    foreach (Type t in Assembly.GetExecutingAssembly().GetTypes()) {
      if (skill.IsAssignableFrom(t) && !t.IsAbstract) {
        skills.Add(t);
      }
    }
    skills.Sort((a,b) => {
      return String.Compare(a.displayName(), b.displayName());
    });
  }

  bool dictCreated = false;
  public void createDict() {
    if (dictCreated) return;
    dictCreated = true;
    Type skill = Type.GetType("Skill");
    foreach (Type t in Assembly.GetExecutingAssembly().GetTypes()) {
      if (skill.IsAssignableFrom(t) && !t.IsAbstract) {
        skillImages.Add(t, Resources.Load<Sprite>("Skill Images/" + t.ToString()));
      }
    }
  }
}
