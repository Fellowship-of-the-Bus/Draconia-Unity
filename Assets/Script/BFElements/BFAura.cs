using UnityEngine;
using System.Collections.Generic;
using System;

public enum BFAuraType {
  regen, burn,
}

public enum BFAuraActivationCriteria {
  majority,
  singleTeam,
  alwaysPlayers,
  alwaysEnemies,
  alwaysNone,
}

public enum BFAuraShape {
  circle,
  square,
  diamond,
  arbitrary
}

public class BFAura: BFElement {
  public Tile[] activationTiles;
  public Tile centerTile;
  public BFAuraType type;
  public BFAuraActivationCriteria criteria;
  public BFAuraShape shape;
  public int radius;
  public int auraEffectValue;
  //should apply to enemies rather than allies?
  public bool appliesToEnemies = false;
  public List<Tile> effectTiles = new List<Tile>();
  public Dictionary<BattleCharacter, Effect> appliedEffects = new Dictionary<BattleCharacter, Effect>();
  public Team controllingTeam = Team.None;
  public GameObject aoeIndicator;
  //when aura is on
  public Material onMaterial;
  //when aura is off
  public Material offMaterial;
  Renderer aoeRenderer;


  public class BFAuraEffectFactory {
    public static Effect getEffect(BFAura aura) {
      Effect e = null;
      switch(aura.type) {
        case BFAuraType.regen:
          e = new RegenerationEffect();
          e.effectValue = aura.auraEffectValue;
          break;
        case BFAuraType.burn:
          e = new BurnEffect();
          e.effectValue = aura.auraEffectValue;
          break;
      }
      return e;
    }
  }

  new protected void Start() {
    base.Start();
    switch(shape) {
      case BFAuraShape.circle:
        foreach(Tile t in GameManager.get.map.tiles) {
          float dx = Mathf.Abs(centerTile.position.x - t.position.x);
          float dz = Mathf.Abs(centerTile.position.z - t.position.z);
          if (dx*dx + dz*dz <= radius*radius) {
            effectTiles.Add(t);
          }
        }
        break;
      case BFAuraShape.square:
        foreach(Tile t in GameManager.get.map.tiles) {
          float dx = Mathf.Abs(centerTile.position.x - t.position.x);
          float dz = Mathf.Abs(centerTile.position.z - t.position.z);
          if (dx <= radius && dz <= radius) {
            effectTiles.Add(t);
          }
        }
        break;
      case BFAuraShape.diamond:
        foreach(Tile t in GameManager.get.map.tiles) {
          float dx = Mathf.Abs(centerTile.position.x - t.position.x);
          float dz = Mathf.Abs(centerTile.position.z - t.position.z);
          if (dx + dz <= radius) {
            effectTiles.Add(t);
          }
        }
        break;
    }
    switch (actShape) {
      case BFElementActivationShape.single:
        activationTiles = new Tile[] {centerTile};
        break;
      default:
        break;
    }
    if (criteria == BFAuraActivationCriteria.alwaysNone) {
      appliesToEnemies = true;
      controllingTeam = Team.None;
    }
    aoeIndicator.transform.localScale = new Vector3(2*radius+1, 2*radius+1, 2*radius+1);
    aoeRenderer = aoeIndicator.GetComponent<MeshRenderer>();
    updateAuraGraphics();
  }

  public override void init() {
    controllingTeam = controller();
    applyAura();
  }

  Tile previousTile = null;
  protected override void onPreMove(BattleCharacter character) {
    previousTile = character.curTile;
  }

  protected override void onPostMove(BattleCharacter character) {
    controllingTeam = controller();
    removeAura();
    applyAura();
    updateAuraGraphics();
  }

  void removeAura() {
    var keys = new List<BattleCharacter>(appliedEffects.Keys);
    foreach (BattleCharacter c in keys) {
      removeAura(c);
    }
  }

  void removeAura(BattleCharacter character){
    if (appliedEffects.ContainsKey(character)) {
      character.removeEffect(appliedEffects[character]);
      appliedEffects.Remove(character);
    }
  }

  void applyAura() {
    foreach(Tile t in effectTiles) {
      if (t.occupant != null)
        applyAura(t.occupant);
    }
  }
  void applyAura(BattleCharacter character) {
    //always None will apply effect to all units inside
    if (controllingTeam == Team.None && criteria != BFAuraActivationCriteria.alwaysNone) {
      return;
    }
    //either aura applies to enemies and character is an enemy of the controlling team
    //or aura applies to allies and character not an enemy of the controlling team
    if ((character.isEnemyOf(controllingTeam) && appliesToEnemies) ||
        (!character.isEnemyOf(controllingTeam) && !appliesToEnemies)) {
      Effect e = BFAuraEffectFactory.getEffect(this);
      character.applyEffect(e);
      appliedEffects.Add(character, e);
    }
  }
  //Function for determining who controls the aura and who the aura should apply to
  private Team controller() {
    bool hasOccupants = false;
    Dictionary<Team, int> activationOccupants = new Dictionary<Team, int>();
    activationOccupants.Add(Team.Player,0);
    activationOccupants.Add(Team.Enemy,0);
    activationOccupants.Add(Team.Ally,0);
    foreach (Tile t in activationTiles) {
      if (t.occupant == null) {
        continue;
      }
      hasOccupants = true;
      activationOccupants[t.occupant.team] += 1;
    }
    if (! hasOccupants) {
      return Team.None;
    }
    switch(criteria){
      case BFAuraActivationCriteria.majority:
        if (activationOccupants[Team.Player] + activationOccupants[Team.Ally]
            > activationOccupants[Team.Enemy]) {
          return Team.Player;
        } else if (activationOccupants[Team.Player] + activationOccupants[Team.Ally]
            < activationOccupants[Team.Enemy]) {
          return Team.Enemy;
        } else {
          return Team.None;
        }
      case BFAuraActivationCriteria.singleTeam:
        if (activationOccupants[Team.Enemy] == 0) {
          return Team.Player;
        } else if (activationOccupants[Team.Player] + activationOccupants[Team.Ally] == 0) {
          return Team.Enemy;
        } else {
          return Team.None;
        }
      case BFAuraActivationCriteria.alwaysPlayers:
        return Team.Player;
      case BFAuraActivationCriteria.alwaysEnemies:
        return Team.Enemy;
      default:
        return Team.None;
    }
  }

  public bool isActive() {
    return controllingTeam != Team.None || criteria == BFAuraActivationCriteria.alwaysNone;
  }

  public void updateAuraGraphics() {
    if (isActive()) {
      aoeRenderer.material = onMaterial;
      switch(controllingTeam){
        case Team.None:
          actRenderer.material = actNoneMaterial;
          break;
        case Team.Player:
          actRenderer.material = actAllyMaterial;
          break;
        case Team.Enemy:
          actRenderer.material = actEnemyMaterial;
          break;
        default:
          actRenderer.material = actNoneMaterial;
          break;
      }
    } else {
      aoeRenderer.material = offMaterial;
      actRenderer.material = actNoneMaterial;
    }
  }

}
