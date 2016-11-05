using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

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

  public LinkedList<Tile> path = null;

  //variables to handles turns
  public Tile originalTile;
  public GameObject SelectedPiece { get; private set;}
  public int SelectedSkill {get; private set;}
  List<GameObject> skillTargets;
  GameObject previewTarget;

  public GameState gameState = GameState.moving;

  //EventManager
  public EventManager eventManager = new EventManager();

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

  class lockUICount {
    public int count;
  }
  private lockUICount UILock;

  void Start() {
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

    actionQueue = new ActionQueue(GameObject.FindGameObjectsWithTag("ActionBar")[0], turnButton, this);
    GameObject[] characterObjects = GameObject.FindGameObjectsWithTag("PiecePlayer1");
    GameObject[] enemies = GameObject.FindGameObjectsWithTag("PiecePlayer2");

    foreach (GameObject o in characterObjects) {
      actionQueue.add(o);
      Character c = o.GetComponent<Character>();
      Tile t = getTile(o.transform.position);
      t.occupant = o;
      c.curTile = t;
    }

    foreach (GameObject o in enemies) {
      actionQueue.add(o);
      Character c = o.GetComponent<Character>();
      Tile t = getTile(o.transform.position);
      t.occupant = o;
      c.curTile = t;
    }

    //set skill buttons to the selected piece's skills, enable those that actually have skills
    new Range(0, skillButtons.Count).ForEach(i => {
      skillButtons[i].onClick.AddListener(() => selectSkill(i));
      skillButtons[i].enabled = false;
    });


    winningConditions.Add(new Rout());
    losingConditions.Add(new BrodricDies());

    startTurn();
    //set occupants
  }

  int blinkFrameNumber = 0;
  bool displayLowerHealth = false;
  void Update() {
    //enable the line only when attacking
    if (gameState == GameState.attacking) {
      line.enabled = true;
    } else {
      line.enabled = false;
    }

    if (SelectedPiece) {
      Character selectedCharacter = SelectedPiece.GetComponent<Character>();
      selectedCharacter.updateLifeBar(selectedHealth);
    }

    if (previewTarget == null) {
      targetPanel.SetActive(false);
    } else {
      Character targetCharacter = previewTarget.GetComponent<Character>();
      if (targetCharacter == null) return;
      targetPanel.SetActive(true);
      if (displayLowerHealth) {
        Vector3 scale = targetHealth.transform.localScale;
        scale.x = (float)(targetCharacter.curHealth - targetCharacter.PreviewDamage)/targetCharacter.attr.maxHealth;
        targetHealth.transform.localScale = scale;
      } else {
        Vector3 scale = targetHealth.transform.localScale;
        scale.x = (float)targetCharacter.curHealth/targetCharacter.attr.maxHealth;
        targetHealth.transform.localScale = scale;
      }
      blinkFrameNumber = (blinkFrameNumber + 1 )% 30;
      if (blinkFrameNumber == 0) {
        displayLowerHealth = !displayLowerHealth;
      }

      //other characters bars appear and blink
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
    if (SelectedPiece) {
      SelectedPiece.GetComponent<Renderer>().material.color = Color.white;
    }

    //get character whose turn it is
    //do something different for ai
    SelectedPiece = actionQueue.getNext();

    SelectedPiece.GetComponent<Renderer>().material.color = Color.red;
    line.SetPosition(0, SelectedPiece.transform.position);
    line.SetPosition(1, SelectedPiece.transform.position);


    Vector3 position = SelectedPiece.transform.position;
    djikstra(position);

    if (SelectedPiece.tag == "PiecePlayer2") {
      handleAI();
      return;
    }

    originalTile = getTile(position);

    changeState(GameState.moving);

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
    Skill skill = SelectedPiece.GetComponent<Character>().equippedSkills[i];

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
      cTarget.PreviewDamage = selectedCharacter.equippedSkills[SelectedSkill].calculateDamage(selectedCharacter, cTarget);
    }
    //todo: aoe stuff
  }

  public void attackTarget(GameObject target) {
    bool aoe = (SelectedPiece.GetComponent<Character>().equippedSkills[SelectedSkill] is AoeSkill);
    List<Character> targets = new List<Character>();
    List<GameObject> validTargets = SelectedPiece.GetComponent<Character>().equippedSkills[SelectedSkill].getTargets();

    if (validTargets.Contains(target)){
      if (aoe) {
        AoeSkill skill = SelectedPiece.GetComponent<Character>().equippedSkills[SelectedSkill] as AoeSkill;
        foreach (GameObject o in skill.getTargetsInAoe(target.transform.position)) {
          Character c = o.GetComponent<Character>();
          if (c) targets.Add(c);
        }
      } else {
        targets.Add(target.GetComponent<Character>());
      }
      SelectedPiece.GetComponent<Character>().equippedSkills[SelectedSkill].activate(targets);

      endTurn();
    }

  }

  public void endTurn() {
    SelectedPiece.GetComponent<Renderer>().material.color = Color.white;
    actionQueue.endTurn();
    clearColour();
    startTurn();
  }

  IEnumerator IterateMove(LinkedList<Tile> path) {
    const float FPS = 60f;
    const float speed = 4f;

    foreach (Tile destination in path) {
      // fix height
      Vector3 pos = destination.gameObject.transform.position;
      pos.y = destination.transform.position.y + getHeight(destination);

      // move piece
      Vector3 d = speed*(pos-SelectedPiece.transform.position)/FPS;
      for (int i = 0; i < FPS/speed; i++) {
        SelectedPiece.transform.Translate(d);
        yield return new WaitForSeconds(1/FPS);
      }
    }
    moving = false;
    for (int i = 0; i < skillButtons.Count; i++) {
      skillButtons[i].enabled = i < SelectedPiece.GetComponent<Character>().equippedSkills.Count;
    }
    changeState(GameState.attacking);
  }

  public bool moving {get; private set;}
  // Move the SelectedPiece to the inputted coords
  public void MovePiece(Vector3 coordToMove, bool immediate = true, bool doChangeState = false) {
    // don't start moving twice
    if (moving) return;

    Tile destination = getTile(coordToMove);
    Character c = SelectedPiece.GetComponent<Character>();
    Tile origin = c.curTile;


    if (destination.distance <= c.moveRange && !destination.occupied()) {
      //after moving, remove from origin tile,
      //add to new tile
      origin.occupant = null;
      c.curTile = destination;
      c.curTile.occupant = c.gameObject;

      coordToMove.y = destination.transform.position.y + getHeight(destination);
      if (immediate) {
        path.RemoveFirst(); // discard current position
        moving = true;
        for (int i = 0; i < skillButtons.Count; i++) {
          skillButtons[i].enabled = false;
        }
        line.GetComponent<Renderer>().material.color = Color.clear;
        StartCoroutine(IterateMove(new LinkedList<Tile>(path)));
      } else {
        SelectedPiece.transform.position = coordToMove;
        if (doChangeState) {
          changeState(GameState.attacking);
        }
      }
    }

    // To avoid concurrency problems, avoid putting any code after StartCoroutine.
    // Any code that should be executed when the coroutine finishes should just
    // go at the end of the coroutine.
  }

  public void cancelAction() {
    if (gameState == GameState.attacking) {
      Vector3 coordToMove = originalTile.gameObject.transform.position;
      MovePiece(coordToMove, false);
      changeState(GameState.moving);
    }
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
    MovePiece(destination, true);
    while(moving) {
      yield return new WaitForSeconds(time);
    }
    selectedCharacter.attackAI.target();
    unlockUI();
    endTurn();
  }
  public void handleAI() {
    StartCoroutine(doHandleAI(1));
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
    Ray ray;
    RaycastHit hitInfo;

    ray = PlayerCam.ScreenPointToRay(Input.mousePosition); // Specify the ray to be casted from the position of the mouse click
    // Raycast and verify that it collided
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
        if (tile.distance <= SelectedPiece.GetComponent<Character>().moveRange && !tile.occupied()) {
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
        foreach (GameObject o in skill.getTargetsInAoe(src.gameObject.transform.position)) {
          if (o.tag == "Cube") getTile(o.transform.position).gameObject.GetComponent<Renderer>().material.color = Color.yellow;
          else getTile(o.transform.position).gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
      }
    }
  }

  public void djikstra(Vector3 unitLocation) {
    foreach (Tile tile in tiles) {
      tile.distance = System.Int32.MaxValue/2;
      tile.dir = Direction.None;
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

      //above
      Vector3 neighbour = minTile.gameObject.transform.position + Vector3.forward;
      Tile neighbourTile = getTile(neighbour, tilesToGo);
      if (neighbourTile != null) {
        int d = minTile.distance + distance(neighbourTile, minTile);
        if (d < neighbourTile.distance) {
          neighbourTile.distance = d;
          neighbourTile.dir = Direction.Forward;
        }
        //neighbourTile.distance = Math.Min(minTile.distance + distance(neighbourTile, minTile), neighbourTile.distance);
      }
      //below
      neighbour = minTile.gameObject.transform.position + Vector3.back;
      neighbourTile = getTile(neighbour, tilesToGo);
      if (neighbourTile != null) {
        int d = minTile.distance + distance(neighbourTile, minTile);
        if (d < neighbourTile.distance) {
          neighbourTile.distance = d;
          neighbourTile.dir = Direction.Back;
        }
        //neighbourTile.distance = Math.Min(minTile.distance + distance(neighbourTile, minTile), neighbourTile.distance);
      }
      //right
      neighbour = minTile.gameObject.transform.position + Vector3.right;
      neighbourTile = getTile(neighbour, tilesToGo);
      if (neighbourTile != null) {
        int d = minTile.distance + distance(neighbourTile, minTile);
        if (d < neighbourTile.distance) {
          neighbourTile.distance = d;
          neighbourTile.dir = Direction.Right;
        }
        //neighbourTile.distance = Math.Min(minTile.distance + distance(neighbourTile, minTile), neighbourTile.distance);
      }
      //left
      neighbour = minTile.gameObject.transform.position + Vector3.left;
      neighbourTile = getTile(neighbour, tilesToGo);
      if (neighbourTile != null) {
        int d = minTile.distance + distance(neighbourTile, minTile);
        if (d < neighbourTile.distance) {
          neighbourTile.distance = d;
          neighbourTile.dir = Direction.Left;
        }
        //neighbourTile.distance = Math.Min(minTile.distance + distance(neighbourTile, minTile), neighbourTile.distance);
      }
      tilesToGo.Remove(minTile);
    }
  }

  public int distance(Tile from, Tile to) {
    //check heights
    if (Math.Abs(getHeight(from) - getHeight(to)) > 1.0f) {
      return Int32.MaxValue/2;
    }
    if (from.occupied() && !(from.occupant.tag.Equals(SelectedPiece.tag))) {
      return Int32.MaxValue/4;
    }
    return to.movePointSpent;
  }

  public float getHeight(Tile t) {
    float scale = t.gameObject.transform.localScale.y;
    return scale/2 + 0.5f;
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
    while (t.dir != Direction.None) {
      switch (t.dir) {
        case Direction.Forward:
          coord = coord - Vector3.forward;
          break;
        case Direction.Back:
          coord = coord - Vector3.back;
          break;
        case Direction.Left:
          coord = coord - Vector3.left;
          break;
        case Direction.Right:
          coord = coord - Vector3.right;
          break;
      }
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
}
