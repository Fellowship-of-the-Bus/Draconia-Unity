using UnityEngine;
using System.Collections.Generic;

public interface AoeSkill {
  int aoe {get; set;}
  List<GameObject> getTargetsInAoe(Vector3 position);
}
