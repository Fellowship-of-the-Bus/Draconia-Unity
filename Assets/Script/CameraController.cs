using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public float mouseSensitivity = 0.01f;
	private Vector3 lastPosition;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			lastPosition = Input.mousePosition;
		}

		if (Input.GetMouseButton(0))
		{
			Debug.Log("test");
			Vector3 delta = Input.mousePosition - lastPosition;
			transform.Translate(delta.x * -mouseSensitivity, delta.y * -mouseSensitivity, 0f);
			lastPosition = Input.mousePosition;
		}
	}
}
