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

	private double jumpCooldown = 1.0; // 1s
	private double _jumpWait = 0.0;

	private float WallAvoidanceRadius = 2.5f;

	private Ray ray;
	private RaycastHit hit;


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

			Vector3 force = transform.forward * Input.GetAxis ("Vertical")
				          + transform.right   * Input.GetAxis ("Horizontal");
			force *= moveSpeed * rb.mass * 80 * Time.deltaTime;


			// wall avoidance
			Vector3 npos = Pos + force.normalized * WallAvoidanceRadius;
			if (Physics.Linecast(Pos, npos, out hit, WallCollide.Walls)) {
				Debug.DrawLine(Pos, hit.point, Color.green);  // going until the wall
				Debug.DrawLine(hit.point, npos, Color.red); // going through the wall

				float factor = Mathf.Max(0.0f, hit.distance / WallAvoidanceRadius - 0.5f);
				// von der Wand abgehend
				force = (force * factor) + transform.up * (force.magnitude * (1f - factor) );
				Debug.DrawLine(Pos, Pos + force, Color.blue);
			}

			rb.AddForce (force);

			Debug.DrawLine (Pos, Pos + rb.velocity, Color.cyan);

			// jump cooldown
			if (Input.GetButtonDown ("Jump") && _jumpWait <= 0.0) {
				// not jumping down
				float angle = Mathf.Abs(Vector3.Angle (cam.transform.forward, transform.up));
				if (angle < 80.0) {
					rb.AddForce (jumpSpeed * rb.mass * cam.transform.forward, ForceMode.Impulse);
					//rb.velocity += jumpSpeed * rb.mass * cam.transform.forward;
					_jumpWait = jumpCooldown;
				}
			} else {
				_jumpWait = System.Math.Max(0.0, _jumpWait - Time.deltaTime);
			}

			// gravity if near walls
			ray = new Ray (Pos, -wc.Up);
			if (Physics.Raycast (ray, out hit, WallCollide.GroundedRadius, WallCollide.Walls)) {
				//rb.velocity += WallCollide.Gravity * rb.mass * -wc.Up * hit.distance / WallCollide.GroundedRadius * 50 * Time.deltaTime;
				rb.AddForce(WallCollide.Gravity * rb.mass * -wc.Up * hit.distance / WallCollide.GroundedRadius * 50 * Time.deltaTime);
			}

			//transform.Translate (Vector3.forward * Input.GetAxis ("Vertical") * moveSpeed * Time.deltaTime);
			//transform.Translate (Vector3.right * Input.GetAxis ("Horizontal") * moveSpeed * Time.deltaTime);
		} else {
			rb.drag = 0;
		}
	}
}
