using UnityEngine;

public static class UnityExtensions {
  public static Transform clear(this Transform transform) {
    foreach (Transform child in transform) {
      GameObject.Destroy(child.gameObject);
    }
    return transform;
  }

  private static Transform findRecursiveHelper(this Transform t, string child, bool onlyEnabled) {
    if (onlyEnabled && !(t.gameObject.activeSelf)) return null;
    if (t.name.StartsWith(child)) return t;
    foreach(Transform c in t) {
      Transform ret = findRecursiveHelper(c, child, onlyEnabled);
      if (ret) return ret;
    }
    return null;
  }

  public static Transform findRecursive(this Transform t, string child) {
    Transform ret = t.findRecursiveHelper(child, true);
    if (ret) return ret;
    return t.findRecursiveHelper(child, false);
  }

  public static GameObject InstantiateKeepScale(this GameObject original, Transform parent) {
    GameObject go = GameObject.Instantiate(original, parent, true);
    go.transform.position = parent.position;
    go.transform.localRotation = parent.rotation;
    return go;
  }
}
