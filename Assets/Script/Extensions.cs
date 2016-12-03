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
    IEnumerator<TResult> it =source.GetEnumerator();
    while(it.MoveNext()) {
      if (f(it.Current)) {
        yield return it.Current;
      }
    }
  }
  public static IEnumerable<TResult> Filter<TResult> (this IEnumerable<TResult> source, Func<TResult, bool> f) {
    return source.Where(f);
  }

}

