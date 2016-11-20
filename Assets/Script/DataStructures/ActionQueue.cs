using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

public class ActionQueue {
  GameObject turnButton;
  GameObject actionBar;
  LinkedList<actionTime> queue;
  private GameManager gameManager;
  float curTime = 0;

  float buttonWidth = 0;
  float buttonHeight = 0;

  struct actionTime {
    public GameObject piece;
    public GameObject button;

    public actionTime(GameObject p, GameObject b) {
      piece = p;
      button = b;
    }
  }

  public ActionQueue(GameObject bar, GameObject buttonPrefab, GameManager game) {
    get = this;
    queue = new LinkedList<actionTime>();
    turnButton = buttonPrefab;
    actionBar = bar;
    gameManager = game;
    buttonWidth = turnButton.GetComponent<RectTransform>().rect.width;
    buttonHeight = turnButton.GetComponent<RectTransform>().rect.height;
  }

  public GameObject getNext() {
    GameObject next = queue.First.Value.piece;
    float newTime = next.GetComponent<Character>().nextMoveTime;
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
    remove(SelectedPiece);
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
    int i = 0;

    foreach (LinkedListNode<actionTime> n in new NodeIterator<actionTime>(queue)) {
      if (n.Value.piece.Equals(piece)) {
        gameManager.StartCoroutine(SlideButton(n.Value.button, buttonWidth, 0, true));
        queue.Remove(n);
        moveUp(i - 1);
      }
      i++;
    }
  }

  void enqueue(GameObject piece) {
    GameObject button = GameObject.Instantiate(turnButton, new Vector3 (-25 + buttonWidth,0,0), Quaternion.identity) as GameObject;
    button.transform.SetParent(actionBar.transform, false);
    Character newCharacter = piece.GetComponent<Character>();
    int i = 0;

    foreach (LinkedListNode<actionTime> n in new NodeIterator<actionTime>(queue)) {
      GameObject o = n.Value.piece;
      Character c = o.GetComponent<Character>();
      if (c.nextMoveTime > newCharacter.nextMoveTime) {
        queue.AddBefore(n, new actionTime(piece, button));
        break;
      }
      i++;
    }

    if (i == queue.Count) {
      queue.AddLast(new actionTime(piece, button));
    }

    float posn = getPosn(i);
    moveDown(i);
    if (newCharacter.characterName == "") {
      newCharacter.characterName = queue.Count.ToString();
    }
    button.GetComponentsInChildren<Text>()[0].text = newCharacter.characterName;
    button.transform.Translate(new Vector3(0, posn, 0));
    gameManager.StartCoroutine(SlideButton(button, -buttonWidth, 0));
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
