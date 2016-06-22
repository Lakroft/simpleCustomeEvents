/* Работает в паре с SysEvSender. Смотри описание там
*/
using UnityEngine;
using System.Collections;

public class SysEvents : MonoBehaviour {
	public delegate void Click();
	public event Click onClick;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.W)) {
			onClick ();
		}
	}


}
