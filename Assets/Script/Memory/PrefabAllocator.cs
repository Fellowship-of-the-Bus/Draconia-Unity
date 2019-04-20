using UnityEngine;

public class PrefabAllocator : MonoBehaviour {
  public GameObject prefab;

  public GameObject alloc() {
    return Instantiate(prefab) as GameObject;
  }
}
