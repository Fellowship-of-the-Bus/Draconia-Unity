using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine; // needed for debug, unfortunately

// Interface for save operations that can be performed without interaction from Unity
public static class SaveLoad {
  public enum Mode {
      Saving, Loading, Inactive
  }
  public static string autoSaveName = "AutoSave.bro";
  public static string optionsName = "Options.bro";
  internal static Channel channel = new Channel("Save Game", true);

  public static string dirPath = Path.Combine(Application.persistentDataPath, "savedGames");
  public static string progressDirPath = Path.Combine(dirPath, "progress");

  public static DirectoryInfo dir = System.IO.Directory.CreateDirectory(dirPath);
  public static DirectoryInfo progressDir = System.IO.Directory.CreateDirectory(progressDirPath);

  private static ConcurrentQueue<SaveDataOperation> queuedOperations = new ConcurrentQueue<SaveDataOperation>();

  private static SaveDataOperation runOneOp(SaveDataOperation op) {
    if (op == null) return null;
    if (op.active) return op;
    if (op.readyToStart) {
      op.start();
      return op;
    } else if (op.failed) {
      throw op.exception;
    }
    SaveDataOperation value;
    bool success = queuedOperations.TryDequeue(out value);
    Debug.AssertFormat(success, "Failed to finish operation for {0}", op.saveName);
    Debug.AssertFormat(op == value, "Logic error in finishing operation for {0}; {1} was removed from queue", op.saveName, value.saveName);
    Debug.AssertFormat(op.finished, "Error: removed an op that has not finished yet: {0}", op.saveName);
    return runOneOp(currentOperation);
  }

  public static SaveDataOperation run() {
    return runOneOp(currentOperation);
  }

  public static bool active { get { return ! queuedOperations.IsEmpty; } }

  private static bool onFailDefault(Exception ex) {
    return false;
  }

  private static SaveDataOperation makeOperation(string filename, SaveDataOperation.Params saveData, Func<Exception, bool> onFail) {
    SaveDataOperation op = new SaveDataOperation(filename, saveData, onFail);
    queuedOperations.Enqueue(op);
    return op;
  }

  private static SaveDataOperation makeOperation(string filename, SaveDataOperation.Params saveData) {
    return makeOperation(filename, saveData, onFailDefault);
  }

  public static SaveDataOperation save(string saveName) {
    return makeOperation(saveName, SaveDataOperation.Params.SaveGame);
  }

  public static SaveDataOperation saveAuto() {
    return makeOperation(autoSaveName, SaveDataOperation.Params.SaveGame);
  }

  public static SaveDataOperation saveOptions() {
    return makeOperation(optionsName, SaveDataOperation.Params.SaveOptions);
  }

  public static SaveDataOperation load(string saveName) {
    return makeOperation(saveName, SaveDataOperation.Params.LoadGame);
  }

  public static SaveDataOperation loadAuto() {
    return makeOperation(autoSaveName, SaveDataOperation.Params.LoadGame);
  }

  public static SaveDataOperation loadOptions(Func<Exception, bool> onFail) {
    return makeOperation(optionsName, SaveDataOperation.Params.LoadOptions, onFail);
  }

  public static IEnumerable<FileInfo> listSaveFiles() {
    return progressDir.GetFiles("*.bro");
  }

  private static SaveDataOperation currentOperation {
    get {
      SaveDataOperation result = null;
      queuedOperations.TryPeek(out result);
      return result;
    }
  }

  public static Mode currentMode {
    get {
      SaveDataOperation op = currentOperation;
      if (op == null) {
        return Mode.Inactive;
      }
      switch(op.mode) {
        case SaveDataOperation.Mode.Load: return Mode.Loading;
        case SaveDataOperation.Mode.Save: return Mode.Saving;
      }
      Debug.AssertFormat(false, "Invalid save operation mode: {0}", op.mode);
      return Mode.Inactive;
    }
  }

  public static void deleteAllSaveData() {
    DirectoryInfo persistentDataDir = System.IO.Directory.CreateDirectory(Application.persistentDataPath);
    persistentDataDir.Delete(true);
    // temporarily recreate save data path to prevent crashes
    dir = System.IO.Directory.CreateDirectory(dirPath);
  }
}
