using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.Collections.Generic;

class FBXPostprocessor : AssetPostprocessor {

  const float y_val = -0.78f;

  List<string> models = new List<string>(new string[] {"Human", "Lizard", "Snake"});
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
      /*
      if (g.name != "Human") {
        AnimatorController controller = Resources.Load("Animations/Controller", typeof(AnimatorController)) as AnimatorController;
        Animator animator = g.GetComponent<Animator>();
        g.transform.findRecursive("Armature_L_Lib.001").gameObject.name = "Armature";
        animator.runtimeAnimatorController = controller;
      } else {
        Transform armature = g.transform.findRecursive("Armature");
  //      if (armature == null) g.transform.findRecursive("Armature_L_Lib.001");
        armature.position = new Vector3(0,0,0);
        Animator animator = g.GetComponent<Animator>();


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


        AnimatorController controller = AnimatorController.CreateAnimatorControllerAtPath("Assets/Models/Resources/Animations/" + "Controller.controller");

        controller.AddParameter("isWalking", AnimatorControllerParameterType.Bool);
        controller.AddParameter("isNinja", AnimatorControllerParameterType.Bool);
        controller.AddParameter("Blend", AnimatorControllerParameterType.Float);

        var machine = controller.layers[0].stateMachine;
        var idleState = machine.AddState("Idle");
        machine.defaultState = idleState;

        BlendTree bt;
        var btstate = controller.CreateBlendTreeInController("Idle_NinjaMove", out bt);
        foreach(AnimationClip anim in animations) {
          if (anim.name == "Idle") {
            idleState.motion = anim;
            bt.AddChild(anim,0);
          } else if (anim.name == "Move") {
            var state = machine.AddState(anim.name);
            state.motion = anim;
            var transition = state.AddTransition(idleState);
            transition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.IfNot, 0, "isWalking");
            transition = idleState.AddTransition(state);
            transition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "isWalking");
          } else if (anim.name == "NinjaMove") {
            bt.AddChild(anim,1);
            var bttransition = btstate.AddTransition(idleState);
            bttransition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.IfNot, 0, "isWalking");
            bttransition = btstate.AddTransition(btstate);
            bttransition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "isWalking");
            bttransition = idleState.AddTransition(btstate);
            bttransition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "isWalking");
            bttransition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "isNinja");
            bt.blendParameter = "Blend";
          } else {
            controller.AddParameter(anim.name, AnimatorControllerParameterType.Trigger);
            var state = machine.AddState(anim.name.Replace(".",""));
            state.motion = anim;
            state.AddTransition(idleState,true);
            var transition = idleState.AddTransition(state);
            transition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, anim.name);
          }
        }
        idleState.AddTransition(idleState,true);

        animator.runtimeAnimatorController = controller;
      }
      */
    }

  }

}
