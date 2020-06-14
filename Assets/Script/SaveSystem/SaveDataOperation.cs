using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Profiling;
using static SaveLoad;

// Handle that wraps a save or load operation. Can be queried to see if the operation is finished
public partial class SaveDataOperation {
  public enum Type { Game, Options }
  public enum Mode { Save, Load }

  private static CustomSampler sampler = CustomSampler.Create("SaveDataOperationSampler");

  public string saveName { get; private set; }
  private string path;
  private Task mainTask;
  private Task onFailureTask;
  private Params saveData;
  private Func<Exception, bool> onFail;

  public bool readyToStart { get { return mainTask.isReadyToStart(); } }
  public bool failed { get { return onFailureTask.isFailed(); } }
  public bool finished { get { return onFailureTask.isFinished(); } }
  public bool active { get { return mainTask.isActive() || onFailureTask.isActive(); } }

  public Mode mode { get { return saveData.mode; } }
  public Type type { get { return saveData.type; } }

  public Exception exception { get { return onFailureTask.Exception; } }

  internal SaveDataOperation(string saveName, Params saveData, Func<Exception, bool> onFail) {
    if (!saveName.EndsWith(".bro")) saveName += ".bro";
    this.saveName = saveName;
    this.saveData = saveData;
    this.onFail = onFail;
    path = Path.Combine(saveData.dirPath, saveName);
    Action act = getAction();
    // don't start immediately, only one operation can be on the go at a time
    mainTask = new Task(act);
    TaskContinuationOptions config = TaskContinuationOptions.OnlyOnFaulted
      | TaskContinuationOptions.ExecuteSynchronously;
    onFailureTask = mainTask.ContinueWith(exceptionHandler, config);
  }

  private void exceptionHandler(Task t) {
    t.Exception.Handle(onFail);
  }

  public void start() {
    mainTask.Start();
  }

  public void startSynchronously() {
    mainTask.RunSynchronously();
  }

  private Action getAction() {
    switch(saveData.mode) {
      case Mode.Save: return doSave;
      case Mode.Load: return doLoad;
    }
    Debug.AssertFormat(false, "Unhandled save mode: {0}", saveData.mode);
    return null;
  }

  private void doSave() {
    // Register the thread for profiling
    Profiler.BeginThreadProfiling("Tasks", "Save Task");
    sampler.Begin();
    channel.Log("Saving to {0}", saveName);

    BinaryFormatter bf = new BinaryFormatter();
    using (FileStream file = File.Create(path))
    {
      // does this need to be copied before beginning the save?
      saveData.action(bf, file);
    }

    // Unregister the thread before exit
    sampler.End();
    Profiler.EndThreadProfiling();
  }

  private void doLoad() {
    // Register the thread for profiling
    Profiler.BeginThreadProfiling("Tasks", "Load Task");
    sampler.Begin();
    channel.Log("Loading {0}", saveName);

    BinaryFormatter bf = new BinaryFormatter();
    using (FileStream file = File.Open(path, FileMode.Open)) {
      saveData.action(bf, file);
    }

    // Unregister the thread before exit
    sampler.End();
    Profiler.EndThreadProfiling();
  }
}
