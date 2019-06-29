using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

public class ActionQueue : MonoBehaviour {
  public GameObject turnButton;
  public CharacterPortraitManager portraitManager;

  private LinkedList<actionTime> queue = new LinkedList<actionTime>();
  private List<BattleCharacter> pieces = new List<BattleCharacter>();
  private float curTime = 0;

  private float buttonWidth = 0;
  private float buttonHeight = 0;
  public bool usePortraits = false;

  struct actionTime {
    public BattleCharacter piece;
    public ActionQueueElem button;

    public float time;

    public actionTime(BattleCharacter p, ActionQueueElem b, float t) {
      piece = p;
      button = b;
      time = t;
    }
  }

  void Awake() {
    get = this;
    RectTransform trans = turnButton.GetComponent<RectTransform>();
    buttonWidth = trans.rect.width;
    buttonHeight = trans.rect.height;
  }

  public float peekNext() {
    return queue.First.Value.time;
  }

  public BattleCharacter getNext() {
    BattleCharacter next = queue.First.Value.piece;
    float newTime = queue.First.Value.time;
    float timePassed = newTime - curTime;

    //update action bars for all characters
    foreach (LinkedListNode<actionTime> n in new NodeIterator<actionTime>(queue)) {
      n.Value.piece.updateActionBar(timePassed);
    }

    next.curAction = 0f;
    curTime = newTime;
    return next;
  }

  public void endTurn() {
  	// TODO: Fix crash here when everyone dies
    BattleCharacter SelectedPiece = queue.First.Value.piece;
    removeFirst(SelectedPiece);

    if (!hasObject(SelectedPiece)) {
      bool last = enqueue(SelectedPiece);
      if (last) {
        foreach (BattleCharacter p in pieces) {
          if (p != SelectedPiece) {
            fillActions(p);
          }
        }
      }
    }
  }

  public bool hasObject (BattleCharacter piece) {
    foreach (LinkedListNode<actionTime> n in new NodeIterator<actionTime>(queue)) {
      if (n.Value.piece == piece) {
        return true;
      }
    }
    return false;
  }

  int actionsCount (BattleCharacter piece) {
    int count = 0;
    foreach (LinkedListNode<actionTime> n in new NodeIterator<actionTime>(queue)) {
      if (n.Value.piece == piece) {
        count++;
      }
    }
    return count;
  }

  public void updateTime(BattleCharacter piece) {
    if (!hasObject(piece)) {
      return;
    }
    remove(piece);
    add(piece);
  }

  public void add(BattleCharacter piece) {
    int i = 1;
    bool last = enqueue(piece);
    if (last) {
      foreach (BattleCharacter p in pieces) {
        fillActions(p);
      }
    }
    while (!last) {
      i++;
      last = enqueue(piece, i);
    }

    pieces.Add(piece);
  }

  void fillActions(BattleCharacter piece) {
    int k = actionsCount(piece) + 1;
    bool filledIn = false;
    while (!filledIn) {
      filledIn = enqueue(piece, k);
      k++;
    }
  }

  public void remove(BattleCharacter piece) {
    int i = 0;
    List<LinkedListNode<actionTime>> toRemove = new List<LinkedListNode<actionTime>>();

    pieces.Remove(piece);
    foreach (LinkedListNode<actionTime> n in new NodeIterator<actionTime>(queue)) {
      if (n.Value.piece.Equals(piece)) {
        GameManager.get.StartCoroutine(SlideButton(n.Value.button, buttonWidth, 0, true));
        toRemove.Add(n);
        moveUp(i - 1);
      }
      i++;
    }

    foreach (LinkedListNode<actionTime> n in toRemove) {
      queue.Remove(n);
    }
  }

  public void highlight(BattleCharacter piece) {
    foreach (LinkedListNode<actionTime> n in new NodeIterator<actionTime>(queue)) {
      Image buttonImg = n.Value.button.image;

      if (piece == null || n.Value.piece.Equals(piece)) {
        buttonImg.color = Color.white;
      } else {
        buttonImg.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
      }
    }
  }

  public void removeFirst(BattleCharacter piece) {
    int i = 0;

    foreach (LinkedListNode<actionTime> n in new NodeIterator<actionTime>(queue)) {
      if (n.Value.piece.Equals(piece)) {
        GameManager.get.StartCoroutine(SlideButton(n.Value.button, buttonWidth, 0, true));
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
  bool enqueue(BattleCharacter piece, int turn = 1) {
    bool isLast = false;
    ActionQueueElem buttonObject = null;

    int i = 0;
    float newTime = piece.calcMoveTime(curTime, turn);
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
    if (piece.name == "") {
      piece.name = queue.Count.ToString();
    }

    if (buttonObject != null) {
      buttonObject.text.text = piece.name;
      buttonObject.transform.localPosition += new Vector3(0, posn, 0);
    }

    return isLast;
  }

  ActionQueueElem makeButton(BattleCharacter piece) {
    GameObject buttonObject = GameObject.Instantiate(turnButton, new Vector3 (0,0,0), Quaternion.identity) as GameObject;
    buttonObject.transform.SetParent(gameObject.transform, false);
    ActionQueueElem elem = buttonObject.GetComponent<ActionQueueElem>();
    Button button = elem.button;
    button.onClick.AddListener(delegate {
      GameManager.get.cam.panTo(piece.gameObject.transform.position);
    });
    if (usePortraits) {
      elem.image.sprite = CharacterPortraitManager.getPortrait(piece);
      elem.text.gameObject.SetActive(false);
    }
    return elem;
  }

  float getPosn(int i) {
    return -i * buttonHeight;
  }

  void moveDown(int i) {
    int index = 0;
    foreach (LinkedListNode<actionTime> n in new NodeIterator<actionTime>(queue)) {
      if (index > i) {
        ActionQueueElem o = n.Value.button;
        GameManager.get.StartCoroutine(SlideButton(o, 0, -buttonHeight));
      }
      index++;
    }
  }

  void moveUp(int i) {
    int index = 0;
    foreach (LinkedListNode<actionTime> n in new NodeIterator<actionTime>(queue)) {
      if (index > i) {
        ActionQueueElem o = n.Value.button;
        GameManager.get.StartCoroutine(SlideButton(o, 0, buttonHeight));
      }
      index++;
    }
  }

  IEnumerator SlideButton(ActionQueueElem button, float x, float y, bool deleteAfter = false) {
    const float time = 0.25f;


    GameManager.get.lockUI();

    Vector3 d = new Vector3(x, y, 0) / (Options.FPS * time);
    for (int i = 0; i < Options.FPS * time; i++) {
      button.transform.localPosition += d;
      yield return null;
    }

    if (deleteAfter) {
      GameObject.Destroy(button);
    }
    GameManager.get.unlockUI();
  }
  public static ActionQueue get { get; private set; }
}
