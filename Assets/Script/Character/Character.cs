using System;

[System.Serializable]
public class Character {
  public SkillTree skills = new SkillTree();
  public Attributes attr = new Attributes();
  public string name = "";
  public Weapon weapon = new Weapon();

  public Character(string name): this() {
    this.name = name;
  }

  public Attributes totalAttr { get { return attr+ weapon.attr; } }


  public Character() {
  }
}
