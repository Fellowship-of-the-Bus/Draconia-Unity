using System;

[System.Serializable]
public class Character {
  public SkillTree skills;
  public Attributes attr;
  public string name;
  public Weapon weapon;

  public Character() {
  	skills = new SkillTree();
  	attr = new Attributes();
  	name = "";
  	weapon = new Weapon();

    skills.addSkill<Cripple>();
    skills.addSkill<Knockback>();
    skills.addSkill<Cripple>();
  }
}
