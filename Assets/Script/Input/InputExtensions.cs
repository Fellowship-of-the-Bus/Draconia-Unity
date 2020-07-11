using UnityEngine.InputSystem;
using System;
using System.Collections.Generic;

public static class InputExtensions {
  // convenience
  public static IEnumerable<InputActions> actions(this IEnumerable<PlayerInput> inputs) {
    foreach (PlayerInput input in inputs) {
      yield return (InputActions)input.user.actions;
    }
  }
}
