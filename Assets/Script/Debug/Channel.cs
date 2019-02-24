// Wraps a channel string to make use of UberDebug.LogChannel more like Debug.Log
// Also add a flag to allow completely disabling logging through this channel
[System.Serializable]
public struct Channel {
  public readonly string channel;
  public bool enabled;

  public Channel(string channel, bool enabled) {
    this.channel = channel;
    this.enabled = enabled;
  }

  public void Log(UnityEngine.Object context, string message, params object[] par) {
    if (! enabled) return;
    UberDebug.LogChannel(context, channel, message, par);
  }

  public void Log(string message, params object[] par) {
    if (! enabled) return;
    UberDebug.LogChannel(channel, message, par);
  }
}
