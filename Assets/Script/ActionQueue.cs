using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

public class ActionQueue {
  LinkedList<GameObject> queue;
  float curTime = 0;

  //public ActionQueue(GameObject actionBar, GameObject[] characters) {
  public ActionQueue() {
    queue = new LinkedList<GameObject>();
    /*foreach (GameObject o in characters) {
      float time = o.GetComponent<Character>().calcMoveTime(0f);
      enqueue(o);
    }*/
  }

  public GameObject getNext() {
    GameObject next = queue.First.Value;
    
    curTime = next.GetComponent<Character>().nextMoveTime;
    return next;
  }

  public void endTurn() {
    GameObject SelectedPiece = queue.First.Value;
    queue.RemoveFirst();
    Character SelectedCharacter = SelectedPiece.GetComponent<Character>();
    SelectedCharacter.calcMoveTime(curTime);
    enqueue(SelectedPiece);
  }

  public void updateTime(GameObject piece) {
    Character character = piece.GetComponent<Character>();
    character.calcMoveTime(curTime);
    remove(piece);
    enqueue(piece);
  }

  public void add(GameObject piece) {
    Character character = piece.GetComponent<Character>();
    character.calcMoveTime(curTime);
    enqueue(piece);
  }

  public void remove(GameObject piece) {
    queue.Remove(piece);
  }

  void enqueue(GameObject piece) {
    Character newCharacter = piece.GetComponent<Character>();

    foreach (LinkedListNode<GameObject> n in new NodeIterator<GameObject>(queue)) {
      GameObject o = n.Value;
      Character c = o.GetComponent<Character>();
      if (c.nextMoveTime > newCharacter.nextMoveTime) {
        queue.AddBefore(n, piece);
        return;
      }
    }

    queue.AddLast(piece);
  }
}
