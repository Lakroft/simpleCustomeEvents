using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float speed = 1;
	public float angularSpeed = 1;
	private CharacterController controller;
	private Transform transform;
	private Vector3 rotateVector = Vector3.zero;
	// Use this for initialization
	void Start () {
		controller = gameObject.GetComponent<CharacterController>();
		transform = gameObject.transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		controller.Move(transform.forward * Input.GetAxis("Vertical") * Time.deltaTime * speed);
		//transform.Rotate(transform.rotation.eulerAngles + rotateVector);
		transform.RotateAround(transform.position, transform.up, Input.GetAxis("Horizontal") * angularSpeed);
		//Input.GetAxis("Horizontal");
	}
}
