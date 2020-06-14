using System;
using System.Collections.Generic;

public static class MiscExtensions {
  public static string displayName(this Type t) {
    return t.FullName;
  }

  public static IEnumerable<T> GetValues<T>(this T x) where T : IConvertible {
    return (T[])Enum.GetValues(typeof(T));
  }

  // switch to this instead of Range?
  public static IEnumerable<int> range(int start, int finish, int step = 1, bool inclusive = false) {
    while (! ((start > finish && step > 0) || (start < finish && step < 0) || (start == finish && !inclusive))) {
      start += step;
      yield return start;
    }
  }
}
