using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests {
  public class TestHelper {
    private int i = 0;
    public void Add(CircularBuffer<int> buffer, int x) {
      Assert.AreEqual(i, buffer.Count);
      buffer.Add(x);
      Assert.AreEqual(++i, buffer.Count);
    }

    public bool Remove(CircularBuffer<int> buffer, int x) {
      Assert.AreEqual(i, buffer.Count);
      if (! buffer.Remove(x)) return false;
      Assert.AreEqual(--i, buffer.Count);
      return true;
    }
  }

  public class CircularBufferTest {

    // A Test behaves as an ordinary method
    [Test]
    public void AddRemove() {
      // index:  0 1 2 3 4 5 6 7
      // array:  4 5 _ _ _ 1 2 3
      CircularBuffer<int> buffer = new CircularBuffer<int>(8);
      TestHelper helper = new TestHelper();

      // add 0s to create a hole
      for (int i = 0; i < 5; ++i) {
        helper.Add(buffer, 0);
      }

      helper.Add(buffer, 1);
      helper.Add(buffer, 2);
      helper.Add(buffer, 3);

      // remove 0s to create a hole
      while (true){
        if (! helper.Remove(buffer, 0)) break;
      }

      helper.Add(buffer, 4);
      helper.Add(buffer, 5);

      int idx = 0;
      foreach (int x in buffer) {
        Assert.AreEqual(++idx, x);
      }
    }
  }
}
