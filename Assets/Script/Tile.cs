using UnityEngine;
using System.Collections.Generic;

public class Tile : Effected {
  private static Color transparent = new Color(0.8f, 0.8f, 0.8f, 0.25f); // TODO: update transparency on change
  public static int unpathableCost = 1000;
  public int distance = 0;
  public int movePointSpent = 1;
  public Vector3 dir = Vector3.zero;
  public BattleCharacter occupant = null;
  public float additionalHeight = 0f;
  public bool startTile = false;
  public string type;
  public Material color;
  public Renderer[] borders = new Renderer[4];
  private bool isClear = false;

  public bool occupied() {
    return occupant != null;
  }

  public float getHeight() {
    return transform.localScale.y/2 + additionalHeight;
  }

  public Vector3 position {
    get {
      Vector3 pos = transform.position;
      pos.y = 0.5f + getHeight();
      return pos;
    }
  }

  public void setup() {
    Transform t = gameObject.transform.Find("Top");
    color = t.gameObject.GetComponent<Renderer>().material;
    borders[0] = this.transform.Find("LeftBorder").gameObject.GetComponent<Renderer>();
    borders[1] = this.transform.Find("RightBorder").gameObject.GetComponent<Renderer>();
    borders[2] = this.transform.Find("BackBorder").gameObject.GetComponent<Renderer>();
    borders[3] = this.transform.Find("FrontBorder").gameObject.GetComponent<Renderer>();
  }

  public bool unpathable() {
    return movePointSpent >= Tile.unpathableCost;
  }

  public void setColor(Color c) {
    isClear = false;
    if (unpathable()) {
      return;
    }

    foreach (Renderer b in borders) {
      b.material.color = c;
    }
  }

  public void clearColour() {
    if (!isClear) {
      setColor(transparent);
    }
    isClear = true;
  }
}
