using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
  public float mouseSensitivity = 1f;
  public float scrollSensitivity = 10f;
  private Vector3 lastPosition;
  private Vector3 rotationDirection = Vector3.up;
  private float rotationTime;
  private Transform preTransform;
  private Vector3 rotateAbout;
  private bool rotating;

  Ray ray;
  float distance = 0;

  private GameObject following = null;
  private Vector3 savedPosn;
  private Vector3 relativePosn;
  private Vector3 panOrigin;
  float panTime = 0f;
  bool animatingPan = false;
  const float maxPanTime = 0.25f;

  private bool shaking = false;
  private float shakeDuration;
  private float shakeIntensity = 0.3f;

  Quaternion rot;
  Plane hPlane;

  // Use this for initialization
  void Start () {
    GameObject o = new GameObject("Previous transform");
    o.transform.SetParent(transform);
    preTransform =  o.transform;
    // create a plane at 0,0,0 whose normal points to +Y:
    hPlane = new Plane(Vector3.up, Vector3.zero);
  }

  // Update is called once per frame
  void Update () {
    ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
    // Plane.Raycast stores the distance from ray.origin to the hit point in this variable:
    distance = 0;
    // if the ray hits the plane...
    hPlane.Raycast(ray, out distance);
    rotateAbout = ray.GetPoint(distance);

    float scroll = Input.GetAxis("Mouse ScrollWheel");
    if ((scroll > 0 && transform.position.y > 5f) || (scroll < 0 && transform.position.y < 15f)) {
      float scrollFactor = scrollSensitivity / (Mathf.Abs(transform.position.y - 10f) + 1);
      transform.Translate (Vector3.forward * scroll * scrollFactor);
    }

    if (rotating && rotationTime > 0) {
      float rotationAmount = rotationTime < Time.deltaTime ? rotationTime : Time.deltaTime;
      transform.RotateAround(rotateAbout, rotationDirection, 90 * rotationAmount);
      rotationTime -= Time.deltaTime;
    } else if (rotating) {
      rotating = false;
      transform.rotation = preTransform.rotation;
    } else if (following != null) {
      Vector3 targetPosn = new Vector3(following.transform.position.x, 0, following.transform.position.z);
      Vector3 forward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
      forward = forward * following.transform.position.y;
      targetPosn = targetPosn + forward;
      if (panTime > 0f) {
        animatePan(panOrigin, targetPosn);
      } else {
        lookAt(targetPosn);
      }
    } else if (panTime > 0f) {
      animatePan(panOrigin, savedPosn);
    } else if (animatingPan) {
      transform.position = relativePosn + savedPosn;
      animatingPan = false;
    } else if (shakeDuration > 0f) {
      shakeDuration -= Time.deltaTime;
      transform.position = Vector3.Lerp(savedPosn,
        savedPosn + (Random.insideUnitSphere * shakeIntensity), shakeDuration);
    } else if (shaking) {
      shaking = false;
      transform.position = savedPosn;
    } else {
      float dx = 0;
      float dy = 0;

      if (Input.GetMouseButtonDown(0)) {
        lastPosition = Input.mousePosition;
      }

      if (Input.GetMouseButton (0)) {
        Vector3 curr = Input.mousePosition;
        Vector3 delta = curr - lastPosition;
        var olddx = delta.x;
        var olddy = delta.y;

        Ray rayCurr = Camera.main.ScreenPointToRay(Input.mousePosition);
        Ray rayPrev = Camera.main.ScreenPointToRay(lastPosition);

        float distCurr = 0f;
        float distPrev = 0f;

        if (hPlane.Raycast(rayCurr, out distCurr) && hPlane.Raycast(rayPrev, out distPrev)) {
          Vector3 worldCurr = rayCurr.GetPoint(distCurr);
          Vector3 worldPrev = rayPrev.GetPoint(distPrev);

          Vector3 forward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;

          Vector3 worldDelta = worldCurr - worldPrev;

          Vector3 proj = Vector3.Project(worldDelta, forward);
          Vector3 proj2 = Vector3.Project(worldDelta, new Vector3(forward.z, 0, -forward.x));

          dx = proj2.x / forward.z;
          dy = proj.x / forward.x;
        }
      }

      float moveFactor = transform.position.y * 2 * 0.01f;
      if (Input.GetKey(KeyCode.UpArrow))
        dy = -1 * moveFactor;
      if (Input.GetKey(KeyCode.DownArrow))
        dy = moveFactor;
      if (Input.GetKey(KeyCode.LeftArrow))
        dx = moveFactor;
      if (Input.GetKey(KeyCode.RightArrow))
        dx = -1 * moveFactor;

      if (dx != 0 || dy != 0) {
        pan(dx, dy);
      }
    }
    lastPosition = Input.mousePosition;
  }

  void LateUpdate() {
    preTransform.rotation = rot;
  }

  public void rotateLeft () {
    setupRotation (Vector3.down);
  }

  public void rotateRight () {
    setupRotation (Vector3.up);
  }

  private void setupRotation (Vector3 direction) {
    if (rotationTime <= 0) {
      preTransform.rotation = transform.localRotation;
      preTransform.position = transform.localPosition;
      preTransform.localScale = transform.localScale;
      rotating = true;
      rotationDirection = direction;
      rotationTime = 1.0f;
      preTransform.RotateAround(rotateAbout, rotationDirection, 90);
      rot = preTransform.rotation;
    }
  }

  private void pan(float x, float y) {
    Quaternion rotation = transform.rotation;
    Quaternion baseRotation = Quaternion.Euler (-45, 0, 0);
    Vector3 myVector = new Vector3 (x * -mouseSensitivity, 0f, y * -mouseSensitivity);
    Vector3 rotateVector = rotation * baseRotation * myVector;

    transform.Translate (rotateVector, Space.World);
  }

  private void lookAt(Vector3 p) {
    transform.position = relativePosn + p;
  }

  private void animatePan(Vector3 start, Vector3 end) {
    panTime -= Time.deltaTime;
    transform.position += (end - start) * Time.deltaTime * (1.0f / maxPanTime);
  }

  public void follow(GameObject o) {
    if (!animatingPan) {
      savedPosn = rotateAbout;
      relativePosn = transform.position - rotateAbout;
    }
    panOrigin = transform.position - relativePosn;
    following = o;
    panTime = maxPanTime;
    animatingPan = true;
  }

  public void unfollow(bool restore = false) {
    if (restore) {
      panOrigin = following.transform.position;
      panTime = maxPanTime;
    } else {
      animatingPan = false;
    }
    following = null;
  }

  public void panTo(Vector3 target) {
    Vector3 forward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
    forward = forward * target.y;
    savedPosn = new Vector3(target.x, 0, target.z) + forward;
    relativePosn = transform.position - rotateAbout;
    panOrigin = transform.position - relativePosn;
    panTime = maxPanTime;
    animatingPan = true;
  }

  public void screenShake() {
    // TODO: make it work while panning, make additional shaking add to it.
    if (!shaking && !animatingPan && !rotating) {
      shaking = true;
      shakeDuration = 1.0f;
      savedPosn = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }
  }
}
