using UnityEngine;
using System.Collections;

public class ScaleTimeInSpace : MonoBehaviour {
	public Rigidbody rigidbody;
	private bool customTime;
	private float gravityScale;
	// Use this for initialization
	void Awake () {
		rigidbody = gameObject.GetComponent<Rigidbody> ();
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.T)) {
			rigidbody.velocity = -Vector3.forward * 30;
		}
	}

	void FixedUpdate () {
		if (customTime) {
			rigidbody.AddForce (Physics.gravity * gravityScale);// * Time.fixedTime;
		}
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Time") {
			rigidbody.velocity /= 10f;
			rigidbody.angularVelocity /= 10f;
			rigidbody.useGravity = false;
			gravityScale = 0.1f;
			customTime = true;
		}
	}

	void OnTriggerExit(Collider col)
	{
		if (col.gameObject.tag == "Time") {
			rigidbody.velocity *= 10f;
			rigidbody.angularVelocity *= 10f;
			rigidbody.useGravity = true;
			gravityScale = 0.1f;
			customTime = false;
		}
	}
}
