﻿using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public float mouseSensitivity = 0.01f;
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

	// Use this for initialization
	void Start () {
		preTransform =  new GameObject().transform;
	}
	
	// Update is called once per frame
	void Update () {
		ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
    Plane hPlane;
    if (following == null) {
      // create a plane at 0,0,0 whose normal points to +Y:
      hPlane = new Plane(Vector3.up, Vector3.zero);
    } else {
      hPlane = new Plane(Vector3.up, following.transform.position);
    }
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
      if (panTime > 0f) {
        animatePan(panOrigin, following.transform.position);
      } else {
        animatingPan = false;
        lookAt(following.transform.position);
      }
    } else if (panTime > 0f) {
      animatePan(panOrigin, savedPosn);
    } else if (animatingPan) {
      transform.position = relativePosn + savedPosn;
      animatingPan = false;
    } else {
      float dx = 0;
      float dy = 0;

			if (Input.GetMouseButtonDown (0)) {
				lastPosition = Input.mousePosition;
			}

			if (Input.GetMouseButton (0)) {
				Vector3 delta = Input.mousePosition - lastPosition;
				lastPosition = Input.mousePosition;
        dx = delta.x;
        dy = delta.y;
			}
        
      if (Input.GetKey(KeyCode.UpArrow))
        dy = -5;
      if (Input.GetKey(KeyCode.DownArrow))
        dy = 5;
      if (Input.GetKey(KeyCode.LeftArrow))
        dx = 5;
      if (Input.GetKey(KeyCode.RightArrow))
        dx = -5;

      if (dx != 0 || dy != 0) {
        pan(dx, dy);
      }
		}
	}

	public void rotateLeft () {
		setupRotation (Vector3.down);
	}

	public void rotateRight () {
		setupRotation (Vector3.up);
	}

	private void setupRotation (Vector3 direction) {
		if (rotationTime <= 0) {
			preTransform.localRotation = transform.localRotation;
			preTransform.localPosition = transform.localPosition;
			preTransform.localScale = transform.localScale;
			rotating = true;
			rotationDirection = direction;
			rotationTime = 1.0f;
			preTransform.RotateAround(rotateAbout, rotationDirection, 90);
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
    }
    panOrigin = rotateAbout;
    relativePosn = transform.position - rotateAbout;
    following = o;
    panTime = maxPanTime;
    animatingPan = true;
  }

  public void unfollow() {
    panOrigin = following.transform.position;
    following = null;
    panTime = maxPanTime;
    animatingPan = true;
  }
}
