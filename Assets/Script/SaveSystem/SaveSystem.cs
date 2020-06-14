using UnityEngine;

// Allow SaveLoad to interface with Unity's update loop
public class SaveSystem : MonoBehaviour {
  public static SaveSystem get;

  void Awake() {
    if (!Singleton.makeSingleton(ref get, this)) return;
  }

  void Update() {
    SaveLoad.run();
  }
}
