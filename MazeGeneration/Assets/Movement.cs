using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

	Rigidbody rb;
	Vector3 Pos { get { return transform.position; } set { transform.position = value; } }
	Camera cam;

	public float moveSpeed = 6; // move speed


	void Start () {
		rb = GetComponent<Rigidbody> ();
		cam = GetComponentInChildren<Camera> ();
	}

	void Update () {
		Vector3 force =
			  cam.transform.forward * Input.GetAxis ("Vertical")
			+ cam.transform.right   * Input.GetAxis ("Horizontal")
			+ cam.transform.up * (Input.GetButton("Jump") ? 1f : 0f)
			- cam.transform.up * (Input.GetKey(KeyCode.LeftControl) ? 1f : 0f)
		;
		force *= moveSpeed * rb.mass * 80 * Time.deltaTime;
		rb.AddForce (force);
	}
}
