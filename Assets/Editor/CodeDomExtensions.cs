using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Text;

//////////////////////////////////////////////////////
// some extensions to make generating codedom nicer //
//////////////////////////////////////////////////////
public static class CodeDomExtensions {
  public static T SetReturnType<T>(this T m, Type type) where T : CodeMemberMethod {
    m.ReturnType = new CodeTypeReference(type);
    return m;
  }

  public static T SetReturnType<T>(this T m, string type) where T : CodeMemberMethod {
    m.ReturnType = new CodeTypeReference(type);
    return m;
  }

  public static T SetName<T>(this T m, string name) where T : CodeTypeMember {
    m.Name = name;
    return m;
  }

  public static T SetAttributes<T>(this T m, MemberAttributes attr) where T : CodeTypeMember {
    m.Attributes = attr;
    return m;
  }

  public static T AddReturnStatement<T>(this T m, CodeExpression expr) where T : CodeMemberMethod {
    m.Statements.Add(new CodeMethodReturnStatement(expr));
    return m;
  }

  public static T AddParameter<T>(this T m, Type t, string name) where T : CodeMemberMethod {
    m.Parameters.Add(new CodeParameterDeclarationExpression(t, name));
    return m;
  }

  public static T AddAssignStatement<T>(this T m, CodeExpression lhs, CodeExpression rhs) where T : CodeMemberMethod {
    m.Statements.Add(new CodeAssignStatement(lhs, rhs));
    return m;
  }

  public static T SetInitializer<T>(this T f, string t, params CodeExpression[] args) where T : CodeMemberField {
    f.InitExpression = new CodeObjectCreateExpression(t, args);
    return f;
  }

  public static T SetArrayInitializer<T>(this T f, CodeTypeReference t, params CodeExpression[] args) where T : CodeMemberField {
    f.InitExpression = new CodeArrayCreateExpression(t, args);
    return f;
  }

  public static CodeArrayIndexerExpression ArrayIndex(this CodeExpression expr, params CodeExpression[] idx) {
    return new CodeArrayIndexerExpression(expr, idx);
  }
}

public class Code {
  public static CodeVariableReferenceExpression VariableRef(string name) {
    return new CodeVariableReferenceExpression(name);
  }

  public static CodePrimitiveExpression Primitive(object obj) {
    return new CodePrimitiveExpression(obj);
  }

  public static CodeMemberMethod Method() {
    return new CodeMemberMethod();
  }

  public static CodeMemberField Field(Type t, string name) {
    return new CodeMemberField(t, name);
  }

  public static CodeMemberField Field(string t, string name) {
    return new CodeMemberField(t, name);
  }

  public static CodeMemberField Field(CodeTypeReference t, string name) {
    return new CodeMemberField(t, name);
  }

  public static CodeTypeReference ArrayType(string t, int rank = 1) {
    return new CodeTypeReference(t, rank);
  }

  public static CodeObjectCreateExpression New(string t, params CodeExpression[] args) {
    return new CodeObjectCreateExpression(t, args);
  }
}

public class This {
  public static CodeFieldReferenceExpression FieldRef(string name) {
    return new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), name);
  }
}
