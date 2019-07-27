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
    protected override string tooltipDescription { get {
      return "Deal " + tooltipDamage + " damage from afar.";
    }}

    public override int damageFormula() {
      return (int)(baseDamage + self.strength*(2));
    }

    public override void additionalEffects (BattleCharacter target) {
      if (weapon.numUses > 0) {
        weapon.numUses -= 1;
      }
    }
  }
