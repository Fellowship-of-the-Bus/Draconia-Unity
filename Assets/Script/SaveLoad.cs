using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public static class SaveLoad {
  public static string saveName = Path.Combine(Application.persistentDataPath, "save.bro");

  public static void save() {
    BinaryFormatter bf = new BinaryFormatter();
    FileStream file = File.Create(saveName);
    bf.Serialize(file, GameManager.get);
    file.Close();
  }

  public static void load() {
    if (File.Exists(saveName)) {
      BinaryFormatter bf = new BinaryFormatter();
      FileStream file = File.Open(saveName, FileMode.Open);
      GameManager.get = (GameManager)bf.Deserialize(file);
      file.Close();
    }
  }
}
