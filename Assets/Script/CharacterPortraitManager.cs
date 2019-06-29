using UnityEngine;
using System.IO;

public class CharacterPortraitManager : MonoBehaviour {
  public static CharacterPortraitManager get;
  public CharacterPortraitGenerator generator;

  void Awake() {
    if (get != null) {
      Destroy(gameObject);
      return;
    }
    get = this;
    DontDestroyOnLoad(gameObject);
  }

  public static string getCanonicalName(BattleCharacter character) {
    if (character.name == "") {
      return "no-name";
    }
    return character.name;
  }

  private static string getPath(BattleCharacter character) {
    // use Application.temporaryCachePath?
    return Application.persistentDataPath + "/" + getCanonicalName(character) + ".png";
  }

  private static Texture2D readTexture(string path) {
    byte[] bytes = File.ReadAllBytes(path);
    Texture2D tex = new Texture2D(0, 0);
    tex.LoadImage(bytes);
    return tex;
  }

  private static void writeTexture(string path, Texture2D tex) {
    // write Texture2D to disk
    var bytes = tex.EncodeToPNG();
    var file = File.Open(path, FileMode.Create);
    var binary = new BinaryWriter(file);
    binary.Write(bytes);
    file.Close();
  }

  public static void savePortrait(BattleCharacter character, Texture2D tex) {
    writeTexture(getPath(character), tex);
  }

  public static Sprite getPortrait(BattleCharacter character) {
    string path = getPath(character);
    Texture2D tex = readTexture(path);
    return Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
  }
}
