using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class CharacterPortraitGenerator : MonoBehaviour {
  public new Camera camera;
  public RenderTexture texture;
  public BattleCharacter target;
  private bool doPhoto = false;

  // OnPostRender: script needs to be attached to an object with a Camera component
  void OnPostRender() {
    if (doPhoto) {
      CharacterPortraitManager.savePortrait(target, renderToTex2d(texture));
      doPhoto = false;
    }
  }

  public void takePhoto(BattleCharacter obj) {
    target = obj;
    // TODO: properly position camera to look at obj
    camera.transform.position = obj.portraitCameraPosition;
    doPhoto = true;
  }

  private Texture2D renderToTex2d(RenderTexture texture) {
    // read from RenderTexture into Texture2D
    var texture2d = new Texture2D(texture.width, texture.height);
    texture2d.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
    texture2d.Apply();
    return texture2d;
  }
}
