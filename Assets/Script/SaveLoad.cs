using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Profiling;

public static class SaveLoad {
  private static Channel channel = new Channel("Save Game", true);
  public static string autoSaveName = "AutoSave";
  public static string dirPath = Path.Combine(Application.persistentDataPath, "savedGames");

  private static DirectoryInfo dir = System.IO.Directory.CreateDirectory(dirPath);

  public static bool active {
    get; private set;
  }

  public enum Mode {
    Inactive, Save, Load
  }
  public static Mode currentMode { get; private set; }

  private static CustomSampler sampler = CustomSampler.Create("SaveLoadSampler");

  private static void doSave(string saveName) {
    // Register the thread for profiling
    Profiler.BeginThreadProfiling("Tasks", "Save Task");
    sampler.Begin();

    Debug.Assert(! active);
    currentMode = Mode.Save;
    active = true;
    BinaryFormatter bf = new BinaryFormatter();
    if (!saveName.EndsWith(".bro")) saveName += ".bro";
    FileStream file = File.Create(Path.Combine(dirPath, saveName));
    // does this need to be copied before beginning the save?
    bf.Serialize(file, GameData.gameData);
    file.Close();
    currentMode = Mode.Inactive;
    active = false;

    // Unregister the thread before exit
    sampler.End();
    Profiler.EndThreadProfiling();
  }

  public static void save(string saveName) {
    // run on a worker thread
    Task.Run(() => doSave(saveName));
  }

  public static void saveAuto() {
    save(SaveLoad.autoSaveName);
  }

  private static void doLoad(string saveName) {
    // Register the thread for profiling
    Profiler.BeginThreadProfiling("Tasks", "Load Task");
    sampler.Begin();

    Debug.Assert(! active);
    currentMode = Mode.Load;
    active = true;
    saveName = Path.Combine(dirPath, saveName);
    if (File.Exists(saveName)) {
      BinaryFormatter bf = new BinaryFormatter();
      FileStream file = File.Open(saveName, FileMode.Open);
      try {
        GameData.gameData = (GameData)bf.Deserialize(file);
      } catch (SerializationException) {
        channel.Log("Corrupted save file: " + saveName);
      }
      file.Close();
    } else channel.Log(saveName + " doesn't exist");
    active = false;

    // Unregister the thread before exit
    sampler.End();
    Profiler.EndThreadProfiling();
  }

  public static void load(string saveName) {
    // run on a worker thread
    Task.Run(() => doLoad(saveName));
  }

  public static void loadAuto() {
    load(SaveLoad.autoSaveName);
  }

  public static IEnumerable<FileInfo> listSaveFiles() {
    return dir.GetFiles("*.bro");
  }
}
