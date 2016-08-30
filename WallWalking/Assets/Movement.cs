using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

	WallCollide wc;
	Camera cam;
	Rigidbody rb;
	Vector3 Pos { get { return transform.position; } set { transform.position = value; } }
	Quaternion Rot { get { return transform.rotation; } set { transform.rotation = value; } }

	private float moveSpeed = 6; // move speed
	private float jumpSpeed = 12; // vertical jump initial speed

	private double jumpCooldown = 0.0;


	void Start () {
		rb = GetComponent<Rigidbody> ();
		wc = GetComponent<WallCollide> ();
		cam = GetComponentInChildren<Camera> ();
	}

	void Update () {
		if (wc.Grounded) {
			//TODO: slow down if unpressed
			rb.drag = 2;

			//rb.velocity = Vector3.zero;
			//rb.velocity += transform.forward * Input.GetAxis ("Vertical") * moveSpeed * rb.mass * 50 * Time.deltaTime;
			//rb.velocity += transform.right * Input.GetAxis ("Horizontal") * moveSpeed * rb.mass * 50 * Time.deltaTime;


			rb.AddForce (transform.forward * Input.GetAxis ("Vertical") * moveSpeed * rb.mass * 80 * Time.deltaTime);
			rb.AddForce (transform.right * Input.GetAxis ("Horizontal") * moveSpeed * rb.mass * 80 * Time.deltaTime);

			// todo jump cooldown
			if (Input.GetButtonDown ("Jump") && jumpCooldown <= 0.0) {
				rb.AddForce (jumpSpeed * rb.mass * cam.transform.forward, ForceMode.Impulse);
				//rb.velocity += jumpSpeed * rb.mass * cam.transform.forward;
				jumpCooldown = 1.0;
			} else {
				jumpCooldown = System.Math.Max(0.0, jumpCooldown - Time.deltaTime);
			}

			// gravity if near walls
			Ray ray;
			RaycastHit hit;
			ray = new Ray (Pos, -wc.Up);
			if (Physics.Raycast (ray, out hit, WallCollide.Radius, WallCollide.Walls)) {
				//rb.velocity += WallCollide.Gravity * rb.mass * -wc.Up * hit.distance / WallCollide.Radius * 50 * Time.deltaTime;
				rb.AddForce(WallCollide.Gravity * rb.mass * -wc.Up * hit.distance / WallCollide.Radius * 50 * Time.deltaTime);
			}

			//transform.Translate (Vector3.forward * Input.GetAxis ("Vertical") * moveSpeed * Time.deltaTime);
			//transform.Translate (Vector3.right * Input.GetAxis ("Horizontal") * moveSpeed * Time.deltaTime);
		} else {
			rb.drag = 0;
		}
	}
}
