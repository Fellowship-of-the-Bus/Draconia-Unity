using System.Collections.Generic;

public class ActionQueue {
  public interface Elem {
    float curAction { get; set; }
    float calcMoveTime(float time, int turns = 1);
  }

  private LinkedList<ActionTime> queue = new LinkedList<ActionTime>();
  private List<Elem> pieces = new List<Elem>();
  private float curTime = 0;

  private struct ActionTime {
    public Elem piece;

    public float time;

    public ActionTime(Elem p, float t) {
      piece = p;
      time = t;
    }
  }

  public float peekNext() {
    return queue.First.Value.time;
  }

  public Elem getNext() {
    Elem next = queue.First.Value.piece;
    float newTime = queue.First.Value.time;
    float timePassed = newTime - curTime;

    // //update action bars for all characters
    // foreach (Elem character in pieces) {
    //   character.updateActionBar(timePassed);
    // }

    next.curAction = 0f;
    curTime = newTime;
    return next;
  }

  public void endTurn() {
  	// TODO: Fix crash here when everyone dies
    Elem SelectedCharacter = queue.First.Value.piece;
    removeFirst(SelectedCharacter);

    if (!hasObject(SelectedCharacter)) {
      bool last = enqueue(SelectedCharacter);
      if (last) {
        foreach (Elem p in pieces) {
          if (p != SelectedCharacter) {
            fillActions(p);
          }
        }
      }
    }
  }

  private bool hasObject (Elem piece) {
    foreach (LinkedListNode<ActionTime> n in new NodeIterator<ActionTime>(queue)) {
      if (n.Value.piece == piece) {
        return true;
      }
    }
    return false;
  }

  private int actionsCount (Elem piece) {
    int count = 0;
    foreach (LinkedListNode<ActionTime> n in new NodeIterator<ActionTime>(queue)) {
      if (n.Value.piece == piece) {
        count++;
      }
    }
    return count;
  }

  public void updateTime(Elem piece) {
    if (!hasObject(piece)) {
      return;
    }
    remove(piece);
    add(piece);
  }

  public void add(Elem piece) {
    int i = 1;
    bool last = enqueue(piece);
    if (last) {
      foreach (Elem p in pieces) {
        fillActions(p);
      }
    }
    while (!last) {
      i++;
      last = enqueue(piece, i);
    }

    pieces.Add(piece);
  }

  private void fillActions(Elem piece) {
    int k = actionsCount(piece) + 1;
    bool filledIn = false;
    while (!filledIn) {
      filledIn = enqueue(piece, k);
      k++;
    }
  }

  public void remove(Elem piece) {
    int i = 0;
    List<LinkedListNode<ActionTime>> toRemove = new List<LinkedListNode<ActionTime>>();

    pieces.Remove(piece);
    foreach (LinkedListNode<ActionTime> n in new NodeIterator<ActionTime>(queue)) {
      if (n.Value.piece.Equals(piece)) {
        // GameManager.get.StartCoroutine(SlideButton(n.Value.button, buttonWidth, 0, true));
        toRemove.Add(n);
        // moveUp(i - 1);
      }
      i++;
    }

    foreach (LinkedListNode<ActionTime> n in toRemove) {
      queue.Remove(n);
    }
  }

  private void removeFirst(Elem piece) {
    int i = 0;

    foreach (LinkedListNode<ActionTime> n in new NodeIterator<ActionTime>(queue)) {
      if (n.Value.piece.Equals(piece)) {
        // GameManager.get.StartCoroutine(SlideButton(n.Value.button, buttonWidth, 0, true));
        queue.Remove(n);
        // moveUp(i - 1);
        break;
      }
      i++;
    }
  }

  // Add a turn marker to the action queue
  // Does not add to the end unless it is the only turn marker for that piece
  // Returns whether the requested marker belongs at the end of the queue
  private bool enqueue(Elem piece, int turn = 1) {
    bool isLast = false;
    // ActionBarElem buttonObject = null;

    int i = 0;
    float newTime = piece.calcMoveTime(curTime, turn);
    foreach (LinkedListNode<ActionTime> n in new NodeIterator<ActionTime>(queue)) {
      float time = n.Value.time;
      if (time > newTime) {
        // buttonObject = makeButton(piece);
        queue.AddBefore(n, new ActionTime(piece,  newTime));
        break;
      }
      i++;
    }

    if (i == queue.Count) {
      isLast = true;
      if (turn == 1) {
        // buttonObject = makeButton(piece);
        queue.AddLast(new ActionTime(piece, newTime));
      }
    }

    // float posn = getPosn(i);
    // moveDown(i);

    // if (buttonObject != null) {
    //   buttonObject.text.text = piece.name;
    //   buttonObject.transform.localPosition += new Vector3(0, posn, 0);
    // }

    return isLast;
  }
}

