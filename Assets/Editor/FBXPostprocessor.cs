using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.Collections.Generic;

class FBXPostprocessor : AssetPostprocessor {

  const float y_val = -0.78f;

  List<string> models = new List<string>(new string[] {"Human", "Lizard", "Snake"});
  List<string> unwanted = new List<string>(new string[] {"Camera", "Lamp"});

  List<GameObject> getUnwantedParts(GameObject g) {
    List<GameObject> ret = new List<GameObject>();
    for(int i = 0; i < g.transform.childCount; i++) {
      Transform t = g.transform.GetChild(i);
      ret.AddRange(getUnwantedParts(t.gameObject));
    }

    if (unwanted.Contains(g.name)) {
      ret.Add(g);
    }
    return ret;
  }

  void OnPostprocessModel(GameObject g) {
    foreach (GameObject go in getUnwantedParts(g)) {
      Object.DestroyImmediate(go);
    }

    g.transform.localScale = new Vector3(1,1,1);
    g.transform.localEulerAngles = new Vector3(0,0,0);


    if (models.Contains(g.name)) {
      AnimatorController controller = Resources.Load("Animations/Controller", typeof(AnimatorController)) as AnimatorController;
      Animator animator = g.GetComponent<Animator>();
      animator.runtimeAnimatorController = controller;
      g.transform.position =  new Vector3(0,y_val,0);
      if (g.name != "Human") {
        g.transform.findRecursive("Armature_L_Lib.001").gameObject.name = "Armature";
      }
    }
  }
}
