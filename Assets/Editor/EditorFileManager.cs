using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

public interface Serializer {
  void Serialize<T>(string filePath, T src);
  T Deserialize<T>(string filePath);
}

public class BinarySerializer : Serializer {
  public void Serialize<T>(string filePath, T src) {
    using (FileStream file = File.Create(filePath)) {
      BinaryFormatter bf = new BinaryFormatter();
      bf.Serialize(file, src);
    }
  }

  public T Deserialize<T>(string filePath) {
    BinaryFormatter bf = new BinaryFormatter();
    using (FileStream file = File.Open(filePath, FileMode.Open)) {
      return (T)bf.Deserialize(file);
    }
  }
}

public class JsonSerializer : Serializer {
  public void Serialize<T>(string filePath, T src) {
    using (FileStream file = File.Create(filePath))
    using (StreamWriter writer = new StreamWriter(file)) {
      writer.Write(JsonUtility.ToJson(src));
    }
  }

  public T Deserialize<T>(string filePath) {
    return JsonUtility.FromJson<T>(File.ReadAllText(filePath));
  }
}

// handles tracking path of currently open file and saving/loading the file
public class EditorFileManager {
  public Serializer serializer;

  // data for file panel
  public string activeFilePath;
  public string defaultPath;
  public string saveTitle;
  public string loadTitle;
  public string extension;

  public EditorFileManager(Serializer serializer, string path, string saveTitle, string loadTitle, string extension) {
    this.serializer = serializer;
    this.activeFilePath = null;
    this.defaultPath = path;
    this.saveTitle = saveTitle;
    this.loadTitle = loadTitle;
    this.extension = extension;
    System.IO.Directory.CreateDirectory(path);
  }

  public bool Load<T>(ref T dest) {
    activeFilePath = EditorUtility.OpenFilePanel(loadTitle, defaultPath, extension);
    try {
      if (!string.IsNullOrEmpty(activeFilePath)) {
        dest = serializer.Deserialize<T>(activeFilePath);
        return true;
      }
    } catch (SerializationException) {
      Channel.editor.Log("Corrupted " + typeof(T) + " file: " + activeFilePath);
    }
    return false;
  }

  public void Save<T>(T src) {
    if (string.IsNullOrEmpty(activeFilePath)) {
      activeFilePath = EditorUtility.SaveFilePanel(saveTitle, defaultPath, "", extension);
    }
    // not an else -- activeFilePath may not have been set correctly above
    // also needs to execute even if the code above did
    if (!string.IsNullOrEmpty(activeFilePath)) {
      serializer.Serialize(activeFilePath, src);
    }
  }

  public void Reset() {
    activeFilePath = null;  // new file, forget path
  }
}
