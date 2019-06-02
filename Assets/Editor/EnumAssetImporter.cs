using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.AssetImporters;
using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Text;

[ScriptedImporter(1, "enum")]
public class EnumAssetImporter : ScriptedImporter {
  private static string scriptPath;

  private class ParsedData {
    public struct Enumerator {
      public string name;
      public int value;
    }

    public string namespaceName;
    public string name;
    public Enumerator[] enumerators;

    public int minValue;
    public int maxValue;
  }

  private StreamWriter file;

  public EnumAssetImporter() {
    Channel.editor.Log("New EnumAssetImporter");
  }

  public override void OnImportAsset(AssetImportContext ctx) {
    if (scriptPath == null) {
      scriptPath = Path.Combine(Application.dataPath, "Script/generated/enum");
      System.IO.Directory.CreateDirectory(scriptPath);
    }

    EnumData data = JsonUtility.FromJson<EnumData>(File.ReadAllText(ctx.assetPath));

    ParsedData parsed = validate(ctx, data);

    MonoScript script = generate(parsed);

    // 'script' is a a GameObject and will be automatically converted into a Prefab
    // (Only the 'Main Asset' is elligible to become a Prefab.)
    ctx.AddObjectToAsset("main obj", script);
    ctx.SetMainObject(script);
  }

  private ParsedData validate(AssetImportContext ctx, EnumData data) {
    // TODO: ensure enum data is sensible, and convert into ParsedData
    // * no values outside int range (probably already handled by JsonUtility?)
    // * all names are valid identifiers (enum name, namespaces, enumerator names)
    // * ensure (warning?) no duplicate values

    ParsedData parsed = new ParsedData();

    int nElems = data.values.Length;
    switch(data.type) {
      // leave space for special member
      case EnumType.Bitmask: nElems += 1; break;
    }

    parsed.enumerators = new ParsedData.Enumerator[nElems];
    parsed.minValue = Int32.MaxValue;
    parsed.maxValue = Int32.MinValue;
    int currentValue = 0;
    int idxOffset = 0;
    switch(data.type) {
      case EnumType.Bitmask:
        // for Bitmask enums add a special None value and then start the first real value at 1
        parsed.enumerators[0].name = "None";
        parsed.enumerators[0].value = currentValue;
        parsed.minValue = currentValue;
        parsed.maxValue = currentValue;
        currentValue = 1;
        idxOffset = 1;
        break;
    }

    for (int i = 0; i < data.values.Length; ++i) {
      EnumValue enumValue = data.values[i];
      // reset cursor to chosen value
      if (enumValue.useCustomValue) {
        currentValue = enumValue.value;
      }

      // generate enumerator for current value
      parsed.enumerators[i+idxOffset].name = data.values[i].name;
      parsed.enumerators[i+idxOffset].value = currentValue;
      parsed.minValue = Math.Min(parsed.minValue, currentValue);
      parsed.maxValue = Math.Max(parsed.maxValue, currentValue);

      // increment based on kind of enum
      switch(data.type) {
        case EnumType.Sequential: currentValue += 1; break;
        case EnumType.Bitmask: currentValue <<= 1; break;  // TODO: get next power of 2, currently assuming all values are powers of 2
      }
    }

    parsed.name = data.name;
    parsed.namespaceName = NamespaceName(data.namespaces);
    return parsed;
  }

  private string NamespaceName(string[] namespaces) {
    // convert array of names into a single dot separated namespace name
    if (namespaces.Length > 0) {
      int length = namespaces[0].Length;
      for (int i = 1; i < namespaces.Length; ++i) {
        length += namespaces[i].Length + 1;
      }

      StringBuilder sb = new StringBuilder(namespaces[0], length);
      for (int i = 1; i < namespaces.Length; ++i) {
        sb.AppendFormat(".{0}", namespaces[i]);
      }
      return sb.ToString();
    }
    return "";
  }

  private MonoScript generate(ParsedData data) {
    // generate the top-level namespace
    CodeCompileUnit compileUnit = new CodeCompileUnit();
    CodeNamespace nameSpace = new CodeNamespace();
    nameSpace.Name = data.namespaceName;
    compileUnit.Namespaces.Add(nameSpace);

    // generate the type container, inheriting from Draconia.Enum
    CodeTypeDeclaration enumType = new CodeTypeDeclaration(data.name);
    nameSpace.Types.Add(enumType);
    enumType.IsStruct = true;
    enumType.BaseTypes.Add(new CodeTypeReference(typeof(Draconia.Enum)));

    // field containing the value of the enumerator
    CodeMemberField valueField = Code.Field(typeof(int), "value")
      .SetAttributes(MemberAttributes.Private);
    enumType.Members.Add(valueField);


    // generate all enumerators
    CodeExpression[] enumeratorReferences = new CodeExpression[data.enumerators.Length];
    for (int i = 0; i < data.enumerators.Length; ++i) {
      ParsedData.Enumerator enumValue = data.enumerators[i];
      enumType.Members.Add(Code.Field(data.name, enumValue.name)
        .SetAttributes(MemberAttributes.Public | MemberAttributes.Static)
        .SetInitializer(data.name, Code.Primitive(enumValue.value))
      );
      enumeratorReferences[i] = Code.VariableRef(enumValue.name);
    }

    string arrayName = "array";
    enumType.Members.Add(Code.Field(Code.ArrayType(data.name), arrayName)
      .SetAttributes(MemberAttributes.Private | MemberAttributes.Static)
      .SetArrayInitializer(Code.ArrayType(data.name), enumeratorReferences)
    );

    // private constructor(int)
    string param1 = "value";
    enumType.Members.Add(new CodeConstructor()
      .SetAttributes(MemberAttributes.Private)
      .AddParameter(typeof(int), param1)
      .AddAssignStatement(This.FieldRef(valueField.Name), Code.VariableRef(param1))
    );

    // public int getInt()
    enumType.Members.Add(new CodeMemberMethod()
      .SetReturnType(typeof(int))
      .SetAttributes(MemberAttributes.Public | MemberAttributes.Final)
      .SetName("toInt")
      .AddReturnStatement(This.FieldRef(valueField.Name))
    );

    // implicit conversion
    // CodeDom doesn't provide a way to generate implicits or operator overloads, so dirty hack:
    CodeSnippetTypeMember conversion = new CodeSnippetTypeMember($"\n    public static implicit operator int({data.name} value) {{ return {valueField.Name}; }}");
    enumType.Members.Add(conversion);

    // public int minValue()
    enumType.Members.Add(Code.Method()
      .SetReturnType(typeof(int))
      .SetAttributes(MemberAttributes.Public | MemberAttributes.Final | MemberAttributes.Static)
      .SetName("minValue")
      .AddReturnStatement(Code.Primitive(data.minValue))
    );

    // public int maxValue()
    enumType.Members.Add(Code.Method()
      .SetReturnType(typeof(int))
      .SetAttributes(MemberAttributes.Public | MemberAttributes.Final | MemberAttributes.Static)
      .SetName("maxValue")
      .AddReturnStatement(Code.Primitive(data.maxValue))
    );

    // public <ENUM> getByIndex(int)
    string idxName = "idx";
    enumType.Members.Add(Code.Method()
      .SetReturnType(data.name)
      .SetAttributes(MemberAttributes.Public | MemberAttributes.Final | MemberAttributes.Static)
      .SetName("getByIndex")
      .AddParameter(typeof(int), idxName)
      .AddReturnStatement(Code.VariableRef(arrayName).ArrayIndex(Code.VariableRef(idxName)))
    );

    // public static ArrayEnumerator<ENUM> Enumerator()
    enumType.Members.Add(Code.Method()
      .SetReturnType($"ArrayEnumerator<{data.name}>")
      .SetAttributes(MemberAttributes.Public | MemberAttributes.Final | MemberAttributes.Static)
      .SetName("Enumerator")
      .AddReturnStatement(Code.New($"ArrayEnumerator<{data.name}>", Code.VariableRef(arrayName)))
    );

    // write code to disk
    CSharpCodeProvider provider = new CSharpCodeProvider();
    using (StreamWriter sw = new StreamWriter(Path.Combine(scriptPath, data.name + "." + provider.FileExtension))) {
      provider.GenerateCodeFromCompileUnit(compileUnit, sw, new CodeGeneratorOptions() {
        IndentString = "  ",
        BlankLinesBetweenMembers = false,
      });
    }

    // TODO: this isn't really correct, but as far as I can tell there's no way to generate a new MonoScript, so instead the code is currently held separate to this script object...
    return new MonoScript();
  }
}
