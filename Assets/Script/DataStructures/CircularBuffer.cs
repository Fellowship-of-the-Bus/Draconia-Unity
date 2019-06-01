using System;
using System.Collections;
using System.Collections.Generic;

public class BufferOutOfBoundsException : Exception {
}

public class CircularBuffer<T> : ICollection<T>, IEnumerable<T> {
  public struct Enumerator : IEnumerator<T> {
    public Enumerator(CircularBuffer<T> buffer) {
      this.buffer = buffer;
      this.cur = 0;
      Reset();
    }
    private CircularBuffer<T> buffer;
    private int cur;
    public bool MoveNext() {
      cur = (cur+1) % buffer.elems.Length;
      return cur != buffer.end;
    }

    public T Current { get { return buffer.elems[cur]; } }
    object IEnumerator.Current { get { return Current; } }
    public void Reset() { cur = (buffer.begin-1+buffer.length)%buffer.length; }

    void IDisposable.Dispose() { }
  }

  private T[] elems;
  private int begin;
  private int end;
  private int length; // needed to distinguish full vs. empty

  public CircularBuffer(int size) {
    elems = new T[size];
    Clear();
  }

  public void Add(T elem) {
    AddLast(elem);
  }

  public void AddFirst(T elem) {
    if (length == elems.Length) throw new BufferOutOfBoundsException();
    elems[begin] = elem;
    begin = (begin-1+elems.Length)%elems.Length;
    ++length;
  }

  public void AddLast(T elem) {
    if (length == elems.Length) throw new BufferOutOfBoundsException();
    elems[end] = elem;
    end = (end+1)%elems.Length;
    ++length;
  }

  public bool Remove(T elem) {
    EqualityComparer<T> comparer = EqualityComparer<T>.Default;
    for (int i = begin; i < begin+Count; ++i) {
      if (comparer.Equals(elem, elems[i%elems.Length])) {
        if (i == begin) {
          RemoveFirst();
        } else {
          for (; i < begin+Count; ++i) {
            elems[i%elems.Length] = elems[(i+1)%elems.Length];
          }
          length--;
        }
        return true;
      }
    }
    return false;
  }

  public void RemoveFirst() {
    if (length == 0) throw new InvalidOperationException("RemoveFirst called on empty buffer");
    begin = (begin+1)%elems.Length;
    length--;
  }

  public void RemoveLast() {
    if (length == 0) throw new InvalidOperationException("RemoveLast called on empty buffer");
    end = (end-1+elems.Length)%elems.Length;
    length--;
  }

  public void Clear() {
    begin = 0;
    end = 0;
  }

  public bool Contains(T elem) {
    EqualityComparer<T> comparer = EqualityComparer<T>.Default;
    for (int i = begin; i < begin+Count; ++i) {
      if (comparer.Equals(elem, elems[i%elems.Length])) {
        return true;
      }
    }
    return false;
  }

  public void CopyTo(T[] array, Int32 arrayIndex) {
    for (int i = begin; i < begin+Count; ++i) {
      array[arrayIndex+i] = elems[i%elems.Length];
    }
  }

  public IEnumerator<T> GetEnumerator() {
    return new Enumerator(this);
  }

  IEnumerator IEnumerable.GetEnumerator() {
    return new Enumerator(this);
  }

  public int Count { get { return length; } }

  public T First { get { return elems[begin]; } }
  public T Last { get { return elems[(begin+end-1)%elems.Length]; } }

  public bool IsReadOnly { get { return false; } }
}
