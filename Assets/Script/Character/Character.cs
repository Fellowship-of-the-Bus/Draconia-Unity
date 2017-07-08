using System;

[System.Serializable]
public class Character {


  public SkillTree skills = new SkillTree();
  public Attributes attr = new Attributes();
  public string name = "";
  public Equipment[] gear = new Equipment[]{new Weapon("Unarmed", Weapon.kinds.Blunt, 1, 1),
                                            new Armour("Unarmed", Armour.ArmourKinds.Leather, 1)};
  public Character(string name): this() {
    this.name = name;
  }

  public Attributes totalAttr { get { return attr + gearAttr(); } }


  public Character() {
    attr.maxHealth = 10;
    attr.speed = 2;
    attr.moveRange = 4;
  }

  public void unEquip(Equipment e) {
    e.equippedTo = null;
    gear[e.type] = null;
  }

  public void equip(Equipment e) {
    if (gear[e.type] != null){
      gear[e.type].equippedTo = null;
    }
    gear[e.type] = e;
    e.equippedTo = this;
  }
  public Attributes gearAttr() {
    Attributes attr = new Attributes();
    foreach (Equipment e in gear) {
      if (e != null) {
        attr += e.attr;
      }
    }
    return attr;
  }
}
