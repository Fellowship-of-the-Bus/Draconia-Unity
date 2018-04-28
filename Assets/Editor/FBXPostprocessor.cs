using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.Collections.Generic;

class FBXPostprocessor : AssetPostprocessor {

  const float y_val = -0.78f;

  List<string> models = new List<string>(new string[] {"Human", "Lizard"});

  void getAllChildren(GameObject g) {
    for(int i = 0; i < g.transform.childCount; i++) {
      Transform t = g.transform.GetChild(i);
      Debug.Log(t.gameObject);
      getAllChildren(t.gameObject);
    }
  }

  void OnPostprocessModel(GameObject g) {
    if (models.Contains(g.name)) {
      Animator animator = g.GetComponent<Animator>();

      g.transform.position =  new Vector3(0,y_val,0);

      // or get animation Clip
      Object[] objects = AssetDatabase.LoadAllAssetsAtPath(assetPath);
      foreach (Object obj in objects) {
        AnimationClip clip = obj as AnimationClip;
        if (clip != null) {
          if (clip.name.Contains("__preview__")) {
            continue;
          }
          AnimationClip copy = AnimationClip.Instantiate(clip);
          copy.name = copy.name.Replace("|","").Replace("(Clone)","").Replace("Armature","");
          AssetDatabase.CreateAsset(copy, "Assets/Models/Resources/Animations/" + copy.name + ".anim");
        }
      }

      AnimatorController controller = Resources.Load("Human", typeof(AnimatorController)) as AnimatorController;
      AnimatorControllerLayer layer = controller.layers[0]; //Our controller only has one layer
      AnimatorStateMachine machine = layer.stateMachine;
      foreach(ChildAnimatorState cstate in machine.states) {
        AnimatorState state = cstate.state;
        state.motion = Resources.Load("Animations/" + state.name, typeof(AnimationClip)) as AnimationClip;
      }

      animator.runtimeAnimatorController = controller;
    }
  }

}
