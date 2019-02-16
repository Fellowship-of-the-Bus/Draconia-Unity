using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class CharacterPortraitGenerator : MonoBehaviour {
  public new Camera camera;
  public RenderTexture texture;
  public GameObject target;
  private bool doPhoto = false;

  // OnPostRender: script needs to be attached to an object with a Camera component
  void OnPostRender() {
    if (doPhoto) {
      save(target.name == "" ? "untitled" : target.name + ".png");
      doPhoto = false;
    }
  }

  public void takePhoto(GameObject obj) {
    target = obj;
    // TODO: properly position camera to look at obj
    camera.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z - 3);
    doPhoto = true;
  }

  private void save(string fileName) {
    // read from RenderTexture into Texture2D
    var texture2d = new Texture2D(texture.width, texture.height);
    texture2d.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
    texture2d.Apply();

    // write Texture2D to disk
    var bytes = texture2d.EncodeToPNG();
    var file = File.Open(Application.persistentDataPath + "/" + fileName, FileMode.Create);
    var binary = new BinaryWriter(file);
    binary.Write(bytes);
    file.Close();
  }
}
