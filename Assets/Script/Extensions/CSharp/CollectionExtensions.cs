using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public static class CollectionExtensions {
  public static IEnumerable<TSource> Take<TSource>(this IEnumerable<TSource> source, int count) {
    IEnumerator<TSource> it = source.GetEnumerator();
    for (int i = 0; i < count; ++i) {
      if (it.MoveNext()) {
        yield return it.Current;
      }
    }
  }

  public static IEnumerable<TResult> Map<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> f) {
    return source.Select(f);
  }

  public static IEnumerable<TResult> Filter<TResult> (this IEnumerable<TResult> source, Func<TResult, bool> f) {
    return source.Where(f);
  }

  public static void ForEach<T>(this IEnumerable<T> xs, Action<T> f) {
    foreach (var x in xs) {
      f(x);
    }
  }

  public static T Find<T>(this IEnumerable<T> xs, Func<T, bool> pred) {
    foreach (var x in xs) {
      if (pred(x)) {
        return x;
      }
    }
    return default(T);
  }

  // switch to this instead of NodeIterator?
  public static IEnumerable<LinkedListNode<T>> nodeIterate<T>(this LinkedList<T> l) {
    for (LinkedListNode<T> n = l.First; n != null; n = n.Next) {
      yield return n;
    }
  }

  public static ICollection<T> AddAll<T>(this ICollection<T> source, params T[] elems) {
    foreach (T x in elems) source.Add(x);
    return source;
  }

  public static bool IsEmpty<T>(this ICollection<T> source) {
    return source.Count == 0;
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

  public static Dictionary<TKey, TVal> toDictionary<TKey,TVal>(this IEnumerable<IGrouping<TKey, TVal>> x) {
    return (Dictionary<TKey,TVal>)x;
  }

  public static TVal safeGet<TKey, TVal>(this IDictionary<TKey, TVal> x, TKey key) where TVal : new() {
    TVal value;
    if (! x.TryGetValue(key, out value)) {
      return new TVal();
    }
    return value;
  }

  public static string replaceAll(this string str, string oldChars, char newChar) {
    System.Text.StringBuilder sb = new System.Text.StringBuilder();
    for (int i = 0; i < str.Length; ++i)  {
      sb.Append(oldChars.Contains(str[i]) ? newChar : str[i]);
    }
    return sb.ToString();
  }
}
