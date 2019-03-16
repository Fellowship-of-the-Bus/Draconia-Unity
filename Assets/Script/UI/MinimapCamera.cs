using UnityEngine;

class MinimapCamera : MonoBehaviour {
  void Start() {
    int dimX, dimZ;
    GameManager.get.map.getDimensions(out dimX, out dimZ);
    float x = dimX / 2f;
    float z = dimZ / 2f;
    const float y = 50f;  // arbitrary - as long as it's high enough the actual value doesn't matter
    transform.position = new Vector3(x, y, z);

    // TODO: choose camera size based on how large map is
  }
}
