using UnityEngine;

public class Singleton {
  public static bool makeSingleton<T>(ref T globalInstance, T newInstance) where T : MonoBehaviour {
    // can't call DontDestroyOnLoad outside of playmode
    if (!Application.IsPlaying(newInstance.gameObject)) return false;
    if (globalInstance != null) {
      GameObject.Destroy(newInstance.gameObject);
      return false;
    }
    globalInstance = newInstance;
    GameObject.DontDestroyOnLoad(newInstance.gameObject);
    return true;
  }
}
