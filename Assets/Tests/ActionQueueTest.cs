using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests {
  public class ActionQueueTest {
    private static int gcd(int a, int b) { 
      if (a == 0) return b;  
      return gcd(b % a, a);  
    } 

    private static int lcm(int a, int b) { 
      return a / gcd(a, b) * b; 
    }
    
    private static void swap<T>(ref T a, ref T b) {
      T tmp = a;
      a = b;
      b = tmp;
    }

    public class Elem : ActionQueue.Elem {
      private const float maxAction = 1000f;
      public float speed;

      public Elem(float speed) {
        this.speed = Elem.maxAction/speed;
      }

      public float curAction {
        get; set;
      }

      public float calcMoveTime(float time, int turns = 1) {
        Debug.Assert(speed > 0);
        return time + ((maxAction - curAction) / speed) + ((turns - 1) * (maxAction / speed));
      }

      public override string ToString() {
        return string.Format("Elem({0})", speed);
      }
    }

    private class TestHelper {
      private ActionQueue queue;
      private Elem[] elems;
      private float currentTime = 0;

      public TestHelper(ActionQueue queue, Elem[] elems) {
        this.queue = queue;
        this.elems = elems;
      }

      public void iterate(int i, int idx, int k) {
        float nextTime = queue.peekNext();
        Elem nextElem = (Elem)queue.getNext();
        Assert.GreaterOrEqual(nextTime, currentTime, 
          "Iteration {0}x{1}x{2}: Current Time < Next Time", i, idx, k);
        Assert.AreEqual(nextElem, elems[idx], 
          "{0} -> {1} Iteration {2}x{3} Expect index {4}, got {5}", 
          currentTime, nextTime, i, k, idx, Array.IndexOf(elems, nextElem));
        currentTime = nextTime;
        queue.endTurn();
      }

      public void loop(int start, int end, int k) {
        for (int i = start; i < end; ++i) {
          for (int idx = 0; idx < elems.Length; ++idx) {
            iterate(i, idx, k);
          }
        }
      }
    }

    // A Test behaves as an ordinary method
    [Test]
    public void ExpectTime() {
      const int timeInterval = 10;
      Elem[] elems = new Elem[] { new Elem(timeInterval) };
      ActionQueue queue = new ActionQueue();
      for (int i = 0; i < elems.Length; ++i) {
        queue.add(elems[i]);
      }
      TestHelper helper = new TestHelper(queue, elems);
      for (int i = 0; i < 1000; ++i) {
        float time = queue.peekNext();
        Assert.AreEqual(time, (i+1)*timeInterval, "Iteration {0}", i);
        helper.iterate(i, 0, 0);
      }
    }

    [Test]
    public void SimpleSameValueLoop() {
      Elem[] elems = new Elem[] { 
        new Elem(100),
        new Elem(100),
        new Elem(101),
        new Elem(99),
      };
      ActionQueue queue = new ActionQueue();
      for (int i = 0; i < elems.Length; ++i) {
        queue.add(elems[i]);
      }
      TestHelper helper = new TestHelper(queue, elems);
      float currentTime = 0;
      int toggle = 0;
      for (int idx = 0; idx < 30000; ++idx) {
        float nextTime = queue.peekNext();
        Elem nextElem = (Elem)queue.getNext();
        Assert.GreaterOrEqual(nextTime, currentTime, 
          "Iteration {0}: Current Time < Next Time", idx);
        if (nextElem != elems[2] && nextElem != elems[3]) {
          Assert.AreEqual(nextElem, elems[toggle % 2], 
            "{0} -> {1} Iteration {2} Expect index {3}, got {4}", 
            currentTime, nextTime, toggle%2, idx, Array.IndexOf(elems, nextElem));
          ++toggle;
        }
        currentTime = nextTime;
        queue.endTurn();
      }
    }

    [Test]
    public void SimpleCrossoverLoop() {
      Elem[] elems = new Elem[] { 
        new Elem(1),
        new Elem(5)
      };
      ActionQueue queue = new ActionQueue();
      for (int i = 0; i < elems.Length; ++i) {
        queue.add(elems[i]);
      }
      TestHelper helper = new TestHelper(queue, elems);
      float currentTime = 0;
      for (int i = 0; i < 10000; ++i) {
        for (int k = 0; k < 5; ++k) {
          helper.iterate(i, 0, k);
        }
        helper.iterate(i, 1, 0);
      }
    }

    [Test]
    public void LoopThreeElements() {
      // This set will cycle in order for N_1 iterations,
      // with the N_1+1st iteration having the first element
      // followed by a new cycle ordered B A C until N_2
      // followed by A B A C B
      // then the cycle repeats from the beginning

      // occur back to back
      const int X = 11; // must be odd
      const int A = X-2;
      const int B = X-1;
      const int C = X;

      int N_1 = A/(C-A);
      int N_2 = A/(B-A);

      // loops through these times, etc.
      //  9 18 27 36 45 54 63 72 81
      // 10 20 30 40 50 60 70 80 90
      // 11 22 33 44 55 66 77 88 99
      Elem[] elems = new Elem[3] {
        new Elem(A), new Elem(B), new Elem(C)
      };

      ActionQueue queue = new ActionQueue();
      for (int i = 0; i < elems.Length; ++i) {
        queue.add(elems[i]);
      }

      TestHelper helper = new TestHelper(queue, elems);

      for (int k = 0; k < 10; ++k) { // should loop perfectly forever
        helper.loop(0, N_1, k);
        helper.iterate(N_1, 0, k);
        // order changes here from A B C to B A C
        swap(ref elems[0], ref elems[1]);
        helper.loop(N_1, N_2, k);
        swap(ref elems[0], ref elems[1]);
        helper.iterate(N_2+1, 0, k);
        helper.iterate(N_2+2, 1, k);
        helper.iterate(N_2+3, 0, k);
        helper.iterate(N_2+4, 2, k);
        helper.iterate(N_2+5, 1, k);
        // order changes back to A B C
        
        ++k;
        helper.loop(0, N_1, k);
        helper.iterate(N_1, 0, k);
        helper.iterate(N_1, 1, k);
        // order changes here from A B C to C A B
        swap(ref elems[0], ref elems[1]);
        swap(ref elems[0], ref elems[2]);
        helper.loop(N_1, N_2, k);
        swap(ref elems[0], ref elems[1]);
        swap(ref elems[0], ref elems[2]);
        helper.iterate(N_2+1, 0, k);
        helper.iterate(N_2+2, 1, k);
        helper.iterate(N_2+3, 0, k);
        helper.iterate(N_2+4, 2, k);
        helper.iterate(N_2+5, 1, k);
        // order changes back to A B C
      }
    }
  }
}
