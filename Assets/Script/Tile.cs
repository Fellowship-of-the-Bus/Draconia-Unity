using UnityEngine;
using System.Collections.Generic;

public class Tile : Effected {
  public static int unpathableCost = 1000;
  public int distance = 0; // Distance from the turn character
  public int movePointSpent = 1;
  public Vector3 dir = Vector3.zero;
  public BattleCharacter occupant = null;
  public float additionalHeight = 0f;
  public bool startTile = false;
  public string type;
  public Material color;
  private Renderer border;
  private bool isClear = false;
  public bool isWall = false;

  new void Awake() {
    base.Awake();
  }

  public bool occupied() {
    return occupant != null;
  }

  public float getHeight() {
    return transform.parent.localScale.y + additionalHeight;
  }

  public Vector3 position {
    get {
      Vector3 pos = transform.position;
      pos.y = 0.5f + getHeight();
      return pos;
    }
  }

  public void setup() {
    color = GetComponent<Renderer>().sharedMaterial;
    border = this.transform.Find("Border").gameObject.GetComponent<Renderer>();
    this.clearColour();
  }

  public bool unpathable() {
    return movePointSpent >= Tile.unpathableCost;
  }

  public void setColor(Material material) {
    isClear = false;
    if (unpathable()) {
      return;
    }
    border.material = material;
  }

  public void clearColour() {
    if (!isClear) {
      setColor(TileMaterials.get.Transparent);
    }
    isClear = true;
  }
}
