using UnityEngine;

[ExecuteAlways]
class MinimapCamera : MonoBehaviour {
  public Camera camera;
  [Tooltip("Multiplier for minimap zoom level. Smaller numbers zoom farther")]
  public float scaleFactor;

  void Start() {
    float dimX = 0, dimZ = 0;
    if (Application.IsPlaying(gameObject)) {
      GameManager.get.map.getDimensions(out dimX, out dimZ);
    } else {
      foreach (GameObject cube in GameObject.FindGameObjectsWithTag("Cube")) {
        if (cube.transform.position.x > dimX) {
          dimX = cube.transform.position.x;
        }
        if (cube.transform.position.z > dimZ) {
          dimZ = cube.transform.position.z;
        }
      }
    }
    // Center the minimap camera based on the map dimensions. Subtract 1 because the center
    // of a tile is its origin.
    float x = (dimX-1) / 2f;
    float z = (dimZ-1) / 2f;
    const float y = 50f;  // arbitrary - as long as it's high enough the actual value doesn't matter
    transform.position = new Vector3(x, y, z);

    // set orthographic size based on the larger dimension so that the entire map is visible in the minimap
    float dimMax = Mathf.Max(dimX, dimZ);
    camera.orthographicSize = dimMax * scaleFactor;
  }
}
