using UnityEngine;
using System.Collections;

public class Destroy : MonoBehaviour {
  public GameObject objectToDestroy;

  void die() {
    objectToDestroy.SetActive(false);
  }
}
