﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class Map {
  public static Vector3 portalDir = Vector3.one;

  public LinkedList<Tile> path = new LinkedList<Tile>();
  public List<Tile> tiles = new List<Tile>();
  List<GameObject> cubes;

  public List<Tile> startPositions = new List<Tile>();
  Tile[,] map = null;

  public static readonly Color orange = new Color(1, 0.5f, 0, 1);

  public void awake() {
    float xMax = 0f;
    float zMax = 0f;

    cubes = new List<GameObject>(GameObject.FindGameObjectsWithTag("Cube"));
    foreach (GameObject cube in cubes) {
      Tile t = cube.GetComponent<Tile>();
      tiles.Add(t);
      t.setup();

      if (t.transform.position.x > xMax) {
        xMax = t.transform.position.x;
      }
      if (t.transform.position.z > zMax) {
        zMax = t.transform.position.z;
      }
    }

    map = new Tile[Mathf.RoundToInt(xMax) + 1, Mathf.RoundToInt(zMax) + 1];
    foreach (GameObject cube in cubes) {
      Tile t = cube.GetComponent<Tile>();
      Vector3 p = t.transform.position;
      int x = Mathf.RoundToInt(p.x);
      int z = Mathf.RoundToInt(p.z);
      // in the case where multiple tiles occupy the same (x, z) position, choose the top one
      if (map[x,z] == null || p.y > map[x,z].transform.position.y) {
        map[x,z] = t;
      }
    }
  }

  public void djikstra(Vector3 unitLocation, BattleCharacter charToMove) {
    foreach (Tile tile in tiles) {
      tile.distance = System.Int32.MaxValue/2;
      tile.dir = Vector3.zero;
    }

    float moveTolerance = charToMove.moveTolerance;
    HashSet<Tile> tilesToGo = new HashSet<Tile>(tiles);

    Tile startTile = getTile(unitLocation);
    startTile.distance = 0;

    PortalEffect portal = startTile.getEffect<PortalEffect>();
    if (portal != null) {
      Tile sibling = portal.sibling.ownerTile;
      sibling.distance = 0;
      sibling.dir = portalDir;
    }

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
        Vector3 neighbour = minTile.transform.position + dir;
        Tile neighbourTile = getTile(neighbour, tilesToGo);
        if (neighbourTile != null) {
          int d = minTile.distance + distance(minTile, neighbourTile, moveTolerance);
          if (d < neighbourTile.distance) {
            neighbourTile.distance = d;
            neighbourTile.dir = dir;
          }
          // update portal dest distance to portal src distance
          portal = neighbourTile.getEffect<PortalEffect>();
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

  private int distance(Tile from, Tile to, float moveTolerance) {
    //check heights
    if (Math.Abs(getHeight(from) - getHeight(to)) > moveTolerance) {
      return Int32.MaxValue/2;
    }
    if (from.occupied() && !(GameManager.get.SelectedCharacter.team == from.occupant.team)) {
      return Int32.MaxValue/4;
    }
    //Look at what moveTolerance actually means
    return Math.Max((int)(to.movePointSpent - moveTolerance + 1), 1);
  }

  public float getHeight(Tile t) {
    float scale = t.getHeight();
    return scale + 0.5f;
  }


  public Tile getTile(Vector3 location) {
    int x = Mathf.RoundToInt(location.x);
    int z = Mathf.RoundToInt(location.z);

    return getTile(x, z);
  }

  public Tile getTile(int x, int z) {
    if (0 <= x && x <= map.GetUpperBound(0) && 0 <= z && z <= map.GetUpperBound(1)) {
      return map[x, z];
    }
    return null;
  }

  // for internal use by djikstra only
  private Tile getTile(Vector3 location, ISet<Tile> set) {
    Tile target = getTile(location);
    return set.Contains(target) ? target : null;
  }

  List<Tile> getAdjacentTiles(Tile t) {
    List<Tile> adjacentTiles = new List<Tile>();

    int x = Mathf.RoundToInt(t.position.x);
    int z = Mathf.RoundToInt(t.position.z);

    Tile adjacent = getTile(x - 1, z);
    if (adjacent != null) {
      adjacentTiles.Add(adjacent);
    }

    adjacent = getTile(x + 1, z);
    if (adjacent != null) {
      adjacentTiles.Add(adjacent);
    }

    adjacent = getTile(x, z - 1);
    if (adjacent != null) {
      adjacentTiles.Add(adjacent);
    }

    adjacent = getTile(x, z + 1);
    if (adjacent != null) {
      adjacentTiles.Add(adjacent);
    }

    return adjacentTiles;
  }

  // ignorePathing - if this is true, the range will simply extend out ignoring whether terrain is pathable
  // with the exception that it will still be blocked by "Wall" tiles.
  private List<Tile> getTilesWithinRange(Tile t, int range, bool ignorePathing, bool melee) {
    ISet<Tile> inRangeTiles = new HashSet<Tile>();
    ISet<Tile> edgeTiles = new HashSet<Tile>();
    ISet<Tile> outerTiles = new HashSet<Tile>();
    inRangeTiles.Add(t);
    edgeTiles.Add(t);

    for (int dist = 1; dist < range + 1; dist++) {
      foreach (Tile e in edgeTiles) {
        foreach (Tile a in getAdjacentTiles(e)) {
          if (!inRangeTiles.Contains(a) && ((!outerTiles.Contains(a) && a.movePointSpent < 10) || (ignorePathing && !a.isWall))
                                        && (!melee || dist + (int)Mathf.Ceil(Math.Abs(a.getHeight() - t.getHeight()))  <= range + 1)) {
            outerTiles.Add(a);
          }
        }
      }

      inRangeTiles.UnionWith(outerTiles);
      edgeTiles = outerTiles;
      outerTiles = new HashSet<Tile>();
    }

    inRangeTiles.Remove(t);
    return new List<Tile>(inRangeTiles);
  }

  public List<Tile> getTilesWithinRange(Tile t, int range, bool melee) {
    return getTilesWithinRange(t, range, false, melee);

  }

  // Get all tiles within a fixed distance of the given tile, ignoring height differences and pathability
  public List<Tile> getTilesWithinDistance(Tile t, int range, bool melee) {
    return getTilesWithinRange(t, range, true, melee);
  }

  public List<Tile> getTilesWithinMovementRange(int mvRange) {
    List<Tile> inRangeTiles = new List<Tile>();
    foreach (Tile other in tiles) {
      if (other.distance <= mvRange) {
        inRangeTiles.Add(other);
      }
    }
    return inRangeTiles;
  }

  public List<Tile> getCardinalTilesWithinRange(Tile t, int range) {
    List<Tile> inRangeTiles = getTilesWithinRange(t, range, false);
    inRangeTiles = new List<Tile>(inRangeTiles.Filter((tile) =>
      Math.Abs(tile.transform.position.x - t.transform.position.x) < 0.05f  ||
      Math.Abs(tile.transform.position.z - t.transform.position.z) < 0.05f));
    return inRangeTiles;
  }

  // Manhattan distance
  public int l1Distance(Tile t1, Tile t2) {
    return (int)Math.Floor(Math.Abs(t1.transform.position.x - t2.transform.position.x) + 0.5f)  +
          (int)Math.Floor(Math.Abs(t1.transform.position.z - t2.transform.position.z) + 0.5f) ;
  }

  public void clearColour() {
    foreach (Tile tile in tiles) {
      tile.clearColour();
    }
  }

  public void setPath(Vector3 coord) {
    path = getPath(coord);
  }

  public LinkedList<Tile> getPath(Vector3 coord) {
    LinkedList<Tile> newPath = new LinkedList<Tile>();
    Tile t = getTile(coord);
    newPath.AddFirst(t);
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
      newPath.AddFirst(t);
    }

    return newPath;
  }

  public List<Tile> tilesInMoveRange(BattleCharacter character) {
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
    BattleCharacter SelectedCharacter = GameManager.get.SelectedCharacter;
    int SelectedSkill = GameManager.get.SelectedSkill;
    if (src == null) src = getTile(SelectedCharacter.transform.position);
    clearColour();

    if (GameManager.get.previewTarget != null) {
      GameManager.get.previewTarget.curTile.setColor(TileMaterials.get.Yellow);
    }

    if (GameManager.get.gameState == GameState.moving) {
      foreach (Tile tile in tiles) {
        if (tile.distance <= GameManager.get.moveRange && !tile.occupied()) {
          tile.setColor(TileMaterials.get.Green);
        }
      }
      // color the path
      foreach (Tile ti in path) {
        ti.setColor(TileMaterials.get.Blue);
      }
    } else if (GameManager.get.gameState == GameState.attacking && SelectedSkill != -1) {
      ActiveSkill skill = SelectedCharacter.equippedSkills[SelectedSkill];
      bool aoe = (skill is AoeSkill);
      int range = skill.range;
      List<Tile> inRangeTiles = getTilesWithinRange(getTile(SelectedCharacter.gameObject.transform.position), range, skill.isMeleeRequired());
      if (!aoe) {
        foreach (Tile t in inRangeTiles) {
          t.setColor(TileMaterials.get.White);
        }
        foreach (Tile t in GameManager.get.skillTargets) {
          t.setColor(TileMaterials.get.Red);
        }
      } else {
        var skillTargets = skill.getTargets();
        foreach (Tile t in skillTargets) {
          t.setColor(TileMaterials.get.White);
        }
        AoeSkill areaSkill = skill as AoeSkill;
        var targetsInAoe = areaSkill.getTargetsInAoe(src.transform.position);
        if (areaSkill is Sprint) {
          foreach (Tile t in path) {
            t.setColor(TileMaterials.get.Blue);
          }
        } else if (targetsInAoe != null) {
          foreach (Tile t in targetsInAoe) {
            t.setColor(TileMaterials.get.Yellow);
            if (t.occupied() && SelectedCharacter.equippedSkills[SelectedSkill].canTarget(t)) {
              t.setColor(TileMaterials.get.Red);
            } else {
              foreach (Tile tile in skillTargets) {
                if (tile == t) {
                  t.setColor(TileMaterials.get.Orange);
                }
              }
            }
          }
        }
      }
      // color the selected targets
      foreach (Tile target in GameManager.get.targets) {
        target.setColor(TileMaterials.get.Magenta);
      }
    }
  }

  public List<Tile> getStartTiles() {
    return new List<Tile>(tiles.Filter((t) => t.startTile));
  }

  public void clearPath() {
    path.Clear();
  }

  public void getDimensions(out float x, out float z) {
    x = map.GetLength(0);
    z = map.GetLength(1);
  }
}

