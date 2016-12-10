using System;
using System.Collections;
using System.Collections.Generic;

public class Range : IEnumerable<int>, IEnumerator<int> {
  public readonly int start, finish, step;
  public readonly bool inclusive;
  private int cur;

  private bool isDone() {
    return (cur > finish && step > 0)
      || (cur < finish && step < 0)
      || (cur == finish && !inclusive);
  }

  public Range(int start, int finish, int step = 1, bool inclusive = false) {
    this.start = start;
    this.finish = finish;
    this.step = step;
    this.inclusive = inclusive;
    Reset();
  }

  public IEnumerator<int> GetEnumerator() { return this; }
  IEnumerator IEnumerable.GetEnumerator() { return this; }

  public bool MoveNext() {
    cur += step;
    if (isDone()) return false;
    return true;
  }

  public int Current { get { return cur; } }
  object IEnumerator.Current { get { return Current; } }
  public void Reset() { cur = start-step; }

  void IDisposable.Dispose() { }
};
