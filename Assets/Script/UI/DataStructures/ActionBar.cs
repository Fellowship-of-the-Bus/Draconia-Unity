using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

public class ActionBar : MonoBehaviour {
  public GameObject turnButton;

  private LinkedList<ActionTime> queue = new LinkedList<ActionTime>();
  private List<BattleCharacter> pieces = new List<BattleCharacter>();
  private float curTime = 0;

  private float buttonWidth = 0;
  private float buttonHeight = 0;

  private struct ActionTime {
    public BattleCharacter piece;
    public ActionBarElem button;

    public float time;

    public ActionTime(BattleCharacter p, ActionBarElem b, float t) {
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
    if (queue.Count == 0) {
      return Mathf.Infinity;
    }

    return queue.First.Value.time;
  }

  public BattleCharacter getNext() {
    BattleCharacter next = queue.First.Value.piece;
    float newTime = queue.First.Value.time;
    float timePassed = newTime - curTime;

    //update action bars for all characters
    foreach (BattleCharacter character in pieces) {
      character.updateActionBar(timePassed);
    }

    next.curAction = 0f;
    curTime = newTime;
    return next;
  }

  public void endTurn() {
    if (queue.Count == 0) {
      return;
    }

    BattleCharacter SelectedCharacter = queue.First.Value.piece;
    removeFirst(SelectedCharacter);

    if (!hasObject(SelectedCharacter)) {
      bool last = enqueue(SelectedCharacter);
      if (last) {
        foreach (BattleCharacter p in pieces) {
          if (p != SelectedCharacter) {
            fillActions(p);
          }
        }
      }
    }
  }

  private bool hasObject (BattleCharacter piece) {
    foreach (LinkedListNode<ActionTime> n in new NodeIterator<ActionTime>(queue)) {
      if (n.Value.piece == piece) {
        return true;
      }
    }
    return false;
  }

  private int actionsCount (BattleCharacter piece) {
    int count = 0;
    foreach (LinkedListNode<ActionTime> n in new NodeIterator<ActionTime>(queue)) {
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

  private void fillActions(BattleCharacter piece) {
    int k = actionsCount(piece) + 1;
    bool filledIn = false;
    while (!filledIn) {
      filledIn = enqueue(piece, k);
      k++;
    }
  }

  public void remove(BattleCharacter piece) {
    int i = 0;
    List<LinkedListNode<ActionTime>> toRemove = new List<LinkedListNode<ActionTime>>();

    pieces.Remove(piece);
    foreach (LinkedListNode<ActionTime> n in new NodeIterator<ActionTime>(queue)) {
      if (n.Value.piece.Equals(piece)) {
        GameManager.get.StartCoroutine(SlideButton(n.Value.button, buttonWidth, 0, true));
        toRemove.Add(n);
        moveUp(i - 1);
      }
      i++;
    }

    foreach (LinkedListNode<ActionTime> n in toRemove) {
      queue.Remove(n);
    }
  }

  public void highlight(BattleCharacter piece) {
    foreach (LinkedListNode<ActionTime> n in new NodeIterator<ActionTime>(queue)) {
      Image buttonImg = n.Value.button.image;

      if (piece == null || n.Value.piece.Equals(piece)) {
        buttonImg.color = Color.white;
      } else {
        buttonImg.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
      }
    }
  }

  private void removeFirst(BattleCharacter piece) {
    int i = 0;

    foreach (LinkedListNode<ActionTime> n in new NodeIterator<ActionTime>(queue)) {
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
  private bool enqueue(BattleCharacter piece, int turn = 1) {
    bool isLast = false;
    ActionBarElem buttonObject = null;

    int i = 0;
    float newTime = piece.calcMoveTime(curTime, turn);
    foreach (LinkedListNode<ActionTime> n in new NodeIterator<ActionTime>(queue)) {
      float time = n.Value.time;
      if (time > newTime) {
        buttonObject = makeButton(piece);
        queue.AddBefore(n, new ActionTime(piece, buttonObject, newTime));
        break;
      }
      i++;
    }

    if (i == queue.Count) {
      isLast = true;
      if (turn == 1) {
        buttonObject = makeButton(piece);
        queue.AddLast(new ActionTime(piece, buttonObject, newTime));
      }
    }

    float posn = getPosn(i);
    moveDown(i);

    if (buttonObject != null) {
      buttonObject.text.text = piece.name;
      buttonObject.transform.localPosition += new Vector3(0, posn, 0);
    }

    return isLast;
  }

  private ActionBarElem makeButton(BattleCharacter piece) {
    GameObject buttonObject = GameObject.Instantiate(turnButton, new Vector3 (0,0,0), Quaternion.identity) as GameObject;
    buttonObject.transform.SetParent(gameObject.transform, false);
    ActionBarElem elem = buttonObject.GetComponent<ActionBarElem>();
    Button button = elem.button;
    button.onClick.AddListener(delegate {
      GameManager.get.cam.panTo(piece.gameObject.transform.position);
    });
    elem.image.sprite = CharacterPortraitManager.getPortrait(piece);
    return elem;
  }

  private float getPosn(int i) {
    return -i * buttonHeight;
  }

  private void moveDown(int i) {
    int index = 0;
    foreach (LinkedListNode<ActionTime> n in new NodeIterator<ActionTime>(queue)) {
      if (index > i) {
        ActionBarElem o = n.Value.button;
        GameManager.get.StartCoroutine(SlideButton(o, 0, -buttonHeight));
      }
      index++;
    }
  }

  private void moveUp(int i) {
    int index = 0;
    foreach (LinkedListNode<ActionTime> n in new NodeIterator<ActionTime>(queue)) {
      if (index > i) {
        ActionBarElem o = n.Value.button;
        GameManager.get.StartCoroutine(SlideButton(o, 0, buttonHeight));
      }
      index++;
    }
  }

  private IEnumerator SlideButton(ActionBarElem button, float x, float y, bool deleteAfter = false) {
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
  public static ActionBar get { get; private set; }
}
