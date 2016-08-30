using UnityEngine;
using System.Collections;

public class WallCollide : MonoBehaviour {
	Rigidbody rb;


	public const int Walls = 1 << 8;
	public const float Gravity = 3f;
	public const float GroundedRadius = 3.0f;
	public const float NormalRadius = 2.0f;

	public bool Grounded { get; private set; }
	public Vector3 Up { get; private set; }

	void Start() {
		rb = GetComponent<Rigidbody> ();
		Up = transform.up;
	}

	void FixedUpdate() {
		string o = "";
		Vector3 Pos = rb.worldCenterOfMass;
		Vector3 up = Vector3.zero;

		// isGrounded?
		Collider[] hits = Physics.OverlapSphere(Pos, GroundedRadius, Walls);
		Grounded = hits.Length > 0;

		hits = Physics.OverlapSphere(Pos, NormalRadius, Walls);
		foreach (Collider hit in hits) {
			Vector3 HitPos = hit.ClosestPointOnBounds (Pos);
			float dist = Vector3.Distance (Pos, HitPos);
			o += hit.name + ": " + dist + ", ";

			Vector3 v = Pos - HitPos;
			v = v.normalized * (1f / v.magnitude);
			//Debug.DrawLine (HitPos, Pos, Color.green); // all connected walls

			up += v;
		}
		//Debug.Log (o);

		up.Normalize();
		if (up != Vector3.zero) {
			Up = Vector3.Slerp (Up, up, 3f * Time.fixedDeltaTime);
		}

		// prevent standing on head
		if (hits.Length == 1) {
			float angle = Vector3.Angle (Up, up);
			//Debug.Log(angle);
			if (angle > 120.0) {
				rb.AddForceAtPosition (transform.right, transform.position + transform.up);
			}
		}

		// Orientation
		//Debug.DrawLine (Pos, Pos + Up * 3f, Color.red);
		//Debug.DrawLine (Pos, Pos + transform.forward * 3f, Color.blue);

		/*if (Grounded) {
			// gravity
			Ray ray;
			RaycastHit hit;
			ray = new Ray (Pos, -Up);
			if (Physics.Raycast (ray, out hit, Radius, Walls)) {
				rb.AddForce(Gravity * rb.mass * -Up * hit.distance / Radius);
			}
		}*/

		float lerpSpeed = 15.5f;
		Vector3 myNormal = Vector3.Lerp(transform.up, up, lerpSpeed * Time.fixedDeltaTime);
		Vector3 myForward = Vector3.Cross(transform.right, myNormal);
		Quaternion targetRot = Quaternion.LookRotation(myForward, myNormal);
		//transform.rotation = targetRot;
		transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, lerpSpeed * Time.fixedDeltaTime);

		/*
		//Quaternion q = Quaternion.FromToRotation (transform.up, Up);
		//Quaternion q = Quaternion.FromToRotation (transform.up, Up);
		//transform.rotation = Quaternion.Slerp(transform.rotation, q, 0.5f * Time.fixedDeltaTime);

		//rb.AddForce (transform.forward * Input.GetAxis ("Horizontal") * Time.fixedDeltaTime * 8f, ForceMode.Acceleration);
		//rb.AddForce (transform.right * Input.GetAxis ("Vertical") * Time.fixedDeltaTime * -8f, ForceMode.Acceleration);
		*/
	}

	/*void OnCollisionEnter (Collision col) {
		Debug.Log (col.gameObject.name);
	}*/
}
