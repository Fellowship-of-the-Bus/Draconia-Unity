using System;
using System.Collections;
using System.Collections.Generic;

// Simple array enumerator that doesn't allocate
public struct ArrayEnumerator<T> : IEnumerable<T>, IEnumerator<T> {
  private T[] array;
  private int cursor;

  public ArrayEnumerator(T[] array) {
    this.array = array;
    this.cursor = -1;
  }

  public bool MoveNext() {
    cursor += 1;
    return cursor < array.Length;
  }

  public T Current { get { return array[cursor]; } }
  object IEnumerator.Current { get { return Current; } }
  public void Reset() { cursor = -1; }

  void IDisposable.Dispose() {}

  IEnumerator<T> IEnumerable<T>.GetEnumerator() { return this; }
  public ArrayEnumerator<T> GetEnumerator() { return this; }
  IEnumerator IEnumerable.GetEnumerator() { return this; }
}
