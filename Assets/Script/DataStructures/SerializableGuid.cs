using System;
using System.Runtime.Serialization;
using UnityEngine;

// From: https://druss.co/2016/04/unity3d-serialize-and-deserialize-system-guid-using-jsonutility/

[Serializable]
public struct SerializableGuid: IComparable, IComparable<SerializableGuid>, IEquatable<SerializableGuid>, ISerializationCallbackReceiver {
  [SerializeField]
  private byte[] value;

  [NonSerialized]
  private Guid guid;

  private SerializableGuid(byte[] value) {
    this.value = value;
    guid = new Guid(value);
  }

  private SerializableGuid(Guid guid) {
    this.value = guid.ToByteArray();
    this.guid = guid;
  }

  public static implicit operator SerializableGuid(Guid guid) {
    return new SerializableGuid(guid);
  }

  public static implicit operator Guid(SerializableGuid serializableGuid) {
    return serializableGuid.guid;
  }

  public int CompareTo(object value) {
    if (value == null) return 1;
    if (!(value is SerializableGuid)) throw new ArgumentException("Must be SerializableGuid");
    SerializableGuid other = (SerializableGuid)value;
    return this.guid.CompareTo(other.guid);
  }

  public int CompareTo(SerializableGuid other) {
    return guid.CompareTo(other.guid);
  }

  public bool Equals(SerializableGuid other) {
    return guid.Equals(other.guid);
  }

  public override bool Equals(object obj) {
    return base.Equals(obj);
  }

  public override int GetHashCode() {
    return guid.GetHashCode();
  }

  public override string ToString() {
    return guid.ToString();
  }

  // TODO: do we need both unity and C# derialization callbacks?

  // C# deserialization finished
  [OnDeserialized]
  private void onPostDeserialize(StreamingContext context) {
    this.guid = new Guid(value);
  }

  // pre unity serialize
  public void OnBeforeSerialize() {
    Debug.LogFormat("Serializing: value: {0} & guid: {1}", value, guid);
    this.value = guid.ToByteArray();
  }

  // unity deserialization finished
  public void OnAfterDeserialize() {
    Debug.LogFormat("Deserialized {0}", value);
    this.guid = new Guid(value);
  }
}