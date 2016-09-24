using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public float mouseSensitivity = 0.01f;
	private Vector3 lastPosition;
	private Vector3 rotationDirection = Vector3.up;
	private float rotationTime;
	private Transform preTransform;
	private Vector3 rotateAbout;
	private bool wasRotating;

	// Use this for initialization
	void Start () {
		preTransform =  new GameObject().transform;
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
		// create a plane at 0,0,0 whose normal points to +Y:
		Plane hPlane = new Plane(Vector3.up, Vector3.zero);
		// Plane.Raycast stores the distance from ray.origin to the hit point in this variable:
		float distance = 0; 
		// if the ray hits the plane...
		hPlane.Raycast(ray, out distance);
		rotateAbout = ray.GetPoint(distance);


		if (rotationTime > 0) {
			float rotationAmount = rotationTime < Time.deltaTime ? rotationTime : Time.deltaTime;
			transform.RotateAround (rotateAbout, rotationDirection, 90 * rotationAmount);
			rotationTime -= Time.deltaTime;
		} else if (wasRotating) {
			wasRotating = false;
			transform.rotation = preTransform.rotation;
		} else {
			if (Input.GetMouseButtonDown (0)) {
				lastPosition = Input.mousePosition;
			}

			if (Input.GetMouseButton (0)) {
				Vector3 delta = Input.mousePosition - lastPosition;
				Quaternion rotation = transform.rotation;
				Quaternion baseRotation = Quaternion.Euler (-45, 0, 0);
				Vector3 myVector = new Vector3 (delta.x * -mouseSensitivity, 0f, delta.y * -mouseSensitivity);
				Vector3 rotateVector = rotation * baseRotation * myVector;

				transform.Translate (rotateVector, Space.World);
				lastPosition = Input.mousePosition;
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
			wasRotating = true;
			rotationDirection = direction;
			rotationTime = 1.0f;
			preTransform.RotateAround(rotateAbout, rotationDirection, 90);
		}
	}
}
