using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;               // for assert, to avoid unity dependencies
using System.Runtime.InteropServices;

namespace Draconia.Collections.StaticAlloc {
  internal unsafe struct Node {
    public Node * next;
    public Node * prev;
    // GCHandle is needed here because unsafe structs cannot use generic types
    // and because the element should not be touched by the garbage collector
    // while it's in a StaticAlloc.LinkedList
    public GCHandle elem;
  }

  [System.Serializable]
  public unsafe class LinkedList<T> : ICollection<T>, IEnumerable<T> {
    public struct Enumerator : IEnumerator<T> {
      public Enumerator(LinkedList<T> list) {
        this.list = list;
        this.node = null;
      }
      private LinkedList<T> list;
      private Node * node;
      public bool MoveNext() {
        node = node == null ? list.head : node->next;
        return node != null;
      }

      public T Current { get { return NodeToElem(node); } }
      object IEnumerator.Current { get { return Current; } }
      public void Reset() { node = list.head; }

      void IDisposable.Dispose() { }
    }

    private Node * head;
    private Node * tail;
    private Node * freeList;
    private int length;
    private PoolAllocator<Node> pool;

    public LinkedList(int size) {
      pool = new PoolAllocator<Node>(size);
    }

    public void Add(T elem) {
      AddLast(elem);
    }

    public void AddFirst(T elem) {
      Node * node = AllocNode(elem);
      if (head == node) return;
      head->prev = node;
      node->next = head;
      head = node;
    }

    public void AddLast(T elem) {
      Node * node = AllocNode(elem);
      if (tail == node) return;
      tail->next = node; 
      node->prev = tail;
      tail = node;
    }

    public bool Remove(T elem) {
      Node * node = FindNode(elem);
      if (node != null) {
        RemoveNode(node);
        return true;
      }
      return false;
    }

    public void RemoveFirst() {
      RemoveNode(head);
    }

    public void RemoveLast() {
      RemoveNode(tail);
    }

    public void Clear() {
      for (Node * n = head; n != null;) {
        Node * tmp = n;
        n = n->next;
        FreeNode(tmp);
      }
    }

    public bool Contains(T elem) {
      return FindNode(elem) != null;
    }

    public void CopyTo(T[] array, Int32 arrayIndex) {
      Node * cur = head;
      for (int i = arrayIndex; i < arrayIndex + length; ++i) {
        array[i] = NodeToElem(cur);
        cur = cur->next;
      }
    }

    public IEnumerator<T> GetEnumerator() {
      return new Enumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return new Enumerator(this);
    }

    public int Count { get { return length; } }

    public T First { get { return NodeToElem(head); } }
    public T Last { get { return NodeToElem(tail); } }

    public bool IsReadOnly { get { return false; } }


    private static T NodeToElem(Node * node) {
      return (T)node->elem.Target;
    }

    private Node * AllocNode(T elem) {
      Node * node = pool.Alloc();
      node->elem = GCHandle.Alloc(elem, GCHandleType.Pinned);
      node->next = null;
      node->prev = null;
      if (head == null) {
        Debug.Assert(tail == null);
        head = node;
        tail = node;
      }
      Debug.Assert(head != null && tail != null);
      length++;
      return node;
    }

    private void FreeNode(Node * node) {
      node->elem.Free();
      node->next = null;
      node->prev = null;
      pool.Free(node);
      length--;
    }

    private Node * FindNode(T elem) {
      for (Node * n = head; n != null; n = n->next) {
        if (NodeToElem(n).Equals(elem)) {
          return n;
        }
      }
      return null;
    }

    private void RemoveNode(Node * node) {
      Node * next = node->next;
      Node * prev = node->prev;
      if (next == null && prev == null) {
        // only node, list is now empty
        head = null;
        tail = null;
      } else if (next == null) {
        // last node
        tail = prev;
      } else if (prev == null) {
        // first node
        head = next;
      } else {
        // internal node
        next->prev = prev;
        prev->next = next;
      }
      FreeNode(node);
    }
  }
}
