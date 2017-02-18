using System;

public class Pair<T, U> {
  public Pair(T first, U second) {
    this.first = first;
    this.second = second;
  }

  public T first { get; set; }
  public U second { get; set; }
};

public class Pair {
  public static Pair<T, U> create<T, U>(T first, U second) {
    return new Pair<T, U>(first, second);
  }
}
