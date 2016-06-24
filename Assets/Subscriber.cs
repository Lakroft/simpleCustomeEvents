/* Работает с классом EventObserverSrc, если повесить на объект, он подпишется на событие move и будет сдвигаться 
 * при его получении.
*/
using UnityEngine;
using System.Collections;

public class Subscriber : MonoBehaviour {
	public EventObserver observer;

	// Use this for initialization
	void Start () {
		//observer.Subscribe ("move", Move, gameObject);
		EventObserver.Instance.Subscribe("move", Move, gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			//EventObserverSrc.EventDelegate forAdd = Move;
			//observer.Unsubscribe ("move", Move);
			EventObserver.Instance.Unsubscribe ("move", Move);
		}
	}

	public void Move()
	{
		gameObject.transform.position += Vector3.right * 3;
		//Debug.Log ("Move object " + gameObject.name);
	}
}
