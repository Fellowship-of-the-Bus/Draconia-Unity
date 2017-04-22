using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;


//ideally we want constraint T to implement >
public class Heap<T> where T: IComparable<T> {
  LinkedList<T> heap;

  public Heap() {
    heap = new LinkedList<T>();
  }

  public T getMax() {
    if (heap.Count == 0) {
      return default(T);
    }
    return heap.First.Value;
  }

  public int Count {
    get { return heap.Count; }
  }

  public bool hasObject (T o) {
    foreach (LinkedListNode<T> n in new NodeIterator<T>(heap)) {
      if (n.Value.CompareTo(o) == 0) {
        return true;
      }
    }
    return false;
  }

  public void add(T o) {
    int i = 0;
    foreach (LinkedListNode<T> n in new NodeIterator<T>(heap)) {
      if (o.CompareTo(n.Value) > 0) {
        heap.AddBefore(n, o);
        break;
      }
      i++;
    }

    if (i == heap.Count) {
      heap.AddLast(o);
    }
  }

  public void remove(T o) {
    foreach (LinkedListNode<T> n in new NodeIterator<T>(heap)) {
      if (n.Value.CompareTo(o) == 0) {
        heap.Remove(n);
      }
    }
  }

  public void clear() {
    heap.Clear();
  }
}
