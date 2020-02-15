using System;
using System.Collections.Generic;
using System.Collections;

public class NodeIterator<T> : IEnumerable<LinkedListNode<T>>, IEnumerator<LinkedListNode<T>> {
  LinkedList<T> list;
  LinkedListNode<T> cur;

  public NodeIterator(LinkedList<T> l) {
    list = l;
  }

  public IEnumerator<LinkedListNode<T>> GetEnumerator() { return this; }
  IEnumerator IEnumerable.GetEnumerator() { return this; }

  bool start = true;
  public bool MoveNext() {
    if (start) {
      start = false;
      cur = list.First;
    } else if (cur != null) cur = cur.Next;
    return cur != null;
  }

  public LinkedListNode<T> Current { get { return cur; } }
  object IEnumerator.Current { get { return Current; } }
  public void Reset() { start = true; }

  void IDisposable.Dispose() { }
}
