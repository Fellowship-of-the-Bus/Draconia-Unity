using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public enum EnumType {
  Sequential,
  Bitmask
}

[System.Serializable]
public class EnumValue {
  public string name;
  public bool useCustomValue;
  public int value;
}

[System.Serializable]
public class EnumData {
  public string name;
  public string[] namespaces;
  public EnumType type;
  public EnumValue[] values;
}

// IngredientDrawer
[CustomPropertyDrawer(typeof(EnumValue))]
public class EnumValueDrawer : PropertyDrawer {
  private static GUIContent IntMax = new GUIContent(Int32.MaxValue.ToString());

  // Draw the property inside the given rect
  public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
    // Using BeginProperty / EndProperty on the parent property means that
    // prefab override logic works on the entire property.
    EditorGUI.BeginProperty(position, label, property);

    // Draw label
    position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

    // Don't make child fields be indented
    var indent = EditorGUI.indentLevel;
    EditorGUI.indentLevel = 0;

    // Calculate rects
    int toggleWidth = (int)GUI.skin.toggle.CalcSize(GUIContent.none).x;
    int intFieldWidth = (int)GUI.skin.textField.CalcSize(IntMax).x;
    var nameRect = new Rect(position.x, position.y, position.width-toggleWidth-intFieldWidth, position.height);
    var toggleRect = new Rect(nameRect.xMax, position.y, toggleWidth, position.height);
    var valueRect = new Rect(toggleRect.xMax, position.y, intFieldWidth, position.height);

    // Draw fields - passs GUIContent.none to each so they are drawn without labels
    EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("name"), GUIContent.none);
    SerializedProperty useCustomValue = property.FindPropertyRelative("useCustomValue");
    EditorGUI.PropertyField(toggleRect, useCustomValue, GUIContent.none);
    using (new EditorGUI.DisabledScope(!useCustomValue.boolValue)) {
      EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("value"), GUIContent.none);
    }

    // Set indent back to what it was
    EditorGUI.indentLevel = indent;

    EditorGUI.EndProperty();
  }
}

public class EnumEditor : EditorWindow {
  public EnumData data;

  public EditorFileManager fileManager;

  [MenuItem("Window/Enum Editor")]
  static void Init() {
    EnumEditor instance = (EnumEditor)EditorWindow.GetWindow(typeof(EnumEditor));
    instance.onNew();
    instance.Show();
  }

  // needed to maintain scroll position between draw calls
  private Vector2 scrollPos;

  private void OnEnable() {
    fileManager = new EditorFileManager(
      serializer: new JsonSerializer(),
      saveTitle: "Save enum file",
      loadTitle: "Select enum file",
      path: Path.Combine(Application.dataPath, "EnumData/data"),
      extension: "enum"
    );
  }

  private void OnGUI() {
    EditorGUILayout.BeginHorizontal();
    if (GUILayout.Button("New")) {
      onNew();
    }
    if (GUILayout.Button("Load")) {
      onLoad();
    }
    if (GUILayout.Button("Save")) {
      onSave();
    }
    EditorGUILayout.EndHorizontal();

    scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
    SerializedObject serializedObject = new SerializedObject(this);
    SerializedProperty serializedProperty = serializedObject.FindProperty("data");
    EditorGUILayout.PropertyField(serializedProperty, true);
    serializedObject.ApplyModifiedProperties();
    EditorGUILayout.EndScrollView();
  }

  private void onNew() {
    fileManager.Reset();
    data = new EnumData();

    SerializedObject serializedObject = new SerializedObject(this);
    SerializedProperty serializedProperty = serializedObject.FindProperty("data");
    serializedProperty.isExpanded = true;
    while (serializedProperty.NextVisible(true)) {
      serializedProperty.isExpanded = true;
    }
  }

  private bool onLoad() {
    return fileManager.Load(ref data);
  }

  private void onSave() {
    fileManager.Save(data);
  }
}
