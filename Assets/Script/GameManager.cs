using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Linq;

public enum GameState {
  moving,
  attacking,
  ending
}

public class GameManager : MonoBehaviour {
  // Selected Piece
  public Map map = new Map();
  public SkillButton[] skillButtons;
  LineRenderer line;
  public ActionQueue actionQueue;
  public BuffBar activeBuffBar;
  public BuffBar targetBuffBar;
  public GameObject buffButton;
  public PlayerControl pControl;

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
  public BattleCharacter SelectedPiece { get; private set;}
  public int SelectedSkill {get; set;}
  [HideInInspector]
  public List<Tile> skillTargets;
  [HideInInspector]
  private BattleCharacter previewTarget;

  [HideInInspector]
  public GameState gameState = GameState.moving;

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
  public HealthBarManager selectedHealth;
  public HealthBarManager targetHealth;
  public GameObject targetPanel;
  public GameObject mainUI;

  //skillbutton tooltip
  public GameObject tooltip;

  //tile information
  public TileInfo tInfo;

  //main dialogue controller
  public Dialogue dialogue;
  public DialogueReader reader;

  //Map Boss
  public BattleCharacter boss;

  public Dictionary<BattleCharacter.Team, List<BattleCharacter>> characters = new Dictionary<BattleCharacter.Team, List<BattleCharacter>>();
  public List<BattleCharacter> players { get{ return characters[BattleCharacter.Team.Player]; } }
  public List<BattleCharacter> enemies { get{ return characters[BattleCharacter.Team.Enemy]; } }
  private List<Coroutine> waitingOn = new List<Coroutine>();

  public Material[] minimapIcons;

  private List<BFEvent> BFevents = new List<BFEvent>();

  private class CharacterListener : EventListener {
    public override void onEvent(Draconia.Event e) {
      if (e.hook == EventHook.postDeath) {
        GameManager g = GameManager.get;
        g.characters[e.sender.team].Remove(e.sender);
        if (e.sender == g.SelectedPiece) {
          g.endTurnWrapper();
        }
      }
    }
  }

  private CharacterListener characterListener = new CharacterListener();

  public class PostGameData {
    public string mapName;
    public bool win;
    public List<Character> inBattle;
    public List<Equipment> loot;
  }
  public static PostGameData postData = new PostGameData();

  public Button cancelButton;

  IEnumerator waitForSeconds(float s) {
    yield return new WaitForSeconds(s);
  }

  IEnumerator popAtEnd(Coroutine c, Action act) {
    yield return c;
    if (act != null) act();
    waitingOn.Remove(c);
  }

  public IEnumerator waitUntilPopped(Coroutine c) {
    while (waitingOn.Contains(c)) {
      yield return null;
    }
  }

  public IEnumerator waitUntilEmpty() {
    while (waitingOn.Count > 0) {
      yield return null;
    }
  }

  public Coroutine waitFor(float s, Action act = null) {
    Coroutine c = StartCoroutine(waitForSeconds(s));
    waitFor(c, act);
    return c;
  }

  IEnumerator waitForAnimation(Animator animator, String trigger) {
    if (Options.displayAnimation) {
      animator.SetTrigger(trigger);
      AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
      float time = 0f;
      foreach(AnimationClip c in clips) {
        string s = c.name;
        if (s == trigger) time = c.length;
      }
      yield return new WaitForSeconds(time);
    } else {
      yield return new WaitForEndOfFrame();
    }
  }

  public Coroutine waitFor(Animator a, String trigger, Action act = null) {
    Coroutine c = StartCoroutine(waitForAnimation(a, trigger));
    waitFor(c, act);
    return c;
  }

  public Coroutine waitFor(Coroutine c, Action act = null) {
    waitingOn.Add(c);
    StartCoroutine(popAtEnd(c, act));
    return c;
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

    map.awake();

    line = gameObject.GetComponent<LineRenderer>();

    //set skill buttons to the selected piece's skills, enable those that actually have skills
    new Range(0, skillButtons.Length).ForEach(i => {
      skillButtons[i].button.onClick.AddListener(() => selectSkill(i));
      skillButtons[i].button.enabled = true;
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
    lockUI();
    dialogue.setOnExit(() => GameSceneController.get.pControl.enabled = true);

    var chars = GameObject.FindGameObjectsWithTag("Unit").Select(x => x.GetComponent<BattleCharacter>());
    var objs = chars.GroupBy(x => x.team);
    foreach (var x in objs) {
      characters[x.Key] = new List<BattleCharacter>(x);
    }

    foreach (var l in characters.Values) {
      foreach (var c in l) {
        Tile t = map.getTile(c.gameObject.transform.position);
        c.transform.position = t.position;
        t.occupant = c;
        c.curTile = t;
      }
    }

    foreach (var l in characters.Values) {
      foreach (var bchar in l) {
        bchar.init();
        bchar.ai.init();
        characterListener.attachListener(bchar, EventHook.postDeath);
        actionQueue.add(bchar); //Needs to be done here since it relies on characters having their attribute set
      }
    }
    List<BattleCharacter> users = new List<BattleCharacter>(players.Filter(x => x.aiType == AIType.None));
    List<Character> charInBattle = new List<Character>(users.Map(x => x.baseChar));
    GameManager.postData.inBattle = charInBattle;
  }

  void Update() {
    cancelButton.interactable = cancelStack.Count != 0;

    //enable the line only when attacking
    line.enabled = gameState == GameState.attacking;
    ActiveSkill s;
    int netChange = 0;
    List<Effect> effects = new List<Effect>(SelectedPiece.getEffects());
    effects.AddRange(new List<Effect>(SelectedPiece.curTile.getEffects()));

    foreach(Effect e in effects) {
      if (e is HealthChangingEffect) {
        netChange += (e as HealthChangingEffect).healthChange();
      }
    }


/*    if (netChange < 0) {
      selectedCharacter.updateLifeBar(selectedHealthBar, selectedCharacter.curHealth + netChange);
    } else if (netChange > 0) {
      selectedCharacter.updateLifeBar(selectedHealingBar, selectedCharacter.curHealth + netChange);
    } else {
      selectedCharacter.updateLifeBar(selectedHealthBar, selectedCharacter.curHealth);
    }
    */

    // Uncomment this code if you need to change skills via the editor ingame.
    // if (Options.debugMode && selectedCharacter.team == 0) {
      // for (int i = 0; i < selectedCharacter.equippedSkills.Count; i++) {
      //   s = selectedCharacter.equippedSkills[i];
      //   Debug.AssertFormat(s.name != "", "Skill Name is empty");

      //   // Set the text on the button TODO: Remove this when every skill has an icon
      //   skillButtons[i].GetComponentInChildren<Text>().text = s.name;

      //   // Set the button image
      //   Sprite skillImage = SkillList.get.skillImages[s.GetType()];
      //   GameObject imageObject = skillButtons[i].transform.Find("SkillImage").gameObject;
      //   Image skillDisplay = imageObject.GetComponent<Image>();
      //   Image cooldownDisplay = imageObject.transform.Find("CooldownOverlay").gameObject.GetComponent<Image>();
      //   if (skillImage == null) {
      //     skillDisplay.enabled = false;
      //   } else {
      //     skillDisplay.sprite = skillImage;
      //     skillDisplay.enabled = true;
      //   }

      //   // Set the tooltip
      //   skillButtons[i].gameObject.GetComponent<Tooltip>().tiptext = s.tooltip;

      //   // Cooldown indication
      //   bool buttonEnabled = s.canUse();
      //   skillButtons[i].interactable = buttonEnabled;
      //   if (buttonEnabled) {
      //     cooldownDisplay.fillAmount = 0f;
      //   } else {
      //     cooldownDisplay.fillAmount = (float)s.curCooldown / (float)(s.maxCooldown + 1);
      //   }
      // }
    // }

    if (previewTarget == null) {
      targetPanel.SetActive(false);
    }

    if (SelectedSkill == -1)  return;

    // Preview damage and healing for the selected skill
    s = SelectedPiece.equippedSkills[SelectedSkill];
    Tile currTile = pControl.currentHoveredTile;
    if (currTile != null && skillTargets.Contains(currTile)) {
      List<Tile> tiles = new List<Tile>();
      if (s is AoeSkill) {
        tiles = (s as AoeSkill).getTargetsInAoe(currTile.position);
      }

      foreach(Tile t in tiles) {
        BattleCharacter c = t.occupant;
        if (c == null) continue;
        if (s is HealingSkill) {
          c.PreviewChange = s.calculateHealing(c);
        } else {
          c.PreviewChange = -1 * s.calculateDamage(c);
        }
        c.updateLifeBars(c.PreviewChange);
      }
    }

    if (previewTarget) {
      targetBuffBar.update(previewTarget);
      targetPanel.SetActive(true);
      previewTarget.updateLifeBars(previewTarget.PreviewChange);
      if (s.canTarget(previewTarget.curTile)) {
        targetHealth.update(previewTarget.PreviewChange);
      }
    }
  }

  // Game State functions //

  void changeState(GameState newState) {
    if (newState == GameState.moving) {
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
    SelectedPiece.onEvent(new Draconia.Event(SelectedPiece, EventHook.startTurn));
    moveRange = SelectedPiece.moveRange;
    activeBuffBar.update(SelectedPiece);
    selectedHealth.setCharacter(SelectedPiece);

    line.SetPosition(0, SelectedPiece.transform.position);
    line.SetPosition(1, SelectedPiece.transform.position);


    Vector3 position = SelectedPiece.transform.position;
    map.djikstra(position, SelectedPiece);

    changeState(GameState.moving);
    // AI's
    if (SelectedPiece.team != 0 || SelectedPiece.aiType != AIType.None) {
      handleAI();
      return;
    }

    cam.panTo(SelectedPiece.transform.position);

    cancelStack.Clear();

    for (int i = 0; i < skillButtons.Length; i++) {
      skillButtons[i].button.interactable = false;
      skillButtons[i].image.enabled = false;
      skillButtons[i].tooltip.tiptext = "";
    }

    for (int i = 0; i < SelectedPiece.equippedSkills.Count; i++) {
      ActiveSkill s = SelectedPiece.equippedSkills[i];
      Debug.AssertFormat(s.name != "", "Skill Name is empty");
      skillButtons[i].tooltip.tiptext = s.tooltip;
      skillButtons[i].button.interactable = s.canUse();
      Image skillDisplay = skillButtons[i].image;
      Sprite skillImage = SkillList.get.skillImages[s.GetType()];
      if (skillImage == null) {
        skillDisplay.enabled = false;
      } else {
        skillDisplay.sprite = skillImage;
        skillDisplay.enabled = true;
      }
      Image cooldownDisplay = skillButtons[i].cooldown;
      if (s.canUse()) {
        cooldownDisplay.fillAmount = 0f;
      } else {
        cooldownDisplay.fillAmount = (float)s.curCooldown / (float)(s.maxCooldown + 1);
      }
    }
    unlockUI();
  }


  public void selectSkill(int i) {
    //unselect
    if (gameState == GameState.attacking) {
      int currentSkill = SelectedSkill;
      while(!cancelStack.Pop().undo());
      if (currentSkill == i) {
        return;
      }
    }

    SelectedSkill = i;
    ActiveSkill skill = SelectedPiece.equippedSkills[i];

    skillTargets = skill.getTargets();
    //change colours of the tiles for attacking
    //check for range skill if not put 1 else put the range
    changeState(GameState.attacking);

    cancelStack.Push(new UndoAction(() => {


      changeState(GameState.moving);
    }, true));
  }

  // Preview of targetting a character
  public void selectTarget(GameObject target) {
    if (previewTarget) {
      previewTarget.PreviewChange = 0;
      // if (targetHealth != null) targetHealth.update();
      selectedHealth.update(previewTarget.PreviewChange);
      previewTarget.updateLifeBars();
      previewTarget = null;
    }

    if (target != null) {
      BattleCharacter targetChar = target.GetComponent<BattleCharacter>();
      previewTarget = targetChar;
      targetHealth.setCharacter(previewTarget);

      if (targetChar == null) {
        actionQueue.highlight(null);
        return;
      }

      actionQueue.highlight(targetChar);
      if (SelectedSkill != -1) {
        ActiveSkill skill = SelectedPiece.equippedSkills[SelectedSkill];
        HealingSkill hskill = skill as HealingSkill;
        if (hskill != null) previewTarget.PreviewChange = skill.calculateHealing(previewTarget);
        else previewTarget.PreviewChange = -1 * skill.calculateDamage(previewTarget);
      }
      if (previewTarget == SelectedPiece) selectedHealth.update(previewTarget.PreviewChange);
    }
  }

  [HideInInspector]
  public List<Tile> targets = new List<Tile>();
  public void attackTarget(Tile target) {
    ActiveSkill skill = SelectedPiece.equippedSkills[SelectedSkill];
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

    foreach(Tile t in targets) {
      if (t.occupied()) t.occupant.updateLifeBars(t.occupant.PreviewChange);
    }
    if (previewTarget) targetHealth.update(previewTarget.PreviewChange);

    if (SelectedPiece.useSkill(skill, targets)) {
      SelectedSkill = -1;
      endTurnWrapper();
    }
    targets.Clear();
  }

  public void endTurnWrapper() {
    StartCoroutine(endTurn());
  }

  public IEnumerator endTurn() {
    if (gameState != GameState.ending) {
      if (SelectedPiece.team == 0 && SelectedPiece.aiType == AIType.None) {
        lockUI();
      }

      changeState(GameState.ending);
      targets.Clear();

      // Wait for turn events to finish
      yield return StartCoroutine(waitUntilEmpty());

      //send endTurn Draconia.Event to the current piece
      Draconia.Event e = new Draconia.Event(null, EventHook.endTurn);
      e.endTurnChar = SelectedPiece;
      e.nextCharTime = actionQueue.peekNext();
      eventManager.onEvent(e);
      SelectedPiece.onEvent(new Draconia.Event(SelectedPiece, EventHook.endTurn));

      // Wait for end turn events to complete
      yield return StartCoroutine(waitUntilEmpty());

      actionQueue.endTurn();
      map.clearColour();
      startTurn();
    }
  }

  public IEnumerator moveObject(
    GameObject obj,
    float speed,
    Vector3 start,
    Vector3 end,
    int startFrame,
    int totalFrames,
    Animator animator,
    float heightChange = 0.5f,
    Action<float> extra = null,
    bool movingPiece = true,
    bool arcing = false
    ) {
    float hopHeight = Math.Max(end.y, start.y) + heightChange;
    float dUp = speed*2*(hopHeight - start.y)/Options.FPS;
    float dDown = speed*2*(end.y - hopHeight)/Options.FPS;
    int curFrame = startFrame;
    for (int i = 0; i < Options.FPS/speed; i++) {
      float pct = curFrame * 1.0f / totalFrames;
      if (movingPiece) animator.SetFloat("Blend", (pct < 0.1) ? pct*10 : ((pct > 0.90) ? (1-pct)*10 : 1));
      Vector3 d = speed*(end-start)/Options.FPS;

      // Arcing or hopping
      if ((d.y != 0 && movingPiece) || (!movingPiece && arcing)) {
        if (i < Options.FPS/(speed*2)) {
          d.y = dUp * Mathf.Lerp(1,0,i/(Options.FPS/(speed*2)));
        } else {
          d.y = dDown * Mathf.Lerp(0,1,(i - Options.FPS/(speed*2))/(Options.FPS/(speed*2)));
        }
        if (extra != null) extra(i/(Options.FPS/(speed)));
      }

      obj.transform.Translate(d, Space.World);
      curFrame++;
      yield return new WaitForSeconds(1/Options.FPS);
    }
  }

  public IEnumerator IterateMove(LinkedList<Tile> path, BattleCharacter character, int index, bool setWalking, bool teleport) {
    const float speed = 3f;
    lockUI();
    GameObject piece = character.gameObject;

    if (gameState == GameState.moving && setWalking) {
      cam.follow(SelectedPiece.gameObject);
      yield return new WaitForSeconds(0.5f);
    }

    Animator animator = character.model.animator;
    if (setWalking && animator) {
      animator.SetBool("isWalking", true);
      animator.SetBool("isNinja", character.isNinja);
    }
    Tile endpoint = path.Last.Value;
    int moveFrames = (int)(path.Count * Options.FPS/speed);
    int curFrame = 0;

    foreach (Tile destination in path) {
      // fix height
      Vector3 pos = destination.transform.position;
      pos.y = map.getHeight(destination);

      bool usingPortal = destination.dir == Map.portalDir;

      if (!usingPortal && !teleport) {
        // Set Rotation
        if (setWalking) character.face(pos);
        // Move Piece
        yield return moveObject(piece, speed, piece.transform.position, pos, curFrame, moveFrames, animator);
        curFrame += (int)(Options.FPS/speed);
        piece.transform.Translate(pos-piece.transform.position);
      }
      // tell listeners that this character moved
      Draconia.Event enterEvent = new Draconia.Event(character, EventHook.enterTile);
      enterEvent.position = destination.transform.position;
      EventManager.get.onEvent(enterEvent);
      if (enterEvent.interruptMove) {
        cancelStack.Clear();
        yield return new WaitForSeconds(0.5f);
      }
      if (!character.isAlive()) break;
      if (animator) animator.enabled = false;
      //yield return waitUntilCount(index+1); //Not needed anymore since the move is interrupted?
      if (animator) animator.enabled = true;
    }
    if (character.isAlive()) {
      piece.transform.position = endpoint.position;
      moveRange -= endpoint.distance;
      updateTile(character,endpoint);
    }
    map.clearPath();
    map.djikstra(endpoint.position, character);
    map.setTileColours();

    if (setWalking && animator) {
      animator.SetBool("isWalking", false);
    }

    if (gameState == GameState.moving && setWalking) {
      yield return new WaitForSeconds(0.25f);
      cam.unfollow();
    }

    moving = false;
    unlockUI();
  }

  // temporarily public
  public bool moving {get; /*private*/ set;}
  // Move the SelectedPiece to the inputted coords


  public void updateTile(BattleCharacter c, Tile t) {
    eventManager.onEvent(new Draconia.Event(c, EventHook.preMove));
    c.curTile.occupant = null;
    c.curTile = t;
    t.occupant = c;
    eventManager.onEvent(new Draconia.Event(c, EventHook.postMove));
  }

  /** remaining move amount */
  [HideInInspector]
  public int moveRange = 0;
  // Walk the selected character to a destination
  public Coroutine movePiece(Vector3 coordToMove) {
    // don't start moving twice
    if (moving) return null;
    LinkedList<Tile> localPath = new LinkedList<Tile>(map.path);

    Tile destination = map.getTile(coordToMove);
    Coroutine co = null;

    if (destination.distance <= moveRange && !destination.occupied()) {
      // update position stack with current values,
      // update remaining move range, and recolor the tiles given the new current position
      int i = moveRange;
      Tile cur = SelectedPiece.curTile;
      cancelStack.Push(new UndoAction(() => {
        Vector3 coord = cur.transform.position;
        moveRange = i;
        SelectedPiece.transform.position = cur.position;
        updateTile(SelectedPiece, cur);
        changeState(GameState.moving);
        map.djikstra(coord, SelectedPiece);
        map.setTileColours();
      }));

      //after moving, remove from origin tile,
      //add to new tile

      coordToMove.y = destination.transform.position.y + map.getHeight(destination);
      localPath.RemoveFirst(); // discard current position
      line.GetComponent<Renderer>().material.color = Color.clear;
      moving = Options.displayAnimation;
      co = StartCoroutine(IterateMove(localPath, SelectedPiece, waitingOn.Count, true, !Options.displayAnimation));
      waitFor(co);
    }
    return co;
    // To avoid concurrency problems, avoid putting any code after StartCoroutine.
    // Any code that should be executed when the coroutine finishes should just
    // go at the end of the coroutine.
  }

  // Move a character along a path of tiles
  public void movePiece(BattleCharacter c, LinkedList<Tile> path, bool setWalking = true) {
    moving = Options.displayAnimation;
    waitFor(StartCoroutine(IterateMove(
      path,
      c,
      waitingOn.Count,
      setWalking && Options.displayAnimation,
      !Options.displayAnimation
    )));
  }

  public void cancelAction() {
    if (cancelStack.Count() > 0) {
      // reset character to previous position and remaining move range, then recolor movable tiles
      UndoAction act = cancelStack.Pop();
      act.undo();
    }
    eventManager.onEvent(new Draconia.Event(SelectedPiece, EventHook.cancel));
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
    GameManager.postData.mapName = SceneManager.GetActiveScene().name;
    LoadingScreen.load("PostMap");
  }

  IEnumerator doHandleAI(int time) {
    Vector3 destination = SelectedPiece.ai.move();
    map.setTileColours();
    Tile t = map.getTile(destination);
    while(t != map.path.Last.Value) {
      map.path.RemoveLast();
    }

    yield return waitUntilPopped(movePiece(destination));
    if (SelectedPiece.ai.willAttack()) {
      if (SelectedPiece.isAlive()) {
        yield return waitUntilPopped(waitFor(StartCoroutine(AIperformAttack(SelectedPiece))));
      }
    }

    StartCoroutine(endTurn());
  }

  public void handleAI() {
    waitFor(StartCoroutine(doHandleAI(1)));
  }

  public IEnumerator AIperformAttack(BattleCharacter selectedCharacter) {
    cam.follow(SelectedPiece.gameObject);
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
      Debug.Assert(UILock.count >= 0);
      if (UILock.count == 0) {
        mainUI.GetComponent<CanvasGroup>().interactable = true;
      }
    }
  }

  public bool UILocked() {
    return UILock.count != 0;
  }

  public bool checkLine(Vector3 source, Vector3 target, Collider targetCollider) {
    RaycastHit info;
    return checkLine(source, target, out info, targetCollider);
  }

  public bool checkLine(Vector3 source, Vector3 target, out RaycastHit info, Collider targetCollider) {
    Vector3 toTarget = target - source;
    Ray ray = new Ray(source, toTarget.normalized);
    RaycastHit hitInfo;
    Physics.Raycast(ray, out hitInfo);
    info = hitInfo;

    if (hitInfo.collider == null) return false;
    return (hitInfo.collider == targetCollider);
  }

  // Draw line to piece
  public void lineTo(GameObject piece) {
    if (SelectedPiece && piece) {
      if (SelectedPiece.gameObject == piece) {
        line.SetPosition(1, SelectedPiece.transform.position);
        line.GetComponent<Renderer>().material.color = Color.clear;
      } else {
        Vector3 source = getTargetingPostion(SelectedPiece.gameObject);
        Vector3 target = getTargetingPostion(piece);
        RaycastHit hitInfo;
        if (checkLine(source, target, out hitInfo, piece.GetComponent<Collider>())) {
          line.GetComponent<Renderer>().material.color = Color.red;
        } else {
          line.GetComponent<Renderer>().material.color = Color.black;
        }

        line.SetPosition(0, source);
        line.SetPosition(1, hitInfo.point);
      }
    }
  }

  // Get the point from which projectiles originate from or hit a character
  public Vector3 getTargetingPostion(GameObject piece) {
    float height = piece.GetComponent<MeshFilter>().mesh.bounds.extents.y;

    return new Vector3(
      piece.transform.position.x,
      piece.transform.position.y + 2*height/3,
      piece.transform.position.z
    );
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

  public GameObject playerPrefab;
  // Test function that instantiates a character
  public BattleCharacter createPiece() {
    GameObject newCharObj = Instantiate(playerPrefab, new Vector3(0f, 1f, 0f), Quaternion.identity, GameObject.Find("Players").transform) as GameObject;
    var c = newCharObj.GetComponent<BattleCharacter>();
    return c;
  }

  public static GameManager get { get; set; }

}
