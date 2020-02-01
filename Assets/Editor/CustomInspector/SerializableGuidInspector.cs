using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

[CustomPropertyDrawer(typeof(SerializableGuid))]
public class SerializableGuidInspector : PropertyDrawer {
  public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
    SerializableGuid target = property.GetTarget<SerializableGuid>();

    // Begin/EndProperty allow prefab editing to work properly, e.g. bold for overrides, etc.
    label = EditorGUI.BeginProperty(position, label, property);

    // Draw field name label - do not allow label to take focus
    position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

    // Don't indent child fields
    var indent = EditorGUI.indentLevel;
    EditorGUI.indentLevel = 0;

    // Draw guid as a string
    EditorGUI.LabelField(position, target.ToString());

    // Set indent back to what it was
    EditorGUI.indentLevel = indent;

    EditorGUI.EndProperty();
  }
}
