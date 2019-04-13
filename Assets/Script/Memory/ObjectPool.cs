using UnityEngine;
using System;
using System.Collections.Generic;

public class ObjectPool<T> where T : class, PoolableObject {
  private struct Node {
    public T obj;
    public bool used;
  }
  private Node[] pool;
  public interface Allocator {
    T alloc();
  }
  private Allocator allocator;
  private bool grow;
  private const int growFactor = 2;

  private static Channel channel = new Channel("ObjectPool", true);

  public ObjectPool(int size, Allocator allocator, bool grow) {
    this.allocator = allocator;
    pool = new Node[size];
    growPool(0, size);
    this.grow = grow;
  }

  private void growPool(int start, int end) {
    for (int i = start; i < end; ++i) {
      pool[i].obj = allocator.alloc();
    }
  }

  private T alloc(int i) {
    pool[i].obj.OnPoolInitialize();
    pool[i].used = true;
    return pool[i].obj;
  }

  public T alloc() {
    for (int i = 0; i < pool.Length; ++i) {
      if (!pool[i].used) {
        return alloc(i);
      }
    }

    if (grow) {
      int newSize = pool.Length*growFactor;
      int ret = pool.Length;
      channel.Log("Pool too small, resizing from {0} to {1}", ret, newSize);
      Array.Resize(ref pool, newSize);
      growPool(ret, newSize);
      return alloc(ret);
    }

    return default(T);
  }

  public void free(T obj) {
    for (int i = 0; i < pool.Length; ++i) {
      if (pool[i].obj == obj) {
        Debug.Assert(pool[i].used);
        pool[i].obj.OnPoolRelease();
        pool[i].used = false;
        return;
      }
    }
    channel.Log("free could not find object: {0} in ObjectPool: {1}", obj, this);
  }
}
