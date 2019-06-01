using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.AssetImporters;
using Microsoft.CSharp;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Text;

[ScriptedImporter(1, "enum")]
public class EnumAssetImporter : ScriptedImporter {
  private static string scriptPath;

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

    validate(ctx, data);

    MonoScript script = generate(data);

    // 'script' is a a GameObject and will be automatically converted into a Prefab
    // (Only the 'Main Asset' is elligible to become a Prefab.)
    ctx.AddObjectToAsset("main obj", script);
    ctx.SetMainObject(script);
  }

  private void validate(AssetImportContext ctx, EnumData data) {
    // TODO: ensure enum data is sensible:
    // * no values outside int range (probably already handled by JsonUtility?)
    // * all names are valid identifiers (enum name, namespaces, enumerator names)
    // * ensure (warning?) no duplicate values
  }

  private MonoScript generate(EnumData data) {
    // generate the top-level namespace
    CodeCompileUnit compileUnit = new CodeCompileUnit();
    CodeNamespace nameSpace = new CodeNamespace();
    if (data.namespaces.Length > 0) {
      int length = data.namespaces[0].Length;
      for (int i = 1; i < data.namespaces.Length; ++i) {
        length += data.namespaces[i].Length + 1;
      }

      StringBuilder sb = new StringBuilder(data.namespaces[0], length);
      for (int i = 1; i < data.namespaces.Length; ++i) {
        sb.AppendFormat(".{0}", data.namespaces[i]);
      }
      nameSpace.Name = sb.ToString();
      compileUnit.Namespaces.Add(nameSpace);
    }

    // generate the type container, inheriting from Draconia.Enum
    CodeTypeDeclaration enumType = new CodeTypeDeclaration(data.name);
    nameSpace.Types.Add(enumType);
    enumType.IsStruct = true;
    enumType.BaseTypes.Add(new CodeTypeReference(typeof(Draconia.Enum)));

    // field containing the value of the enumerator
    CodeMemberField field1 = new CodeMemberField(typeof(int), "value");
    field1.Attributes = MemberAttributes.Private;
    enumType.Members.Add(field1);

    int currentValue = 0;
    switch(data.type) {
      case EnumType.Bitmask:
        // for Bitmask enums add a special None value and then start the first real value at 1
        CodeMemberField enumerator = new CodeMemberField(data.name, "None");
        enumerator.Attributes = MemberAttributes.Public | MemberAttributes.Static;
        enumerator.InitExpression = new CodeObjectCreateExpression(data.name, new CodeExpression[] { new CodePrimitiveExpression(currentValue) });
        enumType.Members.Add(enumerator);
        currentValue = 1;
        break;
    }

    for (int i = 0; i < data.values.Length; ++i) {
      EnumValue enumValue = data.values[i];
      // reset cursor to chosen value
      if (enumValue.useCustomValue) {
        currentValue = enumValue.value;
      }

      // generate enumerator for current value
      CodeMemberField enumerator = new CodeMemberField(data.name, enumValue.name);
      enumerator.Attributes = MemberAttributes.Public | MemberAttributes.Static;
      enumerator.InitExpression = new CodeObjectCreateExpression(data.name, new CodeExpression[] { new CodePrimitiveExpression(currentValue) });
      enumType.Members.Add(enumerator);

      // increment based on kind of enum
      switch(data.type) {
        case EnumType.Sequential: currentValue += 1; break;
        case EnumType.Bitmask: currentValue <<= 1; break;  // TODO: get next power of 2, currently assuming all values are powers of 2
      }
    }

    // private constructor(int)
    CodeConstructor constructor1 = new CodeConstructor();
    constructor1.Attributes = MemberAttributes.Private;
    CodeParameterDeclarationExpression param1 = new CodeParameterDeclarationExpression(typeof(int), "value");
    constructor1.Parameters.Add(param1);
    constructor1.Statements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), field1.Name), new CodeVariableReferenceExpression(param1.Name)));
    enumType.Members.Add(constructor1);

    // public int getInt()
    CodeMemberMethod method1 = new CodeMemberMethod();
    method1.Attributes = MemberAttributes.Public | MemberAttributes.Final;
    method1.Name = "toInt";
    method1.ReturnType = new CodeTypeReference(typeof(int));
    method1.Statements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), field1.Name)));
    enumType.Members.Add(method1);

    // implicit conversion
    // CodeDom doesn't provide a way to generate implicits or operator overloads, so dirty hack:
    CodeSnippetTypeMember conversion = new CodeSnippetTypeMember($"\n    public static implicit operator int({data.name} value) {{ return {field1.Name}; }}");
    enumType.Members.Add(conversion);

    // public int minValue()

    // public int maxValue()


    // public <ENUM> getByIndex(int)

    // public IEnumerator

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
