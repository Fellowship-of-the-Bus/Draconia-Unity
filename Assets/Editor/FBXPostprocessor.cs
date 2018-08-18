using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.Collections.Generic;

class FBXPostprocessor : AssetPostprocessor {

  const float y_val = -0.78f;

  List<string> models = new List<string>(new string[] {"Human", "Lizard"});
  List<string> toResize = new List<string>(new string[] {"Sword", "Bow", "Hammer", "jumonji", "Staff", "yari"});
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

    if (toResize.Contains(g.name)) {
      g.transform.localScale = new Vector3(1,1,1);
    }


    if (models.Contains(g.name)) {
      Transform armature = g.transform.findRecursive("Armature");
      armature.position = new Vector3(0,0,0);
      Animator animator = g.GetComponent<Animator>();

      g.transform.position =  new Vector3(0,y_val,0);

      List<AnimationClip> animations = new List<AnimationClip>();

      // or get animation Clip
      Object[] objects = AssetDatabase.LoadAllAssetsAtPath(assetPath);
      if (!AssetDatabase.IsValidFolder("Assets/Models")) AssetDatabase.CreateFolder("Assets", "Models");
      if (!AssetDatabase.IsValidFolder("Assets/Models/Resources")) AssetDatabase.CreateFolder("Assets/Models", "Resources");
      if (!AssetDatabase.IsValidFolder("Assets/Models/Resources/Animations")) AssetDatabase.CreateFolder("Assets/Models/Resources", "Animations");
      foreach (Object obj in objects) {
        AnimationClip clip = obj as AnimationClip;
        if (clip != null) {
          if (clip.name.Contains("__preview__")) {
            continue;
          }
          AnimationClip copy = AnimationClip.Instantiate(clip);
          copy.name = copy.name.Replace("|","").Replace("(Clone)","").Replace("Armature","");
          animations.Add(copy);
          AssetDatabase.CreateAsset(copy, "Assets/Models/Resources/Animations/" + copy.name + ".anim");
        }
      }
      /*
      AnimatorController controller = Resources.Load("Human", typeof(AnimatorController)) as AnimatorController;
      AnimatorControllerLayer layer = controller.layers[0]; //Our controller only has one layer
      AnimatorStateMachine machine = layer.stateMachine;
      foreach(ChildAnimatorState cstate in machine.states) {
        AnimatorState state = cstate.state;
        if (state.motion is BlendTree) {
          BlendTree bt = state.motion as BlendTree;
          while (bt.children.Length > 0) bt.RemoveChild(0);
          foreach(string s in state.name.Split('_')) {
            AnimationClip btmotion = Resources.Load("Animations/" + s, typeof(AnimationClip)) as AnimationClip;
            if (btmotion) bt.AddChild(btmotion);
          }
          continue;
        }
        AnimationClip motion = Resources.Load("Animations/" + state.name, typeof(AnimationClip)) as AnimationClip;
        if (motion) state.motion = motion;
      }
      */

      AnimatorController controller = AnimatorController.CreateAnimatorControllerAtPath("Assets/Models/Resources/Animations/" + g.name + "Controller.controller");

      controller.AddParameter("isWalking", AnimatorControllerParameterType.Bool);
      controller.AddParameter("isNinja", AnimatorControllerParameterType.Bool);
      controller.AddParameter("Blend", AnimatorControllerParameterType.Float);

      var machine = controller.layers[0].stateMachine;
      var idleState = machine.AddState("Idle");
      machine.defaultState = idleState;

      foreach(AnimationClip anim in animations) {
        if (anim.name == "Idle") {
          idleState.motion = anim;
        } else if (anim.name == "Move") {
          var state = machine.AddState(anim.name);
          state.motion = anim;
          var transition = state.AddTransition(idleState);
          transition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.IfNot, 0, "isWalking");
          transition = idleState.AddTransition(state);
          transition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "isWalking");
        }

         else {
          controller.AddParameter(anim.name, AnimatorControllerParameterType.Trigger);
          var state = machine.AddState(anim.name);
          state.motion = anim;
          state.AddTransition(idleState,true);
          var transition = idleState.AddTransition(state);
          transition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, anim.name);
        }
      }

      animator.runtimeAnimatorController = controller;
    }
  }

}
