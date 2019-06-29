using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class CharacterPortraitGenerator : MonoBehaviour {
  public new Camera camera;
  public RenderTexture texture;
  private ConcurrentQueue<BattleCharacter> queue = new ConcurrentQueue<BattleCharacter>();
  private BattleCharacter target;

  void Start() {
    camera.enabled = false;
  }

  void Update() {
    if (! queue.TryDequeue(out target)) return;
    Transform cameraLoc = target.portraitCameraPosition;
    transform.SetPositionAndRotation(cameraLoc.position, cameraLoc.rotation);
    camera.Render();
  }

  // photos are taken once per frame if there are characters in the queue
  void OnPostRender() {
    CharacterPortraitManager.savePortrait(target, renderToTex2d(texture));
  }

  public void takePhoto(BattleCharacter target) {
    queue.Enqueue(target);
  }

  private Texture2D renderToTex2d(RenderTexture texture) {
    // read from RenderTexture into Texture2D
    var texture2d = new Texture2D(texture.width, texture.height);
    texture2d.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
    texture2d.Apply();
    return texture2d;
  }
}
