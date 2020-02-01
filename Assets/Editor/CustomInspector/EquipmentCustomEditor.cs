using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class EquipmentInspector<E, D> : PropertyDrawer where E : Equipment where D : ItemData {
  public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
    E target = property.GetTarget<E>();

    // Begin/EndProperty allow prefab editing to work properly, e.g. bold for overrides, etc.
    label = EditorGUI.BeginProperty(position, label, property);

    // Draw field name label - do not allow label to take focus
    position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

    // Don't indent child fields
    var indent = EditorGUI.indentLevel;
    EditorGUI.indentLevel = 0;

    // if any control changes value after this point, EndChangeCheck returns true
    EditorGUI.BeginChangeCheck();

    ItemData oldData = target.itemData;
    if (target.itemData == null) {
      EquipmentDB db = (EquipmentDB)AssetDatabase.LoadAssetAtPath("Assets/Prefab/Resources/Map/EquipmentDB.prefab", typeof(EquipmentDB));
      target.itemData = db.find(target);
    }
    target.itemData = (ItemData)EditorGUI.ObjectField(position, target.itemData, typeof(D), false);

    if (EditorGUI.EndChangeCheck()) {
      // need to record changes through the undo system in order for the scene to be marked
      // dirty so that it can be saved.
      Object gameObject = property.serializedObject.targetObject;
      Undo.RecordObject(gameObject, "ItemData Changed");
      PrefabUtility.RecordPrefabInstancePropertyModifications(gameObject);
    }

    // Set indent back to what it was
    EditorGUI.indentLevel = indent;

    EditorGUI.EndProperty();
  }
}

[CustomPropertyDrawer(typeof(Weapon))]
public class WeaponInspector : EquipmentInspector<Weapon, WeaponData> {}

[CustomPropertyDrawer(typeof(Armour))]
public class ArmourInspector : EquipmentInspector<Armour, ArmourData> {}

