using System;
using System.Collections.Generic;
using UnityEngine;

public class Map {
  readonly Vector3 portalDir = Vector3.one;

  public LinkedList<Tile> path = null;
  List<Tile> tiles = new List<Tile>();
  List<GameObject> cubes;

  public void awake() {
    cubes = new List<GameObject>(GameObject.FindGameObjectsWithTag("Cube"));
    foreach (GameObject cube in cubes) {
      cube.AddComponent<Tile>();
      Tile t = cube.GetComponent<Tile>();
      tiles.Add(t);
    }
    path = new LinkedList<Tile>();
  }

  public void djikstra(Vector3 unitLocation, Character charToMove) {
    foreach (Tile tile in tiles) {
      tile.distance = System.Int32.MaxValue/2;
      tile.dir = Vector3.zero;
    }

    HashSet<Tile> tilesToGo = new HashSet<Tile>(tiles);

    Tile startTile = getTile(unitLocation);
    startTile.distance = 0;

    while (tilesToGo.Count != 0) {
      int minDistance = System.Int32.MaxValue;
      Tile minTile = null;
      foreach (Tile tile in tilesToGo) {
        if (tile.distance <= minDistance) {
          minDistance = tile.distance;
          minTile = tile;
        }
      }

      Vector3[] directions = new Vector3[]{ Vector3.forward, Vector3.back, Vector3.right, Vector3.left };
      foreach (Vector3 dir in directions) {
        Vector3 neighbour = minTile.gameObject.transform.position + dir;
        Tile neighbourTile = getTile(neighbour, tilesToGo);
        if (neighbourTile != null) {
          int d = minTile.distance + distance(neighbourTile, minTile, charToMove.moveTolerance);
          if (d < neighbourTile.distance) {
            neighbourTile.distance = d;
            neighbourTile.dir = dir;
          }
          // update portal dest distance to portal src distance
          PortalEffect portal = neighbourTile.getEffect<PortalEffect>();
          if (portal != null) {
            Tile sibling = portal.sibling.ownerTile;
            if (d < sibling.distance) {
              sibling.distance = d;
              sibling.dir = portalDir;
            }
          }
        }
      }
      tilesToGo.Remove(minTile);
    }
  }

  public int distance(Tile from, Tile to, float moveTolerance) {
    //check heights
    if (Math.Abs(getHeight(from) - getHeight(to)) > moveTolerance) {
      return Int32.MaxValue/2;
    }
    if (from.occupied() && !(GameManager.get.SelectedPiece.GetComponent<Character>().team == from.occupant.GetComponent<Character>().team)) {
      return Int32.MaxValue/4;
    }

    return Math.Max((int)(to.movePointSpent - moveTolerance + 1), 1);
  }

  public float getHeight(Tile t) {
    float scale = t.getHeight();//t.gameObject.transform.localScale.y;
    return scale + 0.5f;
  }


  public Tile getTile(Vector3 location) {
    return getTile(location, tiles);
  }

  public Tile getTile(Vector3 location, IEnumerable<Tile> list) {
    foreach (Tile tile in list) {
      if (Math.Abs(tile.gameObject.transform.position.x - location.x) < 0.05f &&
          Math.Abs(tile.gameObject.transform.position.z - location.z) < 0.05f) {
        return tile;
      }
    }
    return null;
  }

  public List<Tile> getTilesWithinRange(Tile t, int range) {
    List<Tile> inRangeTiles = new List<Tile>();
    foreach (Tile other in tiles) {
      int rangeChange = 0;
      if (range >= 2) {
        rangeChange = (int)(t.getHeight() - other.getHeight());
      }
      if (l1Distance(t, other) - rangeChange <= range && l1Distance(t, other) != 0) {
        inRangeTiles.Add(other);
      }
    }
    return inRangeTiles;
  }

  public List<Tile> getCardinalTilesWithinRange(Tile t, int range) {
    List<Tile> inRangeTiles = getTilesWithinRange(t, range);
    inRangeTiles = new List<Tile>(inRangeTiles.Filter((tile) =>
      Math.Abs(tile.gameObject.transform.position.x - t.gameObject.transform.position.x) < 0.05f  ||
      Math.Abs(tile.gameObject.transform.position.z - t.gameObject.transform.position.z) < 0.05f));
    return inRangeTiles;
  }

  public int l1Distance(Tile t1, Tile t2) {
    return (int)Math.Floor(Math.Abs(t1.gameObject.transform.position.x - t2.gameObject.transform.position.x) + 0.5f)  +
          (int)Math.Floor(Math.Abs(t1.gameObject.transform.position.z - t2.gameObject.transform.position.z) + 0.5f) ;
  }

  public void clearColour() {
    foreach (Tile tile in tiles) {
      tile.setColor(Color.clear);
    }
  }

  public void setPath(Vector3 coord) {
    clearPath();
    Tile t = getTile(coord);
    path.AddFirst(t);
    while (t.dir != Vector3.zero) {
      if (t.dir == portalDir) {
        PortalEffect portal = t.getEffect<PortalEffect>();
        Debug.Assert(portal != null, "Went through a missing portal");
        t = portal.sibling.ownerTile;
        coord = t.transform.position;
      } else {
        // normal case
        coord -= t.dir;
        t = getTile(coord);
      }
      path.AddFirst(t);
    }
  }

  public List<Tile> tilesInMoveRange(Character character) {
    List<Tile> ret = new List<Tile>();
    foreach (Tile tile in tiles) {
      if (tile.distance <= character.moveRange && !tile.occupied()) {
        ret.Add(tile);
      }
    }
    return ret;
  }

  // GameMap functions
  public void setTileColours(Tile src = null) {
    GameObject SelectedPiece = GameManager.get.SelectedPiece;
    int SelectedSkill = GameManager.get.SelectedSkill;
    if (src == null) src = getTile(GameManager.get.SelectedPiece.transform.position);
    clearColour();
    if (GameManager.get.gameState == GameState.moving) {
      foreach (Tile tile in tiles) {
        if (tile.distance <= GameManager.get.moveRange && !tile.occupied()) {
          tile.setColor(Color.green);
        } else {
          tile.setColor(Color.clear);
        }
      }
    } else if (GameManager.get.gameState == GameState.attacking && SelectedSkill != -1) {
      bool aoe = (SelectedPiece.GetComponent<Character>().equippedSkills[SelectedSkill] is AoeSkill);
      int range = SelectedPiece.GetComponent<Character>().equippedSkills[SelectedSkill].range;
      List<Tile> inRangeTiles = getTilesWithinRange(getTile(SelectedPiece.transform.position), range);
      if (!aoe) {
        foreach (Tile tile in inRangeTiles) {
          tile.setColor(Color.blue);
        }
        foreach (GameObject o in GameManager.get.skillTargets) {
          getTile(o.transform.position).setColor(Color.red);
        }
      } else {
        foreach (GameObject o in SelectedPiece.GetComponent<Character>().equippedSkills[SelectedSkill].getTargets()) {
          getTile(o.transform.position).setColor(Color.blue);
        }
        AoeSkill skill = SelectedPiece.GetComponent<Character>().equippedSkills[SelectedSkill] as AoeSkill;
        var targetsInAoe = skill.getTargetsInAoe(src.gameObject.transform.position);
        if (targetsInAoe != null) {
          foreach (GameObject o in targetsInAoe) {
            if (o.tag == "Cube") getTile(o.transform.position).setColor(Color.yellow);
            else getTile(o.transform.position).setColor(Color.red);
          }
        }
      }
      // color the selected targets
      foreach (List<Effected> target in GameManager.get.targets) {
        foreach (Effected eff in target) {
          Tile t = eff as Tile;
          if (eff == null) t = (eff as Character).curTile;
          Debug.AssertFormat(eff != null, "Effected is not a character or tile.");
          t.setColor(Color.magenta);
        }
      }
    }
    // color the path
    foreach (Tile ti in path) {
      ti.setColor(Color.blue);
    }
  }

  public void clearPath() {
    path.Clear();
  }
}

