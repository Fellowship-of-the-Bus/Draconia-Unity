using UnityEngine;

[System.Serializable]
public class PrefabAllocator {
  public GameObject prefab;
  public int size;
  public bool grow;

  public GameObject alloc() {
    return GameObject.Instantiate(prefab) as GameObject;
  }
}
