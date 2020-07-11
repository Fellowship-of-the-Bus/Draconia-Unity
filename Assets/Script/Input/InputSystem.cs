using System;
using UnityEngine.InputSystem;

// Idea: make this a monobehavior again
// then hook the InputActionAsset from InputActions into PlayerInput and the UI input system
// both just need an action asset, so it might be okay if they all share an asset.
// One thing that scares me about this is the docs mentioning that players will make a unique copy
// of the asset, which might mean these assets will get out of sync with each other in some situations...
// -- from a brief glance at the implementation, I think this is just saying that it will check that no
// other PlayerInput has the same actions, so as long as each has a unique InputActions as well then it
// should be fine

/// this game will not have split screen multiplayer, maybe just don't use PlayerInput at all.
// What does it really provide to this game? The whole input system is kind of poorly thought out, I think
// maybe by the time we make a game with unity with multiple players it will be better

// another alternative is to have a wrapper layer that gets call throughs for any actions and gameplay
// code hooks into that rather than the playerinput
// for example, have InputSystem implement all of the interfaces in InputActions and hook PlayerInput
// into those.
// this would require manual work to set up though

public static class InputSystem {
  public delegate InputAction Selector(InputActions actions);

  public static void AddCallback(Selector select, Action<InputAction.CallbackContext> callback) {
    foreach (PlayerInput input in PlayerInput.all) {
      UnityEngine.Debug.Log(input.user.actions);
      InputAction act = select((InputActions)input.user.actions);
      act.performed += callback;
    }
  }

  public static void RemoveCallback(Selector select, Action<InputAction.CallbackContext> callback) {
    foreach (PlayerInput input in PlayerInput.all) {
      InputAction act = select((InputActions)input.user.actions);
      act.performed -= callback;
    }
  }

  public static void Enable() {
    foreach (PlayerInput input in PlayerInput.all) {
      input.enabled = true;
    }
  }

  public static void Disable() {
    foreach (PlayerInput input in PlayerInput.all) {
      input.enabled = false;
    }
  }
}
