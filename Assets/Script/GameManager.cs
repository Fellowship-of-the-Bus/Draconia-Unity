using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Threading;
using System.Linq;

public enum GameState {
  moving,
  attacking,
  ending
}

public class GameManager : MonoBehaviour {
  // Selected Piece
  public Map map = new Map();
  List<Button> skillButtons = null;
  LineRenderer line;
  public ActionQueue actionQueue;
  public GameObject turnButton;
  public GameObject iceBlock;
  public BuffBar activeBuffBar;
  public BuffBar targetBuffBar;
  public GameObject buffButton;

  //variables to handles turns

  //TODO: Finish handling of portal skll
  public class UndoAction {
    Action act;
    bool pushedBySkill;

    public UndoAction(Action a, bool skill = false) {
      act = a;
      pushedBySkill = skill;
    }

    public bool undo() {
      act();
      return pushedBySkill;
    }

  }
  public Stack<UndoAction> cancelStack = new Stack<UndoAction>();
  public GameObject SelectedPiece { get; private set;}
  public int SelectedSkill {get; set;}
  public List<Tile> skillTargets;
  BattleCharacter previewTarget;

  public GameState gameState = GameState.moving;
  public bool playerTurn = true;

  //EventManager
  public EventManager eventManager;

  // Camera Control
  public CameraController cam;

  //Objectives for this game
  public string[] winObjs;
  public string[] loseObjs;
  public List<Objective> winningConditions = new List<Objective>();
  public List<Objective> losingConditions = new List<Objective>();

  //health/mana bars
  public GameObject selectedHealth;
  public GameObject targetHealth;
  public GameObject targetPanel;
  public GameObject mainUI;

  public GameObject text;

  //skillbutton tooltip
  public GameObject tooltip;

  //tile information
  public TileInfo tInfo;

  //main dialogue controller
  public Dialogue dialogue;
  public DialogueReader reader;

  //Map Boss
  public BattleCharacter boss;

  public Dictionary<int, List<GameObject>> characters = new Dictionary<int, List<GameObject>>();
  public List<GameObject> players { get{ return characters[0]; } }
  public List<GameObject> enemies { get{ return characters[1]; } }

  private List<Coroutine> waitingOn = new List<Coroutine>();

  private List<BFEvent> BFevents = new List<BFEvent>();

  private class DeathListener : EventListener {
    public override void onEvent(Event e) {
      GameManager g = GameManager.get;
      g.characters[e.sender.team].Remove(e.sender.gameObject);
      if (e.sender == g.SelectedPiece.GetComponent<BattleCharacter>()) g.endTurnWrapper();
    }
  }

  private DeathListener deathListener = new DeathListener();

  public class PostGameData {
    public bool win;
    public List<Character> inBattle;
    public List<Equipment> loot;
  }
  public static PostGameData postData = new PostGameData();

  IEnumerator waitForSeconds(float s) {
    yield return new WaitForSeconds(s);
  }

  IEnumerator popAtEnd(Coroutine c, Action act) {
    yield return c;
    waitingOn.Remove(c);
    if (act != null) act();
  }

  IEnumerator waitUntilCount(int count) {
    while (waitingOn.Count > count) {
      yield return null;
    }
  }

  public void waitFor(float s, Action act = null) {
    waitFor(StartCoroutine(waitForSeconds(s)), act);
  }

  IEnumerator waitForAnimation(Animator animator, String trigger) {
    animator.SetTrigger(trigger);
    yield return new WaitForEndOfFrame(); //Necessary to wait for animator state info to be updated
                                          //otherwise next line always wait for 0.
    yield return new WaitForSeconds(animator.GetNextAnimatorStateInfo(0).length);
  }

  public void waitFor(Animator a, String trigger, Action act = null) {
    waitFor(StartCoroutine(waitForAnimation(a, trigger)), act);
  }

  public void waitFor(Coroutine c, Action act = null) {
    waitingOn.Add(c);
    StartCoroutine(popAtEnd(c, act));
  }

  public int getWaitingIndex() {
    return waitingOn.Count;
  }

  class lockUICount {
    public int count;
  }
  private lockUICount UILock;

  void Awake() {
    eventManager.setGlobal();

    get = this;
    moving = false;
    UILock = new lockUICount();
    UILock.count = 0;

    actionQueue = new ActionQueue(GameObject.FindGameObjectsWithTag("ActionBar")[0], turnButton, this);

    skillButtons = new List<Button>();
    foreach (GameObject o in GameObject.FindGameObjectsWithTag("SkillButton")) {
      skillButtons.Add(o.GetComponent<Button>());
    }

    map.awake();

    line = gameObject.GetComponent<LineRenderer>();

    //set skill buttons to the selected piece's skills, enable those that actually have skills
    new Range(0, skillButtons.Count).ForEach(i => {
      skillButtons[i].onClick.AddListener(() => selectSkill(i));
      skillButtons[i].enabled = true;
    });

    //winningConditions.Add(new Rout());
    //losingConditions.Add(new BrodricDies());
    foreach (string s in winObjs) {
      winningConditions.Add(ObjectiveFactory.makeObjective(s));
    }
    foreach (string s in loseObjs) {
      losingConditions.Add(ObjectiveFactory.makeObjective(s));
    }

    string mapName = SceneManager.GetActiveScene().name;
    reader = new DialogueReader(mapName);
  }
  //should begin in character select phase, probably using a different camera...
  void Start() {
    //startTurn();
    BFevents = reader.inBattle;
    foreach (BFEvent e in BFevents) {
      e.init();
    }
  }

  public void init() {
    dialogue.setOnExit(() => GameSceneController.get.pControl.enabled = true);
    var objs = GameObject.FindGameObjectsWithTag("Unit").GroupBy(x => x.GetComponent<BattleCharacter>().team);
    foreach (var x in objs) {
      characters[x.Key] = new List<GameObject>(x);
    }

    foreach (var l in characters.Values) {
      foreach (var o in l) {
        BattleCharacter c = o.GetComponent<BattleCharacter>();
        Tile t = map.getTile(o.transform.position);
        c.transform.position = t.position;
        t.occupant = c;
        c.curTile = t;
      }
    }

    foreach (var l in characters.Values) {
      foreach (var o in l) {
        var bchar = o.GetComponent<BattleCharacter>();
        bchar.init();
        bchar.ai.init();
        deathListener.attachListener(bchar, EventHook.postDeath);
        actionQueue.add(o); //Needs to be done here since it relies on characters having their attribute set
      }
    }

    List<Character> charInBattle = new List<Character>(players.Map(x => x.GetComponent<BattleCharacter>().baseChar));
    GameManager.postData.inBattle = charInBattle;
  }

  int blinkFrameNumber = 0;
  bool displayChangedHealth = false;
  void Update() {
    if (!UILocked()) {
      if (Input.GetKeyDown(KeyCode.Return)) {
        endTurnWrapper();
      }
    }

    //enable the line only when attacking
    line.enabled = gameState == GameState.attacking;

    if (SelectedPiece) {
      BattleCharacter selectedCharacter = SelectedPiece.GetComponent<BattleCharacter>();
      selectedCharacter.updateLifeBar(selectedHealth);
      if (Options.debugMode) {
        for (int i = 0; i < selectedCharacter.equippedSkills.Count; i++) {
          ActiveSkill s = selectedCharacter.equippedSkills[i];
          Debug.AssertFormat(s.name != "", "Skill Name is empty");
          skillButtons[i].GetComponentInChildren<Text>().text = s.name;
          skillButtons[i].gameObject.GetComponent<Tooltip>().tiptext = s.tooltip;
          skillButtons[i].interactable = s.canUse();
        }
      }
    }

    if (previewTarget == null) {
      targetPanel.SetActive(false);
    } else {
      targetBuffBar.update(previewTarget);
      targetPanel.SetActive(true);
      if (gameState == GameState.moving) displayChangedHealth = false;
      if (displayChangedHealth && skillTargets.Contains(previewTarget.curTile)) {
        BattleCharacter selectedCharacter = SelectedPiece.GetComponent<BattleCharacter>();
        Vector3 scale = targetHealth.transform.localScale;
        Skill s = selectedCharacter.equippedSkills[SelectedSkill];
        if (s is HealingSkill) scale.x = (float)(previewTarget.curHealth + previewTarget.PreviewHealing)/previewTarget.maxHealth;
        else scale.x = (float)(previewTarget.curHealth - previewTarget.PreviewDamage)/previewTarget.maxHealth;
        targetHealth.transform.localScale = scale;
      } else {
        Vector3 scale = targetHealth.transform.localScale;
        scale.x = (float)previewTarget.curHealth/previewTarget.maxHealth;
        targetHealth.transform.localScale = scale;
      }
      blinkFrameNumber = (blinkFrameNumber+1)%30;
      if (blinkFrameNumber == 0) {
        displayChangedHealth = !displayChangedHealth;
      }
    }
  }

  // Game State functions //

  void changeState(GameState newState) {
    if (newState == GameState.moving) {
      previewTarget = null;
      SelectedSkill = -1;
    }
    gameState = newState;
    if (newState == GameState.attacking) map.clearPath();
    map.setTileColours();
  }

  //start the next turn,
  //select the piece whose turn it is from the queue
  //set up variables for the new piece
  public void startTurn() {
    //check winning/losing conditions
    foreach(Objective o in losingConditions) {
      if (o.isMet(this)) {
        endGame(false);
        return;
      }
    }
    bool win = true;
    foreach(Objective o in winningConditions) {
      win = win && o.isMet(this);
    }
    if (win) {
      endGame(true);
      return;
    }

    // Change color of the selected piece to make it apparent. Put it back to white when the piece is unselected
    // change color of the board squares that it can move to.
    map.clearColour();
    map.clearPath();

    //get character whose turn it is
    //do something different for ai
    SelectedPiece = actionQueue.getNext();
    BattleCharacter selectedCharacter = SelectedPiece.GetComponent<BattleCharacter>();
    selectedCharacter.onEvent(new Event(selectedCharacter, EventHook.startTurn));
    moveRange = selectedCharacter.moveRange;
    activeBuffBar.update(selectedCharacter);

    // SelectedPiece.GetComponent<Renderer>().material.color = Color.red;
    line.SetPosition(0, SelectedPiece.transform.position);
    line.SetPosition(1, SelectedPiece.transform.position);


    Vector3 position = SelectedPiece.transform.position;
    map.djikstra(position, SelectedPiece.GetComponent<BattleCharacter>());

    changeState(GameState.moving);
    // enemy
    if (SelectedPiece.GetComponent<BattleCharacter>().team == 1) {
      playerTurn = false;
      handleAI();
      return;
    } else {
      cam.panTo(SelectedPiece.transform.position);
      playerTurn = true;
    }

    cancelStack.Clear();

    for (int i = 0; i < skillButtons.Count; i++) {
      skillButtons[i].interactable = false;
    }


    for (int i = 0; i < selectedCharacter.equippedSkills.Count; i++) {
      ActiveSkill s = selectedCharacter.equippedSkills[i];
      Debug.AssertFormat(s.name != "", "Skill Name is empty");
      skillButtons[i].GetComponentInChildren<Text>().text = s.name;
      skillButtons[i].gameObject.GetComponent<Tooltip>().tiptext = s.tooltip;
      skillButtons[i].interactable = s.canUse();
    }
  }


  public void selectSkill(int i) {
    //unselect
    if (gameState == GameState.attacking) {
      while(!cancelStack.Pop().undo());
      return;
    }

    SelectedSkill = i;
    ActiveSkill skill = SelectedPiece.GetComponent<BattleCharacter>().equippedSkills[i];

    skillTargets = skill.getTargets();
    //change colours of the tiles for attacking
    //check for range skill if not put 1 else put the range
    changeState(GameState.attacking);

    cancelStack.Push(new UndoAction(() => {


      changeState(GameState.moving);
    }, true));
  }

  public void selectTarget(BattleCharacter target) {
    previewTarget = target;

    if (target == null) return;

    if (SelectedSkill != -1 && skillTargets.Contains(target.curTile)) {
      BattleCharacter selectedCharacter = SelectedPiece.GetComponent<BattleCharacter>();
      ActiveSkill skill = selectedCharacter.equippedSkills[SelectedSkill];
      HealingSkill hskill = skill as HealingSkill;
      if (hskill != null) previewTarget.PreviewHealing = skill.calculateHealing(previewTarget);
      else previewTarget.PreviewDamage = skill.calculateDamage(previewTarget);
    }
    //todo: aoe health bar hover?
  }

  public List<Tile> targets = new List<Tile>();
  public void attackTarget(Tile target) {
    BattleCharacter selectedCharacter = SelectedPiece.GetComponent<BattleCharacter>();
    ActiveSkill skill = selectedCharacter.equippedSkills[SelectedSkill];
    List<Tile> validTargets = skill.getTargets();

    if (validTargets.Contains(target)) {
      targets.Add(target);
      cancelStack.Push(new UndoAction(() => {
        targets.Remove(target);
        map.setTileColours();
      }));
      skill.validate(targets);
    }
    if (targets.Count() != skill.ntargets) {
      return;
    }

    if (selectedCharacter.useSkill(skill, targets)) {
      StartCoroutine(endTurn());
    }
    targets.Clear();
  }

  public void endTurnWrapper() {
    StartCoroutine(endTurn());
  }

  public IEnumerator endTurn() {
    if (gameState != GameState.ending) {
      changeState(GameState.ending);
      yield return StartCoroutine(waitUntilCount(0));
      waitingOn.Clear();
      targets.Clear();

      //send endTurn event to the current piece
      BattleCharacter selectedCharacter = SelectedPiece.GetComponent<BattleCharacter>();
      Event e = new Event(null, EventHook.endTurn);
      e.endTurnChar = selectedCharacter;
      e.nextCharTime = actionQueue.peekNext();
      eventManager.onEvent(e);
      selectedCharacter.onEvent(new Event(selectedCharacter, EventHook.endTurn));

      // if (selectedCharacter.team == 0) SelectedPiece.GetComponent<Renderer>().material.color = Color.white;
      // else SelectedPiece.GetComponent<Renderer>().material.color = Color.yellow;
      actionQueue.endTurn();
      map.clearColour();
      startTurn();
    }
  }

  public void movePiece(BattleCharacter c, Tile t, bool setWalking = true) {
    map.djikstra(t.transform.position, c);
    updateTile(c,t);
    LinkedList<Tile> tile = new LinkedList<Tile>();
    tile.AddFirst(t);
    moving = true;
    waitFor(StartCoroutine(IterateMove(tile, c.gameObject, waitingOn.Count, setWalking)));
  }

  public IEnumerator IterateMove(LinkedList<Tile> path, GameObject piece, int index, bool setWalking) {
    const float speed = 3f;
    lockUI();
    BattleCharacter character = piece.GetComponent<BattleCharacter>();

    if (gameState == GameState.moving) {
      cam.follow(SelectedPiece);
      yield return new WaitForSeconds(0.5f);
    }

    Animator animator = piece.GetComponentInChildren<Animator>() as Animator;
    if (setWalking && animator) {
      animator.SetBool("isWalking", true);
    }

    foreach (Tile destination in path) {
      // fix height
      Vector3 pos = destination.transform.position;
      pos.y = destination.transform.position.y + map.getHeight(destination);

      // Set Rotation
      if (setWalking) {
        piece.GetComponent<BattleCharacter>().face(pos);
      }

      // Move Piece
      Vector3 d = speed*(pos-piece.transform.position)/Options.FPS;
      float hopHeight = Math.Max(pos.y, piece.transform.position.y) + 0.5f;
      float dUp = speed*2*(hopHeight - piece.transform.position.y)/Options.FPS;
      float dDown = speed*2*(pos.y - hopHeight)/Options.FPS;
      for (int i = 0; i < Options.FPS/speed; i++) {
        if (d.y != 0) {
          if (i < Options.FPS/(speed*2)) {
            d.y = dUp;
          } else {
            d.y = dDown;
          }
        }
        piece.transform.Translate(d);
        yield return new WaitForSeconds(1/Options.FPS);
      }
      piece.transform.Translate(pos-piece.transform.position);
      // tell listeners that this character moved
      Event enterEvent = new Event(character, EventHook.enterTile);
      enterEvent.position = destination.transform.position;
      EventManager.get.onEvent(enterEvent);
      if (animator) animator.enabled = false;
      yield return waitUntilCount(index+1);
      if (animator) animator.enabled = true;
    }
    map.clearPath();
    map.setTileColours();

    if (setWalking && animator) {
      animator.SetBool("isWalking", false);
    }

    if (gameState == GameState.moving) {
      yield return new WaitForSeconds(0.25f);
      cam.unfollow();
    }

    moving = false;
    for (int i = 0; i < skillButtons.Count; i++) {
      skillButtons[i].enabled = i < piece.GetComponent<BattleCharacter>().equippedSkills.Count;
    }
    unlockUI();
  }

  // temporarily public
  public bool moving {get; /*private*/ set;}
  // Move the SelectedPiece to the inputted coords


  public void updateTile(BattleCharacter c, Tile t) {
    eventManager.onEvent(new Event(c, EventHook.preMove));
    c.curTile.occupant = null;
    c.curTile = t;
    t.occupant = c;
    eventManager.onEvent(new Event(c, EventHook.postMove));
  }

  /** remaining move amount */
  public int moveRange = 0;
  public Coroutine movePiece(Vector3 coordToMove, bool smooth = true, bool moveCommand = true) {
    // don't start moving twice
    if (moving) return null;
    LinkedList<Tile> localPath = new LinkedList<Tile>(map.path);

    Tile destination = map.getTile(coordToMove);
    BattleCharacter c = SelectedPiece.GetComponent<BattleCharacter>();

    if ((destination.distance <= moveRange && !destination.occupied()) || !moveCommand) {
      // if player chose to move, update position stack with current values,
      // update remaining move range, and recolor the tiles given the new current position
      if (moveCommand) {
        int i = moveRange;
        Tile cur = c.curTile;
        cancelStack.Push(new UndoAction(() => {
          Vector3 coord = cur.transform.position;
          moveRange = i;
          movePiece(coord, false, false);
          changeState(GameState.moving);
          map.djikstra(coord, c);
          map.setTileColours();
        }));
        moveRange -= destination.distance;
        map.djikstra(coordToMove, c);
      }
      //after moving, remove from origin tile,
      //add to new tile
      updateTile(c,destination);

      coordToMove.y = destination.transform.position.y + map.getHeight(destination);
      if (smooth) {
        localPath.RemoveFirst(); // discard current position
        moving = true;
        line.GetComponent<Renderer>().material.color = Color.clear;
        Coroutine co = StartCoroutine(IterateMove(localPath, SelectedPiece, waitingOn.Count, true));
        waitFor(co);
        return co;
      } else {
        SelectedPiece.transform.position = coordToMove;
      }
    }
    return null;
    // To avoid concurrency problems, avoid putting any code after StartCoroutine.
    // Any code that should be executed when the coroutine finishes should just
    // go at the end of the coroutine.
  }

  public void cancelAction() {
    BattleCharacter character = SelectedPiece.GetComponent<BattleCharacter>();
    if (cancelStack.Count() > 0) {
      // reset character to previous position and remaining move range, then recolor movable tiles
      UndoAction act = cancelStack.Pop();
      act.undo();
    }
    eventManager.onEvent(new Event(character, EventHook.cancel));
  }

  public void endGame(bool win) {
    dialogue.setOnExit(() => displayPostScreen(win));
    if (win) {
      dialogue.loadDialogue(reader.end);
    } else {
      displayPostScreen(win);
    }
  }

  public void displayPostScreen(bool win) {
    GameManager.postData.win = win;
    GameManager.postData.loot = LootGenerator.get.getLoot(SceneManager.GetActiveScene().name);
    SceneManager.LoadSceneAsync("PostMap");
  }

  IEnumerator doHandleAI(int time) {
    lockUI();
    BattleCharacter selectedCharacter = SelectedPiece.GetComponent<BattleCharacter>();
    Vector3 destination = selectedCharacter.ai.move();
    map.setTileColours();
    Tile t = map.getTile(destination);
    while(t != map.path.Last.Value) {
      map.path.RemoveLast();
    }

    int count = waitingOn.Count;
    movePiece(destination, true);
    yield return waitUntilCount(count);

    yield return StartCoroutine(AIperformAttack(selectedCharacter));
    unlockUI();
    StartCoroutine(endTurn());
  }
  public void handleAI() {
    StartCoroutine(doHandleAI(1));
  }

  public IEnumerator AIperformAttack(BattleCharacter selectedCharacter) {
    cam.follow(SelectedPiece);
    yield return new WaitForSeconds(0.5f);
    selectedCharacter.ai.target();
    yield return new WaitForSeconds(0.25f);
    cam.unfollow();
  }

  public void lockUI() {
    lock (UILock) {
      UILock.count++;
      if (UILock.count == 1) {
        mainUI.GetComponent<CanvasGroup>().interactable = false;
      }
    }
  }

  public void unlockUI() {
    lock (UILock) {
      UILock.count--;
      if (UILock.count == 0) {
        mainUI.GetComponent<CanvasGroup>().interactable = true;
      }
    }
  }

  public bool UILocked() {
    return UILock.count != 0;
  }


  public bool checkLine(Vector3 source, Vector3 target, float offset = 0.25f) {
    RaycastHit info;
    return checkLine(source, target, out info, offset);
  }

  public bool checkLine(Vector3 source, Vector3 target, out RaycastHit info, float offset = 0.25f) {
    Vector3 toTarget = target - source;

    Ray ray = new Ray(source, toTarget.normalized);
    RaycastHit hitInfo;
    Physics.Raycast(ray, out hitInfo);
    info = hitInfo;
    if (hitInfo.collider == null) return false;
    Vector3 hit = new Vector3(hitInfo.collider.transform.position.x, hitInfo.collider.transform.position.y + offset, hitInfo.collider.transform.position.z);
    return (hit == target);
  }

  // Draw line to piece
  public void lineTo(GameObject piece) {
    if (SelectedPiece && piece) {
      if (SelectedPiece == piece) {
        line.SetPosition(1, SelectedPiece.transform.position);
      } else {
        Vector3 source = new Vector3(SelectedPiece.transform.position.x, SelectedPiece.transform.position.y + 0.25f, SelectedPiece.transform.position.z);
        Vector3 target = new Vector3(piece.transform.position.x, piece.transform.position.y + 0.25f, piece.transform.position.z);
        RaycastHit hitInfo;
        if (checkLine(source, target, out hitInfo)) {
          line.GetComponent<Renderer>().material.color = Color.red;
        } else {
          line.GetComponent<Renderer>().material.color = Color.black;
        }

        line.SetPosition(0, source);
        line.SetPosition(1, hitInfo.point);
      }
    }
  }

  // Get the object being clicked on
  public GameObject getClicked(Camera PlayerCam) {
    if (Input.GetMouseButtonDown(0)) {
      return getHovered(PlayerCam);
    }
    return null;
  }

  // Get the object being hovered over
  public GameObject getHovered(Camera PlayerCam) {
    Ray ray = PlayerCam.ScreenPointToRay(Input.mousePosition); // Specify the ray to be casted from the position of the mouse click
    // Raycast and verify that it collided

    RaycastHit hitInfo;
    if (Physics.Raycast(ray, out hitInfo)) {
      return hitInfo.collider.gameObject;
    }
    return null;
  }

  public GameObject piece;
  // Test function that instantiates a character
  public BattleCharacter createPiece() {
    GameObject newCharObj = Instantiate(piece, new Vector3(0f, 1f, 0f), Quaternion.identity, GameObject.FindGameObjectWithTag("ChessModels").transform) as GameObject;
    var c = newCharObj.GetComponent<BattleCharacter>();
    return c;
  }

  public static GameManager get { get; set; }

}
