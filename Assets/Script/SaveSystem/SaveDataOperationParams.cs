using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Profiling;

public partial class SaveDataOperation {
  // Set of parameters to control how to save or load a particular kind of save data (Game data, Options data, etc.)
  internal struct Params {
    public delegate void Action(IFormatter formatter, Stream stream);
    public Action action { get; private set; }
    public Type type { get; private set; }
    public Mode mode { get; private set; }
    public string dirPath { get; private set; }

    private Params(Action action, Type type, Mode mode, string dirPath) {
      this.action = action;
      this.type = type;
      this.mode = mode;
      this.dirPath = dirPath;
    }

    public static Params LoadGame {
      get { return new Params(loadGameAction, Type.Game, Mode.Load, SaveLoad.progressDirPath); }
    }

    public static Params SaveGame {
      get { return new Params(saveGameAction, Type.Game, Mode.Save, SaveLoad.progressDirPath); }
    }

    public static Params LoadOptions {
      get { return new Params(loadOptionsAction, Type.Options, Mode.Load, SaveLoad.dirPath); }
    }

    public static Params SaveOptions {
      get { return new Params(saveOptionsAction, Type.Options, Mode.Save, SaveLoad.dirPath); }
    }

    private static void saveAction<T>(T t, IFormatter format, Stream stream) {
      format.Serialize(stream, t);
    }


    private static void loadAction<T>(ref T t, IFormatter format, Stream stream) {
      t = (T)format.Deserialize(stream);
    }

    private static Action loadGameAction = (IFormatter formatter, Stream stream) => loadAction(ref GameData.gameData, formatter, stream);
    private static Action loadOptionsAction = (IFormatter formatter, Stream stream) => loadAction(ref Options.instance, formatter, stream);
    private static Action saveGameAction = (IFormatter formatter, Stream stream) => saveAction(GameData.gameData, formatter, stream);
    private static Action saveOptionsAction = (IFormatter formatter, Stream stream) => saveAction(Options.instance, formatter, stream);
  }
}
