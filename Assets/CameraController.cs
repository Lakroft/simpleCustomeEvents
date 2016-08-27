using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public Transform targetPos;
	public Transform playerPos;
	private Transform transform;
	private float speed = 0;
	private float filterValue = 0.03f;
	private Vector3 moveToVector;
	//private float oldSpeed;
	// Use this for initialization
	void Start () {
		if(targetPos == null) this.enabled = false;
		transform = gameObject.transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(Vector3.Distance(transform.position, targetPos.position) > 0.1f)
		{
//			speed = lineFilter (speed, Vector3.Distance(transform.position, targetPos.position));
//			transform.position += (targetPos.position - transform.position).normalized * speed;
			transform.position = lineFilter(transform.position, targetPos.position, Vector3.Distance(transform.position, targetPos.position));
		}
		transform.LookAt(playerPos.position);
	}

	float lineFilter (float oldValue, float newValue)
	{
		return oldValue * (1 - filterValue) + newValue * filterValue;
	}

	Vector3 lineFilter (Vector3 oldValue, Vector3 newValue, float inersia)
	{
		return oldValue * (1 - filterValue * inersia) + newValue * filterValue * inersia;
	}
}
