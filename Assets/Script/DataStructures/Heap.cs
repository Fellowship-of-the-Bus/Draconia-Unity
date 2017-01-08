using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;


//ideally we want constraint T to implement >
public class Heap<T> where T: Effect {
  LinkedList<T> heap;

  public Heap() {
    heap = new LinkedList<T>();
  }

  public T getMax() {
    if (heap.Count == 0) {
      return null;
    }
    return heap.First.Value;
  }

  public int Count {
    get { return heap.Count; }
    private set {}
  }

  public bool hasObject (T o) {
    foreach (LinkedListNode<T> n in new NodeIterator<T>(heap)) {
      if (n.Value == o) {
        return true;
      }
    }
    return false;
  }

  public void add(T o) {
    int i = 0;
    foreach (LinkedListNode<T> n in new NodeIterator<T>(heap)) {
      if (o > n.Value) {
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
      if (n.Value == o) {
        heap.Remove(n);
      }
    }
  }

}
