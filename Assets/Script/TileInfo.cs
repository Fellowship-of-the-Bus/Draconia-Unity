using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TileInfo : MonoBehaviour {
  public Tile tile;
  public Text type;
  public Text height;
  public Text cost;
  public Image background;
  public void setTile(Tile t) {
    if (tile == t) {
      return;
    }
    tile = t;
    //other things.
    type.text = tile.type + "Tile";
    height.text = "Height: " + tile.getHeight();
    cost.text = "MvCost: " + tile.movePointSpent;
    Debug.Log(tile.color);
    Debug.Log(background.material);
    background.material = tile.color;
  }
}
