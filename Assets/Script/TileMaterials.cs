using UnityEngine;

public class TileMaterials : MonoBehaviour {
  public Material Blue;
  public Material Green;
  public Material Magenta;
  public Material Orange;
  public Material Red;
  public Material Transparent;
  public Material White;
  public Material Yellow;

  void Awake() {
    get = this;
  }

  void Start() {
    updateTransparency();
  }

  public static void updateTransparency() {
    if (get) {
      Color color = get.Transparent.color;
      get.Transparent.color = new Color(color.r, color.g, color.b, Options.gridTransparency);
    }
  }

  public static TileMaterials get { get; private set; }
}
