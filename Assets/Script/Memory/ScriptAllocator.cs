using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

// converts prefab to script at allocation time
public class ScriptAllocator<T> : ObjectPool<T>.Allocator where T : class,PoolableObject {

  private PrefabAllocator allocator;

  public ScriptAllocator(PrefabAllocator allocator) {
    this.allocator = allocator;
  }

  public T alloc() {
    return allocator.alloc().GetComponent<T>();
  }
}
