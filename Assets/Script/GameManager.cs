using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Threading;
using System.Linq;

public enum GameState {
  moving,
  attacking
}

public class GameManager : MonoBehaviour {

  // Selected Piece
  List<GameObject> cubes = null;
  List<Tile> tiles = null;
  List<Button> skillButtons = null;
  LineRenderer line;
  public ActionQueue actionQueue;
  public GameObject turnButton;
  public GameObject iceBlock;

  public LinkedList<Tile> path = null;

  //variables to handles turns
  /** stack of (position, remaining move range), where top of stack is previous location */
  public Stack<Pair<Tile,int>> positionStack = new Stack<Pair<Tile, int>>();
  public GameObject SelectedPiece { get; private set;}
  public int SelectedSkill {get; set;}
  List<GameObject> skillTargets;
  GameObject previewTarget;

  public GameState gameState = GameState.moving;
  public bool playerTurn = true;

  //EventManager
  public EventManager eventManager;

  // Camera Control
  public CameraController cam;

  //Objectives for this game
  List<Objective> winningConditions = new List<Objective>();
  List<Objective> losingConditions = new List<Objective>();

  //health/mana bars
  public GameObject selectedHealth;
  public GameObject SelectedMana;
  public GameObject targetHealth;
  public GameObject targetMana;
  public GameObject targetPanel;
  public GameObject mainUI;

  public GameObject text;

  public Dictionary<int, List<GameObject>> characters = new Dictionary<int, List<GameObject>>();
  public List<GameObject> players { get{ return characters[0]; } }
  public List<GameObject> enemies { get{ return characters[1]; } }

  private LinkedList<Coroutine> waitEndTurn;

  public void waitToEndTurn(Coroutine c) {
    waitEndTurn.AddFirst(c);
  }

  class lockUICount {
    public int count;
  }
  private lockUICount UILock;

  void Start() {
    foreach (var l in characters.Values) {
      foreach (var o in l) {
        actionQueue.add(o); //Needs to be done here since it relies on characters having their attribute set
      }
    }
    startTurn();
  }

  void Awake() {
    eventManager.setGlobal();

    waitEndTurn = new LinkedList<Coroutine>();
    get = this;
    moving = false;
    cubes = new List<GameObject>(GameObject.FindGameObjectsWithTag("Cube"));
    tiles = new List<Tile>();
    UILock = new lockUICount();
    UILock.count = 0;

    skillButtons = new List<Button>();
    foreach (GameObject o in GameObject.FindGameObjectsWithTag("SkillButton")) {
      skillButtons.Add(o.GetComponent<Button>());
    }
    foreach (GameObject cube in cubes) {
      cube.AddComponent<Tile>();
      Tile t = cube.GetComponent<Tile>();
      tiles.Add(t);
    }

    path = new LinkedList<Tile>();

    line = gameObject.GetComponent<LineRenderer>();

    var objs = GameObject.FindGameObjectsWithTag("Unit").GroupBy(x => x.GetComponent<Character>().team);
    foreach (var x in objs) {
      characters[x.Key] = new List<GameObject>(x);
    }

    //set skill buttons to the selected piece's skills, enable those that actually have skills
    new Range(0, skillButtons.Count).ForEach(i => {
      skillButtons[i].onClick.AddListener(() => selectSkill(i));
      skillButtons[i].enabled = false;
    });


    winningConditions.Add(new Rout());
    losingConditions.Add(new BrodricDies());
    actionQueue = new ActionQueue(GameObject.FindGameObjectsWithTag("ActionBar")[0], turnButton, this);
    foreach (var l in characters.Values) {
      foreach (var o in l) {
        Character c = o.GetComponent<Character>();
        Tile t = getTile(o.transform.position);
        t.occupant = o;
        c.curTile = t;
      }
    }
  }

  int blinkFrameNumber = 0;
  bool displayChangedHealth = false;
  void Update() {
    //enable the line only when attacking
    line.enabled = gameState == GameState.attacking;

    if (SelectedPiece) {
      Character selectedCharacter = SelectedPiece.GetComponent<Character>();
      selectedCharacter.updateLifeBar(selectedHealth);
      for (int i = 0; i < skillButtons.Count; i++) {
        ActiveSkill s = selectedCharacter.equippedSkills[i];
        Debug.AssertFormat(s.name != "", "Skill Name is empty");
        skillButtons[i].GetComponentInChildren<Text>().text = s.name;
        skillButtons[i].interactable = s.canUse();
      }
    }

    if (previewTarget == null) {
      targetPanel.SetActive(false);
    } else {
      Character targetCharacter = previewTarget.GetComponent<Character>();
      if (targetCharacter == null) return;
      targetPanel.SetActive(true);
      if (displayChangedHealth) {
        Character selectedCharacter = SelectedPiece.GetComponent<Character>();
        Vector3 scale = targetHealth.transform.localScale;
        Skill s = selectedCharacter.equippedSkills[SelectedSkill];
        if (s is HealingSkill) scale.x = (float)(targetCharacter.curHealth + targetCharacter.PreviewHealing)/targetCharacter.maxHealth;
        else scale.x = (float)(targetCharacter.curHealth - targetCharacter.PreviewDamage)/targetCharacter.maxHealth;
        targetHealth.transform.localScale = scale;
      } else {
        Vector3 scale = targetHealth.transform.localScale;
        scale.x = (float)targetCharacter.curHealth/targetCharacter.maxHealth;
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
    setTileColours();
  }

  //start the next turn,
  //select the piece whose turn it is from the queue
  //set up variables for the new piece
  public void startTurn() {
    //check winning/losing conditions
    foreach(Objective o in losingConditions) {
      if (o.isMet(this)) {
        endGame(false);
      }
    }
    foreach(Objective o in winningConditions) {
      if (o.isMet(this)) {
        endGame(true);
      }
    }

    // Change color of the selected piece to make it apparent. Put it back to white when the piece is unselected
    // change color of the board squares that it can move to.
    clearColour();
    clearPath();
    if (SelectedPiece) {
      if (SelectedPiece.GetComponent<Character>().team == 0) SelectedPiece.GetComponent<Renderer>().material.color = Color.white;
      else SelectedPiece.GetComponent<Renderer>().material.color = Color.yellow;
    }

    //get character whose turn it is
    //do something different for ai
    SelectedPiece = actionQueue.getNext();
    Character selectedCharacter = SelectedPiece.GetComponent<Character>();
    selectedCharacter.onEvent(new Event(selectedCharacter, EventHook.startTurn));
    moveRange = selectedCharacter.moveRange;

    SelectedPiece.GetComponent<Renderer>().material.color = Color.red;
    line.SetPosition(0, SelectedPiece.transform.position);
    line.SetPosition(1, SelectedPiece.transform.position);


    Vector3 position = SelectedPiece.transform.position;
    djikstra(position, SelectedPiece.GetComponent<Character>());

    changeState(GameState.moving);
    // enemy
    if (SelectedPiece.GetComponent<Character>().team == 1) {
      playerTurn = false;
      handleAI();
      return;
    } else {
      cam.panTo(SelectedPiece.transform.position);
      playerTurn = true;
    }

    positionStack.Clear();

    for (int i = 0; i < skillButtons.Count; i++) {
      skillButtons[i].enabled = i < SelectedPiece.GetComponent<Character>().equippedSkills.Count;
    }
  }


  public void selectSkill(int i) {
    //unselect
    if (gameState == GameState.attacking && SelectedSkill == i) {
      SelectedSkill = -1;
      setTileColours();
      return;
    }

    SelectedSkill = i;
    ActiveSkill skill = SelectedPiece.GetComponent<Character>().equippedSkills[i];

    skillTargets = skill.getTargets();
    //change colours of the tiles for attacking
    //check for range skill if not put 1 else put the range
    changeState(GameState.attacking);
  }

  public void selectTarget(GameObject target) {
    if (SelectedSkill != -1 && skillTargets.Contains(target)) {
      previewTarget = target;
    } else {
      previewTarget = null;
      return;
    }

    Character cTarget = target.GetComponent<Character>();
    if (cTarget != null) {
      Character selectedCharacter = SelectedPiece.GetComponent<Character>();
      if (selectedCharacter.equippedSkills[SelectedSkill] is HealingSkill) cTarget.PreviewHealing = (selectedCharacter.equippedSkills[SelectedSkill] as HealingSkill).calculateHealing(selectedCharacter, cTarget);
      else cTarget.PreviewDamage = selectedCharacter.equippedSkills[SelectedSkill].calculateDamage(selectedCharacter, cTarget);
    }
    //todo: aoe health bar hover?
  }

  public void attackTarget(GameObject target) {
    bool aoe = (SelectedPiece.GetComponent<Character>().equippedSkills[SelectedSkill] is AoeSkill);
    List<Effected> targets = new List<Effected>();
    List<GameObject> validTargets = SelectedPiece.GetComponent<Character>().equippedSkills[SelectedSkill].getTargets();

    if (validTargets.Contains(target)){
      if (aoe) {
        AoeSkill skill = SelectedPiece.GetComponent<Character>().equippedSkills[SelectedSkill] as AoeSkill;
        foreach (GameObject o in skill.getTargetsInAoe(target.transform.position)) {
          Character c = o.GetComponent<Character>();
          if (c) targets.Add(c);
          if (skill.effectsTiles) targets.Add(o.GetComponent<Tile>());
        }
      } else {
        targets.Add(target.GetComponent<Character>());
      }

      Character selectedCharacter = SelectedPiece.GetComponent<Character>();
      selectedCharacter.attackWithSkill(selectedCharacter.equippedSkills[SelectedSkill], targets);
      StartCoroutine(endTurn());
    }
  }

  public void endTurnWrapper() {
    StartCoroutine(endTurn());
  }

  public IEnumerator endTurn() {
    foreach(Coroutine c in waitEndTurn) {
      yield return c;
    }
    waitEndTurn.Clear();

    //send endTurn event to the current piece
    Character selectedCharacter = SelectedPiece.GetComponent<Character>();
    Event e = new Event(null, EventHook.endTurn);
    e.endTurnChar = selectedCharacter;
    eventManager.onEvent(e);
    selectedCharacter.onEvent(new Event(selectedCharacter, EventHook.endTurn));

    if (selectedCharacter.team == 0) SelectedPiece.GetComponent<Renderer>().material.color = Color.white;
    else SelectedPiece.GetComponent<Renderer>().material.color = Color.yellow;
    actionQueue.endTurn();
    clearColour();
    startTurn();
  }

  public IEnumerator IterateMove(LinkedList<Tile> path, GameObject piece) {
    const float FPS = 60f;
    const float speed = 4f;
    lockUI();
    Character character = piece.GetComponent<Character>();

    if (gameState == GameState.moving) {
      cam.follow(SelectedPiece);
      yield return new WaitForSeconds(0.5f);
    }

    foreach (Tile destination in path) {
      // fix height
      Vector3 pos = destination.gameObject.transform.position;
      pos.y = destination.transform.position.y + getHeight(destination);

      // move piece
      Vector3 d = speed*(pos-piece.transform.position)/FPS;
      for (int i = 0; i < FPS/speed; i++) {
        piece.transform.Translate(d);
        yield return new WaitForSeconds(1/FPS);
      }
      // tell listeners that this character moved
      Event enterEvent = new Event(character, EventHook.enterTile);
      enterEvent.position = destination.transform.position;
      EventManager.get.onEvent(enterEvent);
      if (! character.isAlive()) {
        endTurnWrapper();
        break; // character can die mid-move now
      }
    }
    clearPath();
    setTileColours();


    if (gameState == GameState.moving) {
      yield return new WaitForSeconds(0.25f);
      cam.unfollow();
    }

    moving = false;
    for (int i = 0; i < skillButtons.Count; i++) {
      skillButtons[i].enabled = i < piece.GetComponent<Character>().equippedSkills.Count;
    }
    unlockUI();
  }

  // temporarily public
  public bool moving {get; /*private*/ set;}
  // Move the SelectedPiece to the inputted coords


  public void updateTile(Character c, Tile t) {
    eventManager.onEvent(new Event(c, EventHook.preMove));
    c.curTile.occupant = null;
    c.curTile = t;
    t.occupant = c.gameObject;
    eventManager.onEvent(new Event(c, EventHook.postMove));
  }

  /** remaining move amount */
  public int moveRange = 0;
  public Coroutine MovePiece(Vector3 coordToMove, bool smooth = true, bool moveCommand = true) {
    // don't start moving twice
    if (moving) return null;
    LinkedList<Tile> localPath = new LinkedList<Tile>(path);

    Tile destination = getTile(coordToMove);
    Character c = SelectedPiece.GetComponent<Character>();

    if (destination.distance <= moveRange && !destination.occupied()) {
      // if player chose to move, update position stack with current values,
      // update remaining move range, and recolor the tiles given the new current position
      if (moveCommand) {
        positionStack.Push(Pair.create(c.curTile, moveRange));
        moveRange -= destination.distance;
        djikstra(coordToMove, c);
      }
      //after moving, remove from origin tile,
      //add to new tile
      updateTile(c,destination);

      coordToMove.y = destination.transform.position.y + getHeight(destination);
      if (smooth) {
        path.RemoveFirst(); // discard current position
        moving = true;
        line.GetComponent<Renderer>().material.color = Color.clear;
        return StartCoroutine(IterateMove(localPath, SelectedPiece));
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
    if (positionStack.Count() > 0) {
      // reset character to previous position and remaining move range, then recolor movable tiles
      Pair<Tile, int> val = positionStack.Pop();
      Vector3 coordToMove = val.first.gameObject.transform.position;
      moveRange = val.second;
      MovePiece(coordToMove, false, false);
      changeState(GameState.moving);
      djikstra(coordToMove, SelectedPiece.GetComponent<Character>());
      setTileColours();
    }
    eventManager.onEvent(new Event(SelectedPiece.GetComponent<Character>(), EventHook.cancel));
  }

  public void endGame(bool win) {
    Debug.Log(win);
  }

  IEnumerator doHandleAI(int time) {
    lockUI();
    Character selectedCharacter = SelectedPiece.GetComponent<Character>();
    Vector3 destination = selectedCharacter.moveAI.move();
    Tile t = getTile(destination);
    while(t != path.Last.Value) {
      path.RemoveLast();
    }
    t.gameObject.GetComponent<Renderer>().material.color = Color.black;
    yield return MovePiece(destination, true);

    yield return StartCoroutine(AIperformAttack(selectedCharacter));
    unlockUI();
    StartCoroutine(endTurn());
  }
  public void handleAI() {
    StartCoroutine(doHandleAI(1));
  }

  public IEnumerator AIperformAttack(Character selectedCharacter) {
    cam.follow(SelectedPiece);
    yield return new WaitForSeconds(0.5f);
    selectedCharacter.attackAI.target();
    yield return new WaitForSeconds(0.25f);
    cam.unfollow();
  }

  public void lockUI() {
    lock (UILock) {
      UILock.count++;
      if (UILock.count == 1) {
        mainUI.GetComponent<CanvasGroup>().interactable = false;
        gameObject.GetComponent<PlayerControl>().enabled = false;
      }
    }
  }

  public void unlockUI() {
    lock (UILock) {
      UILock.count--;
      if (UILock.count == 0) {
        mainUI.GetComponent<CanvasGroup>().interactable = true;
        gameObject.GetComponent<PlayerControl>().enabled = true;
      }
    }
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
    Vector3 hit = new Vector3(hitInfo.collider.gameObject.transform.position.x, hitInfo.collider.gameObject.transform.position.y + offset, hitInfo.collider.gameObject.transform.position.z);
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


  // GameMap functions
  public void setTileColours(Tile src = null) {
    if (src == null) src = getTile(SelectedPiece.transform.position);
    clearColour();
    if (gameState == GameState.moving) {
      foreach (Tile tile in tiles) {
        if (tile.distance <= moveRange && !tile.occupied()) {
          tile.gameObject.GetComponent<Renderer>().material.color = Color.green;
        } else {
          tile.gameObject.GetComponent<Renderer>().material.color = Color.white;
        }
      }
    } else if (gameState == GameState.attacking && SelectedSkill != -1) {
      bool aoe = (SelectedPiece.GetComponent<Character>().equippedSkills[SelectedSkill] is AoeSkill);
      int range = SelectedPiece.GetComponent<Character>().equippedSkills[SelectedSkill].range;
      List<Tile> inRangeTiles = getTilesWithinRange(getTile(SelectedPiece.transform.position), range);
      if (!aoe) {
        foreach (Tile tile in inRangeTiles) {
          tile.gameObject.GetComponent<Renderer>().material.color = Color.blue;
        }
        foreach (GameObject o in skillTargets) {
          getTile(o.transform.position).gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
      } else {
        foreach (GameObject o in SelectedPiece.GetComponent<Character>().equippedSkills[SelectedSkill].getTargets()) {
          o.GetComponent<Renderer>().material.color = Color.blue;
        }
        AoeSkill skill = SelectedPiece.GetComponent<Character>().equippedSkills[SelectedSkill] as AoeSkill;
        var targetsInAoe = skill.getTargetsInAoe(src.gameObject.transform.position);
        if (targetsInAoe != null) {
          foreach (GameObject o in targetsInAoe) {
            if (o.tag == "Cube") getTile(o.transform.position).gameObject.GetComponent<Renderer>().material.color = Color.yellow;
            else getTile(o.transform.position).gameObject.GetComponent<Renderer>().material.color = Color.red;
          }
        }
      }
    }
    foreach (Tile ti in path) {
      ti.gameObject.GetComponent<Renderer>().material.color = Color.blue;
    }
  }

  public void djikstra(Vector3 unitLocation, Character charToMove) {
    foreach (Tile tile in tiles) {
      tile.distance = System.Int32.MaxValue/2;
      tile.dir = Vector3.zero;
    }

    HashSet<Tile> tilesToGo = new HashSet<Tile>(tiles);

    Tile startTile = getTile(unitLocation);
    startTile.distance = 0;

    while (tilesToGo.Count != 0) {
      int minDistance = System.Int32.MaxValue;
      Tile minTile = null;
      foreach (Tile tile in tilesToGo) {
        if (tile.distance <= minDistance) {
          minDistance = tile.distance;
          minTile = tile;
        }
      }

      // TODO: update portal dest distance to portal src distance
      Vector3[] directions = new Vector3[]{ Vector3.forward, Vector3.back, Vector3.right, Vector3.left };
      foreach (Vector3 dir in directions) {
        Vector3 neighbour = minTile.gameObject.transform.position + dir;
        Tile neighbourTile = getTile(neighbour, tilesToGo);
        if (neighbourTile != null) {
          int d = minTile.distance + distance(neighbourTile, minTile, charToMove.moveTolerance);
          if (d < neighbourTile.distance) {
            neighbourTile.distance = d;
            neighbourTile.dir = dir;
          }
        }
      }
      tilesToGo.Remove(minTile);
    }
  }

  public int distance(Tile from, Tile to, float moveTolerance) {
    //check heights
    if (Math.Abs(getHeight(from) - getHeight(to)) > moveTolerance) {
      return Int32.MaxValue/2;
    }
    if (from.occupied() && !(SelectedPiece.GetComponent<Character>().team == from.occupant.GetComponent<Character>().team)) {
      return Int32.MaxValue/4;
    }

    return Math.Max((int)(to.movePointSpent - moveTolerance + 1), 1);
  }

  public float getHeight(Tile t) {
    float scale = t.getHeight();//t.gameObject.transform.localScale.y;
    return scale + 0.5f;
  }


  public Tile getTile(Vector3 location) {
    return getTile(location, tiles);
  }

  public Tile getTile(Vector3 location, IEnumerable<Tile> list) {
    foreach (Tile tile in list) {
      if (Math.Abs(tile.gameObject.transform.position.x - location.x) < 0.05f &&
          Math.Abs(tile.gameObject.transform.position.z - location.z) < 0.05f) {
        return tile;
      }
    }
    return null;
  }

  public List<Tile> getTilesWithinRange(Tile t, int range) {
    List<Tile> inRangeTiles = new List<Tile>();
    foreach (Tile other in tiles) {
      if (l1Distance(t, other) <= range && l1Distance(t, other) != 0) {
        inRangeTiles.Add(other);
      }
    }
    return inRangeTiles;
  }

  public List<Tile> getCardinalTilesWithinRange(Tile t, int range) {
    List<Tile> inRangeTiles = getTilesWithinRange(t, range);
    inRangeTiles = new List<Tile>(inRangeTiles.Filter((tile) =>
      Math.Abs(tile.gameObject.transform.position.x - t.gameObject.transform.position.x) < 0.05f  ||
      Math.Abs(tile.gameObject.transform.position.z - t.gameObject.transform.position.z) < 0.05f));
    return inRangeTiles;
  }

  public int l1Distance(Tile t1, Tile t2) {
    return (int)Math.Floor(Math.Abs(t1.gameObject.transform.position.x - t2.gameObject.transform.position.x) + 0.5f)  +
          (int)Math.Floor(Math.Abs(t1.gameObject.transform.position.z - t2.gameObject.transform.position.z) + 0.5f) ;
  }

  public void clearColour() {
    foreach (Tile tile in tiles) {
      tile.gameObject.GetComponent<Renderer>().material.color = Color.white;
    }
  }

  public void setPath(Vector3 coord) {
    clearPath();
    Tile t = getTile(coord);
    path.AddFirst(t);
    while (t.dir != Vector3.zero) {
      coord -= t.dir;
      t = getTile(coord);
      path.AddFirst(t);
    }
  }

  public void clearPath() {
    path.Clear();
  }

  public GameObject piece;
  // Test function that instantiates a character
  public void createPiece() {
    GameObject newCharObj = Instantiate(piece, new Vector3(0f, 1f, 0f), Quaternion.identity, GameObject.FindGameObjectWithTag("ChessModels").transform) as GameObject;
    actionQueue.add(newCharObj);
  }

  public static GameManager get { get; private set; }
}
