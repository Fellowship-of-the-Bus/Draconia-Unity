using System;
using System.Collections.Generic;

public class TypeMap<Val> {
  public Dictionary<Type, Val> dic = new Dictionary<Type, Val>();
  public void Add<T>(Val obj) {
    dic[typeof(T)] = obj;
  }

  public void Add<Key>(Key k, Val v) {
    dic[k.GetType()] = v;
  }

  public Val Get(Type t) {
    if (! ContainsKey(t)) {
      return default(Val);
    }
    return dic[t];
  }

  public Val Get<Key>(Key k) {
    return Get(k.GetType());
  }

  public bool ContainsKey(Type t) {
    return dic.ContainsKey(t);
  }

  public bool ContainsKey<T>(T t) {
    return ContainsKey(t.GetType());
  }
}


