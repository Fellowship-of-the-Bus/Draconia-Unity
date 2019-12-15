using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Draconia.Collections.StaticAlloc;

namespace Tests {
  public class LinkedListTestHelper {
    private int i = 0;
    public void Add(LinkedList<int> list, int x) {
      Assert.AreEqual(i, list.Count);
      list.Add(x);
      Assert.AreEqual(++i, list.Count);
    }

    public void AddFirst(LinkedList<int> list, int x) {
      Assert.AreEqual(i, list.Count);
      list.AddFirst(x);
      Assert.AreEqual(++i, list.Count);
    }

    public bool Remove(LinkedList<int> list, int x) {
      Assert.AreEqual(i, list.Count);
      if (! list.Remove(x)) return false;
      Assert.AreEqual(--i, list.Count);
      return true;
    }
  }

  public class LinkedListTest {
    [Test]
    public void AddSingle() {
      LinkedList<int> list = new LinkedList<int>(8);
      LinkedListTestHelper helper = new LinkedListTestHelper();

      helper.Add(list, 1);
      Assert.AreEqual(list.Count, 1);

      Assert.AreEqual(list.Contains(1), true);
      Assert.AreEqual(list.Contains(2), false);

      int idx = 0;
      foreach (int x in list) {
        Assert.AreEqual(++idx, x);
      }
    }

    [Test]
    public void Add() {
      LinkedList<int> list = new LinkedList<int>(8);
      LinkedListTestHelper helper = new LinkedListTestHelper();

      helper.Add(list, 1);
      helper.Add(list, 2);
      helper.Add(list, 3);
      helper.Add(list, 4);
      helper.Add(list, 5);

      int idx = 0;
      foreach (int x in list) {
        Debug.Log(idx);
        Assert.AreEqual(++idx, x);
      }
    }

    [Test]
    public void AddFirst() {
      LinkedList<int> list = new LinkedList<int>(8);
      LinkedListTestHelper helper = new LinkedListTestHelper();

      list.AddFirst(1);
      list.AddFirst(2);
      list.AddFirst(3);
      list.AddFirst(4);
      list.AddFirst(5);
      
      int num = 5;
      foreach (int x in list) {
        Assert.AreEqual(num--, x);
      }
    }

    // A Test behaves as an ordinary method
    [Test]
    public void AddRemove() {
      LinkedList<int> list = new LinkedList<int>(8);
      LinkedListTestHelper helper = new LinkedListTestHelper();

      helper.Add(list, 1);
      helper.Add(list, 2);
      helper.Add(list, 3);
      helper.Add(list, 4);
      helper.Add(list, 5);
      list.Remove(3);

      int idx = 0;
      foreach (int x in list) {
        if (idx == 2) ++idx;
        Assert.AreEqual(++idx, x);
      }
    }

    [Test]
    public void AddToFullList() {
      LinkedList<int> list = new LinkedList<int>(8);
      LinkedListTestHelper helper = new LinkedListTestHelper();
      for (int i = 0; i < 9; i++) {
        list.Add(i);
      }
    }
  }
}
