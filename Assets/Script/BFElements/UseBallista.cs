public class UseBallista : SingleTarget {
    public BFWeapon weapon;
    public int baseDamage;
    public UseBallista() {
      useLos = false;
      name = "Use Ballista";
      range = 10;
      maxCooldown = -1;
      dType = DamageType.physical;

      targetAlly(true);
      targetEnemy(true);

      strAligned();

      projectileType = ProjectileType.Arrow;
      projectileMoveType = ProjectileMovementType.Parabolic;
    }
    public override string tooltipDescription { get {
      string s = "Deal " + tooltipDamage + " damage from afar.";
      if (weapon.numUses >= 0) {
        s += " Has " + weapon.numUses + " arrow(s) remaining.";
      } else {
        s += " Has all the arrows in the world remaining.";
      }
      return s;
    }}

    public override int damageFormula() {
      return (int)(baseDamage + self.strength*(2));
    }

    public override void additionalEffects (BattleCharacter target) {
      if (weapon.numUses > 0) {
        weapon.numUses -= 1;
      }
    }
    public override bool canUse() {
      return base.canUse() && weapon.numUses != 0;
    }
  }
