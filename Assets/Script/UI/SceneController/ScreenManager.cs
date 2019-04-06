using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

// From: https://docs.unity3d.com/Manual/HOWTO-UIScreenTransition.html

public class ScreenManager : MonoBehaviour {
  // Screen to open automatically at the start of the Scene
  public Animator initiallyOpen;

  // Currently Open Screen
  private Animator Open;

  // Hash of the parameter we use to control the transitions.
  private int OpenParameterId;

  // The GameObject Selected before we opened the current Screen.
  // Used when closing a Screen, so we can go back to the button that opened it.
  private GameObject PreviouslySelected;

  // Animator State and Transition names we need to check against.
  const string OpenTransitionName = "Open";

  // Menu containing elements common to all screens
  public GameObject genericMenu;

  public void OnEnable() {
    // We cache the Hash to the "Open" Parameter, so we can feed to Animator.SetBool.
    OpenParameterId = Animator.StringToHash (OpenTransitionName);

    // If set, open the initial Screen now.
    if (initiallyOpen == null)
      return;
    OpenPanel(initiallyOpen);
  }

  // Closes the currently open panel and opens the provided one.
  // It also takes care of handling the navigation, setting the new Selected element.
  public void OpenPanel (Animator anim) {
    if (Open == anim)
      return;

    // Activate the new Screen hierarchy so we can animate it.
    anim.gameObject.SetActive(true);
    // Save the currently selected button that was used to open this Screen. (CloseCurrent will modify it)
    var newPreviouslySelected = EventSystem.current.currentSelectedGameObject;
    // Move the Screen to front.
    anim.transform.SetAsLastSibling();

    CloseCurrent();

    // Activate the back button
    genericMenu.SetActive(true);

    PreviouslySelected = newPreviouslySelected;

    // Set the new Screen as then open one.
    Open = anim;
    // Start the open animation
    Open.SetBool(OpenParameterId, true);

    // Set an element in the new screen as the new Selected one.
    GameObject go = FindFirstEnabledSelectable(anim.gameObject);
    SetSelected(go);
  }

  // Finds the first Selectable element in the providade hierarchy.
  static GameObject FindFirstEnabledSelectable (GameObject gameObject) {
    var selectables = gameObject.GetComponentsInChildren<Selectable> (true);
    foreach (var selectable in selectables) {
      if (selectable.IsActive () && selectable.IsInteractable ()) {
        return selectable.gameObject;
      }
    }
    return null;
  }

  // Closes the currently open Screen
  // It also takes care of navigation.
  // Reverting selection to the Selectable used before opening the current screen.
  public void CloseCurrent() {
    if (Open == null)
      return;

    // Start the close animation.
    Open.SetBool(OpenParameterId, false);

    // Reverting selection to the Selectable used before opening the current screen.
    SetSelected(PreviouslySelected);
    // No screen open.
    Open = null;
    // TODO: this needs to happen at the end of the animation, probably in MenuClosedStateController
    genericMenu.SetActive(false);
  }


  // Make the provided GameObject selected
  // When using the mouse/touch we actually want to set it as the previously selected and
  // set nothing as selected for now.
  private void SetSelected(GameObject go) {
    // Select the GameObject.
    EventSystem.current.SetSelectedGameObject(go);

    // If we are using the keyboard right now, that's all we need to do.
    var standaloneInputModule = EventSystem.current.currentInputModule as StandaloneInputModule;
    if (standaloneInputModule != null)
      return;

    // Since we are using a pointer device, we don't want anything selected.
    // But if the user switches to the keyboard, we want to start the navigation from the provided game object.
    // So here we set the current Selected to null, so the provided gameObject becomes the Last Selected in the EventSystem.
    EventSystem.current.SetSelectedGameObject(null);
  }
}
