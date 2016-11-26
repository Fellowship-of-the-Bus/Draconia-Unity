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

  public Val Get<Key>(Key k) {
    if (! ContainsKey(k.GetType())) {
      return default(Val);
    }
    return dic[k.GetType()];
    //return Get<k.GetType()>();
  }

  public bool ContainsKey(Type t) {
    return dic.ContainsKey(t);
  }

  public bool ContainsKey<T>(T t) {
    return ContainsKey(t.GetType());
  }
}


