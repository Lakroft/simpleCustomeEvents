/* Работает с классом EventObserverSrc, если повесить на объект, он подпишется на событие move и будет сдвигаться 
 * при его получении.
*/
using UnityEngine;
using System.Collections;

public class Subscriber : MonoBehaviour {
	public EventObserverSrc observer;

	// Use this for initialization
	void Start () {
		EventObserverSrc.EventDelegate forAdd = Move;
		observer.Subscribe ("move", forAdd, gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			EventObserverSrc.EventDelegate forAdd = Move;
			observer.Unsubscribe ("move", forAdd);
		}
	}

	public void Move()
	{
		gameObject.transform.position += Vector3.right * 3;
		Debug.Log ("Move object " + gameObject.name);
	}
}
