using UnityEngine;
using System.Collections.Generic;

public class Tile : Effected {
  public int distance = 0;
  public int movePointSpent = 1;
  public Vector3 dir = Vector3.zero;
  public BattleCharacter occupant = null;
  public float additionalHeight = 0f;
  public bool startTile = false;

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

  public void setColor(Color c) {
    Renderer r = this.gameObject.GetComponent<Renderer>();
    if (r == null) {
      // Only this part is needed for multitexture tiles
      this.transform.Find("LeftBorder").gameObject.GetComponent<Renderer>().material.color = c;
      this.transform.Find("RightBorder").gameObject.GetComponent<Renderer>().material.color = c;
      this.transform.Find("BackBorder").gameObject.GetComponent<Renderer>().material.color = c;
      this.transform.Find("FrontBorder").gameObject.GetComponent<Renderer>().material.color = c;
    } else {
      r.material.color = c == Color.clear ? Color.white : c;
    }
  }
}
