using UnityEngine;

[System.Serializable]
public class ItemData : ScriptableObject {
  public SerializableGuid guid = System.Guid.NewGuid();
  public new string name;
  [TextArea]
  public string tooltip;
  public Sprite image;
  public Equipment.Tier tier;
  public GameObject model;
}
