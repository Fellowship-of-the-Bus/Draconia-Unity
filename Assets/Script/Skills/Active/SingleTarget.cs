using UnityEngine;
using System.Collections.Generic;

public abstract class SingleTarget: ActiveSkill {
  public override List<Tile> getTargets() {
    GameManager gm = GameManager.get;
    List<Tile> tiles = gm.map.getTilesWithinRange(self.curTile, range);
    List<Tile> targets = new List<Tile>();
    float height = self.gameObject.GetComponent<MeshFilter>().mesh.bounds.extents.y;
    Vector3 source = new Vector3(self.curTile.transform.position.x, self.curTile.transform.position.y + 2*height/3, self.curTile.transform.position.z);
    foreach (Tile t in tiles) {
      if (t.occupied()) {
        if (!useLos) {
          targets.Add(t);
        } else {
          GameObject o = t.occupant.gameObject;
          float heightOther = o.GetComponent<MeshFilter>().mesh.bounds.extents.y;
          Vector3 target = new Vector3(t.transform.position.x, t.transform.position.y + 2*heightOther/3, t.transform.position.z);
          RaycastHit hitInfo;
          if (gm.checkLine(source, target, out hitInfo)) {
            targets.Add(t);
          }
        }
      }
    }
    targets.Add(self.curTile);
    return targets;
  }


  // public abstract virtual SkillData calculateSkill() {
  //   Heap<SkillData> db = new Heap<SkillData>();

  //   GameManager game = GameManager.get;
  //   Map map = game.map;
  //   List<GameObject> characterObjects = game.players;

  //   List<Tile> possibilities = map.tilesInMoveRange(owner);
  //   possibilities.Add(owner.curTile);

  //   // Debug.Log("=======" + owner.name + "=======");
  //   foreach (Tile tile in possibilities) {
  //     owner.curTile = tile;
  //     int index = 0;
  //     // Debug.Log("Location: " + tile.transform.position);
  //     foreach (ActiveSkill skill in owner.equippedSkills) {
  //       int cur = index++;
  //       if (! skill.canUse()) continue;
  //       getTargets();
  //       List<GameObject> targets = skill.getTargets();
  //       // Debug.Log("Skill " + cur + ", " + skill.name + ", num targets: " + targets.Count);
  //       if (targets.Count == 0) continue;

  //       List<Character> c = new List<Character>(targets.Select(x => x.GetComponent<Character>()));
  //       c = new List<Character>(c.Filter((character) => character.team != owner.team));

  //       foreach (Character ch in c) {
  //         int damage = skill.calculateDamage(ch);
  //         // Debug.Log("character: " + ch.name + " damage: " + damage);
  //         List<Effected> e = new List<Effected>();
  //         e.Add(ch);
  //         new SkillData(this, cur, damage, e, tile);
  //       }
  //     }
  //   }
  //   best = db.getMax();
  //   Vector3 newPosition = best == null ? owner.curTile.transform.position : best.tile.transform.position;
  //   map.setPath(newPosition);
  //   // int damage = best == null ? 0 : best.score;
  //   // Debug.Log("Location: " + newPosition + " damage: " + best.score);
  //   return newPosition;
  // }
}
