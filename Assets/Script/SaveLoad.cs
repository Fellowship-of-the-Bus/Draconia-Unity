using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public static class SaveLoad {
  public static string autoSaveName = "AutoSave.autobro";
  public static string dirPath = Path.Combine(Application.persistentDataPath, "savedGames");

  private static DirectoryInfo dir = System.IO.Directory.CreateDirectory(dirPath);

  public static void save(string saveName, bool auto = false) {
    BinaryFormatter bf = new BinaryFormatter();
    if (!saveName.EndsWith(".bro") && !auto) saveName += ".bro";
    FileStream file = File.Create(Path.Combine(dirPath, saveName));
    bf.Serialize(file, GameData.gameData);
    file.Close();
  }

  public static void saveAuto() {
    save(SaveLoad.autoSaveName, true);
  }

  public static bool load(string saveName) {
    bool success = false;
    saveName = Path.Combine(dirPath, saveName);
    if (File.Exists(saveName)) {
      BinaryFormatter bf = new BinaryFormatter();
      FileStream file = File.Open(saveName, FileMode.Open);
      try {
        GameData.gameData = (GameData)bf.Deserialize(file);
        success = true;
      } catch (SerializationException) {
        Debug.Log("Corrupted save file: " + saveName);
      }
      file.Close();
    } else Debug.Log(saveName + " doesn't exist");
    return success;
  }

  public static void loadAuto() {
    load(SaveLoad.autoSaveName);
  }

  public static IEnumerable<FileInfo> listSaveFiles() {
    return dir.GetFiles("*.bro");
  }
}
