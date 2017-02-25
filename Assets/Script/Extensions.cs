using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public static class Extensions {
  public static IEnumerable<TSource> Take<TSource>(this IEnumerable<TSource> source, int count) {
    IEnumerator<TSource> it = source.GetEnumerator();
    for (int i = 0; i < count; ++i) {
      if (it.MoveNext()) {
        yield return it.Current;
      }
    }
  }

  public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector) {
    IEnumerator<TSource> it = source.GetEnumerator();
    while (it.MoveNext()) {
      yield return selector(it.Current);
    }
  }

  public static IEnumerable<TResult> Map<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> f) {
    return source.Select(f);
  }

  public static IEnumerable<TResult> Where<TResult> (this IEnumerable<TResult> source, Func<TResult, bool> f) {
    IEnumerator<TResult> it = source.GetEnumerator();
    while(it.MoveNext()) {
      if (f(it.Current)) {
        yield return it.Current;
      }
    }
  }
  public static IEnumerable<TResult> Filter<TResult> (this IEnumerable<TResult> source, Func<TResult, bool> f) {
    return source.Where(f);
  }

  public static void ForEach<T>(this IEnumerable<T> xs, Action<T> f) {
    foreach (var x in xs) {
      f(x);
    }
  }

  // switch to this instead of NodeIterator?
  public static IEnumerable<LinkedListNode<T>> nodeIterate<T>(this LinkedList<T> l) {
    for (LinkedListNode<T> n = l.First; n != null; n = n.Next) {
      yield return n;
    }    
  }

  // switch to this instead of Range?
  public static IEnumerable<int> range(int start, int finish, int step = 1, bool inclusive = false) {
    while (! ((start > finish && step > 0) || (start < finish && step < 0) || (start == finish && !inclusive))) {
      start += step;
      yield return start;
    }
  }

  public static ICollection<T> AddAll<T>(this ICollection<T> source, params T[] elems) {
    foreach (T x in elems) source.Add(x);
    return source;
  }

  public static List<T> toList<T>(this IEnumerable<T> source) { 
    return new List<T>(source);
  }

  public static IEnumerable<T> flatten<T>(this List<List<T>> source) {
    foreach (var x in source) {
      foreach (var y in x) {
        yield return y;
      }
    }
  }
}

