using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

public class ActionQueue {
  GameObject turnButton;
  GameObject actionBar;
  LinkedList<actionTime> queue;
  List<GameObject> pieces;
  private GameManager gameManager;
  float curTime = 0;

  float buttonWidth = 0;
  float buttonHeight = 0;

  struct actionTime {
    public GameObject piece;
    public GameObject button;
    public float time;

    public actionTime(GameObject p, GameObject b, float t) {
      piece = p;
      button = b;
      time = t;
    }
  }

  public ActionQueue(GameObject bar, GameObject buttonPrefab, GameManager game) {
    get = this;
    pieces = new List<GameObject>();
    queue = new LinkedList<actionTime>();
    turnButton = buttonPrefab;
    actionBar = bar;
    gameManager = game;
    buttonWidth = turnButton.GetComponent<RectTransform>().rect.width;
    buttonHeight = turnButton.GetComponent<RectTransform>().rect.height;
  }

  public GameObject getNext() {
    GameObject next = queue.First.Value.piece;
    float newTime = queue.First.Value.time;
    float timePassed = newTime - curTime;

    //update action bars for all characters
    foreach (LinkedListNode<actionTime> n in new NodeIterator<actionTime>(queue)) {
      n.Value.piece.GetComponent<Character>().updateActionBar(timePassed);
    }

    next.GetComponent<Character>().curAction = 0f;
    curTime = newTime;
    return next;
  }

  public void endTurn() {
    GameObject SelectedPiece = queue.First.Value.piece;
    removeFirst(SelectedPiece);

    if (!hasObject(SelectedPiece)) {
      bool last = enqueue(SelectedPiece);
      if (last) {
        foreach (GameObject p in pieces) {
          if (p != SelectedPiece) {
            fillActions(p);
          }
        }
      }
    }
  }

  public bool hasObject (GameObject piece) {
    foreach (LinkedListNode<actionTime> n in new NodeIterator<actionTime>(queue)) {
      if (n.Value.piece == piece) {
        return true;
      }
    }
    return false;
  }

  int actionsCount (GameObject piece) {
    int count = 0;
    foreach (LinkedListNode<actionTime> n in new NodeIterator<actionTime>(queue)) {
      if (n.Value.piece == piece) {
        count++;
      }
    }
    return count;
  }

  public void updateTime(GameObject piece) {
    if (!hasObject(piece)) {
      return;
    }
    remove(piece);
    add(piece);
  }

  public void add(GameObject piece) {
    int i = 1;
    bool last = enqueue(piece);
    if (last) {
      foreach (GameObject p in pieces) {
        fillActions(p);
      }
    }
    while (!last) {
      i++;
      last = enqueue(piece, i);
    }

    pieces.Add(piece);
  }

  void fillActions(GameObject piece) {
    int k = actionsCount(piece) + 1;
    bool filledIn = false;
    while (!filledIn) {
      filledIn = enqueue(piece, k);
      k++;
    }
  }

  public void remove(GameObject piece) {
    int i = 0;
    List<LinkedListNode<actionTime>> toRemove = new List<LinkedListNode<actionTime>>();

    pieces.Remove(piece);
    foreach (LinkedListNode<actionTime> n in new NodeIterator<actionTime>(queue)) {
      if (n.Value.piece.Equals(piece)) {
        gameManager.StartCoroutine(SlideButton(n.Value.button, buttonWidth, 0, true));
        toRemove.Add(n);
        moveUp(i - 1);
      }
      i++;
    }

    foreach (LinkedListNode<actionTime> n in toRemove) {
      queue.Remove(n);
    }
  }

  public void removeFirst(GameObject piece) {
    int i = 0;

    foreach (LinkedListNode<actionTime> n in new NodeIterator<actionTime>(queue)) {
      if (n.Value.piece.Equals(piece)) {
        gameManager.StartCoroutine(SlideButton(n.Value.button, buttonWidth, 0, true));
        queue.Remove(n);
        moveUp(i - 1);
        break;
      }
      i++;
    }
  }

  // Add a turn marker to the action queue
  // Does not add to the end unless it is the only turn marker for that piece
  // Returns whether the requested marker belongs at the end of the queue
  bool enqueue(GameObject piece, int turn = 1) {
    bool isLast = false;
    Character newCharacter = piece.GetComponent<Character>();
    GameObject buttonObject = null;

    int i = 0;
    float newTime = newCharacter.calcMoveTime(curTime, turn);
    foreach (LinkedListNode<actionTime> n in new NodeIterator<actionTime>(queue)) {
      float time = n.Value.time;
      if (time > newTime) {
        buttonObject = makeButton(piece);
        queue.AddBefore(n, new actionTime(piece, buttonObject, newTime));
        break;
      }
      i++;
    }

    if (i == queue.Count) {
      isLast = true;
      if (turn == 1) {
        buttonObject = makeButton(piece);
        queue.AddLast(new actionTime(piece, buttonObject, newTime));
      }
    }

    float posn = getPosn(i);
    moveDown(i);
    if (newCharacter.name == "") {
      newCharacter.name = queue.Count.ToString();
    }

    if (buttonObject != null) {
      buttonObject.GetComponentsInChildren<Text>()[0].text = newCharacter.name;
      buttonObject.transform.Translate(new Vector3(0, posn, 0));
      gameManager.StartCoroutine(SlideButton(buttonObject, -buttonWidth, 0));
    }

    return isLast;
  }

  GameObject makeButton(GameObject piece) {
    GameObject buttonObject = GameObject.Instantiate(turnButton, new Vector3 (-25 + buttonWidth,0,0), Quaternion.identity) as GameObject;
    buttonObject.transform.SetParent(actionBar.transform, false);
    Button button = buttonObject.GetComponent<Button>();
    button.onClick.AddListener(delegate {
      GameManager.get.cam.panTo(piece.transform.position);
    });

    return buttonObject;
  }

  float getPosn(int i) {
    float bottom = actionBar.GetComponent<RectTransform>().rect.height;
    return (bottom / 2) - ((i + 0.5f) * buttonHeight);
  }

  void moveDown(int i) {
    int index = 0;
    foreach (LinkedListNode<actionTime> n in new NodeIterator<actionTime>(queue)) {
      if (index > i) {
        GameObject o = n.Value.button;
        gameManager.StartCoroutine(SlideButton(o, 0, -buttonHeight));
      }
      index++;
    }
  }

  void moveUp(int i) {
    int index = 0;
    foreach (LinkedListNode<actionTime> n in new NodeIterator<actionTime>(queue)) {
      if (index > i) {
        GameObject o = n.Value.button;
        gameManager.StartCoroutine(SlideButton(o, 0, buttonHeight));
      }
      index++;
    }
  }

  IEnumerator SlideButton(GameObject button, float x, float y, bool deleteAfter = false) {
    const float FPS = 60f;
    const float time = 0.25f;

    gameManager.lockUI();

    Vector3 d = new Vector3(x, y, 0) / (FPS * time);
    for (int i = 0; i < FPS * time; i++) {
      button.transform.Translate(d);
      yield return new WaitForSeconds(1/FPS);
    }

    if (deleteAfter) {
      GameObject.Destroy(button);
    }
    gameManager.unlockUI();
  }
  public static ActionQueue get { get; private set; }
}
