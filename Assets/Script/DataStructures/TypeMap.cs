using System;
using System.Collections.Generic;

public class TypeMap<Val> {
  public Dictionary<Type, Val> dic = new Dictionary<Type, Val>();
  public void Add<T>(Val obj) {
    dic[typeof(T)] = obj;
  }

  public void Add<Key>(Key k, Val v) {
    Add<Key>(v);
  }

  public Val Get<T>() {
    if (! ContainsKey(typeof(T))) {
      return default(Val);
    }
    return dic[typeof(T)];
  }

  public Val Get<Key>(Key k) {
    return Get<Key>();
  }

  public bool ContainsKey(Type t) {
    return dic.ContainsKey(t);
  }

  public bool ContainsKey<T>(T t) {
    return ContainsKey(typeof(T));
  }
}


