using System;
using System.Collections;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

// extension methods to simplify coding custom inspectors
public static class CustomInspector {
  public static T GetTarget<T>(this SerializedProperty property) {
    object targetObject = property.serializedObject.targetObject;
    return (T)GetTarget(targetObject, property.propertyPath);
  }

  // modified from: https://answers.unity.com/questions/425012/get-the-instance-the-serializedproperty-belongs-to.html
  private static object GetTarget(object target, string propertyPath) {
    string path = propertyPath.Replace(".Array.data[", "[");
    string[] elements = path.Split('.');
    foreach (string element in elements) {
      Debug.Assert(target != null);
      int indexOfOpen = element.IndexOf("[");
      if (indexOfOpen != -1) { // handle array as a special case
        string elementName = element.Substring(0, indexOfOpen);
        int len = element.Length-indexOfOpen;
        StringBuilder sb = new StringBuilder(element, indexOfOpen, len, len);
        sb.Replace("[", "").Replace("]", "");
        int index = Convert.ToInt32(sb.ToString());
        target = GetValue(target, elementName, index);
      } else {
        target = GetValue(target, element);
      }
    }
    return target;
  }

   private static object GetValue(object source, string name) {
    var type = source.GetType();
    var f = FindFieldInTypeHierarchy(type, name);
    if (f == null) {
      var p = FindPropertyInTypeHierarchy(type, name);
      if (p == null) return null;
      return p.GetValue(source, null);
    }
    return f.GetValue(source);
  }

  private static object GetValue(object source, string name, int index) {
    IEnumerable enumerable = GetValue(source, name) as IEnumerable;
    IEnumerator enm = enumerable.GetEnumerator();
    while (index-- >= 0) enm.MoveNext();
    return enm.Current;
  }

  private static FieldInfo FindFieldInTypeHierarchy(Type providedType, string fieldName) {
    FieldInfo field = providedType.GetField(fieldName, (BindingFlags)(-1));
    while (field == null && providedType.BaseType != null) {
      providedType = providedType.BaseType;
      field = providedType.GetField(fieldName, (BindingFlags)(-1));
    }
    return field;
  }

  private static PropertyInfo FindPropertyInTypeHierarchy(Type providedType, string propertyName) {
    PropertyInfo property = providedType.GetProperty(propertyName, (BindingFlags)(-1));
    while (property == null && providedType.BaseType != null) {
      providedType = providedType.BaseType;
      property = providedType.GetProperty(propertyName, (BindingFlags)(-1));
    }
    return property;
  }
}
