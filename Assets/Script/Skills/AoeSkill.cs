using UnityEngine;
using System.Collections.Generic;

public interface AoeSkill {
  int aoe {get; set;}
  bool effectsTiles {get; set;}
  List<GameObject> getTargetsInAoe(Vector3 position);
}
