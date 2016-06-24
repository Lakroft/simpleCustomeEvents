/* Работает с классом EventObserverSrc, выбрасывает событие move при нажатии на кнопку W 
*/
using UnityEngine;
using System.Collections;

public class Sender : MonoBehaviour {
	public EventObserver observer;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.W)) {
			observer.ThrowEvent ("move");
		}
	}
}
