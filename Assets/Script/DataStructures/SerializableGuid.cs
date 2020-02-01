using System;
using System.Runtime.Serialization;
using UnityEngine;

// From: https://druss.co/2016/04/unity3d-serialize-and-deserialize-system-guid-using-jsonutility/
// TODO: Change SerializableGuid from class to struct
// BODY: There is a bug in Mono that prevents deserializing structs with array members. There is
// no reason for this type to be heap allocated, so once the bug is fixed we should change it back.
// See: https://github.com/mono/mono/issues/8915
[Serializable]
public class SerializableGuid: IComparable, IComparable<SerializableGuid>, IEquatable<SerializableGuid>, ISerializationCallbackReceiver {
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

  public bool isValid() {
    return guid != Guid.Empty;
  }

  // TODO: do we need both unity and C# derialization callbacks?

  // C# deserialization finished
  [OnDeserialized]
  public void onPostDeserialize(StreamingContext context) {
    this.guid = new Guid(value);
  }

  // pre unity serialize
  public void OnBeforeSerialize() {
    this.value = guid.ToByteArray();
  }

  // unity deserialization finished
  public void OnAfterDeserialize() {
    if (value == null || value.Length != 16) {
      Debug.LogErrorFormat("Bad guid OnAfterDeserialize: {0} {1}", value, value != null ? value.Length : 0);
    }
    this.guid = new Guid(value);
  }
}
