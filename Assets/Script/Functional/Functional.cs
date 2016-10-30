using System;
using System.Collections.Generic;

public static class Functional {
  public static void ForEach<T>(this IEnumerable<T> xs, Action<T> f) {
    foreach (var x in xs) {
      f(x);
    }
  }
}
