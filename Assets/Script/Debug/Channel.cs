using UnityEngine;
// Wraps a channel string to make use of UberDebug.LogChannel more like Debug.Log
// Also add a flag to allow completely disabling logging through this channel
[System.Serializable]
public struct Channel {
  public readonly string channel;
  public bool enabled;

  // Global generic editor channel - use only when something more specific doesn't make sense
  public static Channel editor = new Channel("Editor", true);

  // Global generic game channel - use only when something more specific doesn't make sense
  public static Channel game = new Channel("Game", true);

  public Channel(string channel, bool enabled) {
    this.channel = channel;
    this.enabled = enabled;
  }

  public void Log(UnityEngine.Object context, string message, params object[] par) {
    if (! enabled) return;
    Debug.LogFormat(context,message,par);
    // UberDebug.LogChannel(context, channel, message, par);
  }

  public void Log(string message, params object[] par) {
    if (! enabled) return;
    Debug.LogFormat(message,par);
    // UberDebug.LogChannel(channel, message, par);
  }

  public void LogWarning(UnityEngine.Object context, string message, params object[] par) {
    Debug.LogWarningFormat(context,message,par);
    // UberDebug.LogWarningChannel(context, channel, message, par);
  }

  public void LogWarning(string message, params object[] par) {
    Debug.LogWarningFormat(message,par);
    // UberDebug.LogWarningChannel(channel, message, par);
  }

  public void LogError(UnityEngine.Object context, string message, params object[] par) {
    Debug.LogErrorFormat(context,message,par);
    // UberDebug.LogErrorChannel(context, channel, message, par);
  }

  public void LogError(string message, params object[] par) {
    Debug.LogErrorFormat(message,par);
    // UberDebug.LogErrorChannel(channel, message, par);
  }
}
