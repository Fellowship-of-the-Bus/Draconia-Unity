using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

using System.IO;

using System.Collections.Generic;

[InitializeOnLoad]
static class PositionToAnchorConverter {

  /*
   * height default = 1
   * start default false (! at front of string)
   * should have a tree (t at front of string)
   * ^ For floating block, with next character base block, optional height next !! optional height ends with space.
   * - for unpathable
   * example !D2.5 for start tile dirt height 2.5
   * example D for non-startTile height 1
   * example !tD2.5 for start tile dirt height 2.5 that contains a tree.
   * example ^OD3 for a 1 tile dirt block at height 3 with a water block at base height
   * example ^O2 D3 for a 2 tile dirt block at height 3 with a water block at base height. Note the space
   */
  const float aspectWidth = 1920;
  const float aspectHeight = 1080;
  const float selectorWidth = 30;
  const float selectorHeight = 30;

  static PositionToAnchorConverter() {}

  [MenuItem("Adjust/Anchorize level selectors")]
  private static void convertPositionToAnchors() {
    Transform parentTransform = GameObject.Find("LevelParent").transform;
    RectTransform rect = null;

    float widthOffset = selectorWidth / 2;
    float heightOffset = selectorHeight / 2;
    foreach (Transform childTransform in parentTransform) {
      rect = (RectTransform)childTransform;
      if (rect != null) {
        if (rect.offsetMax.x != widthOffset || rect.offsetMax.y != heightOffset) {
          float anchorX = (rect.offsetMax.x - widthOffset) / 1920;
          float anchorY = (rect.offsetMax.y - heightOffset) / 1080;

          Vector2 newAnchor = new Vector2(anchorX, anchorY);
          rect.anchorMin = newAnchor;
          rect.anchorMax = newAnchor;
          rect.offsetMax = new Vector2(widthOffset, heightOffset);
          rect.offsetMin = new Vector2(-widthOffset, -heightOffset);

          EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
      }
    }
  }
}
