using UnityEngine;
using System.Collections;

public class HelpText : MonoBehaviour {

	// Use this for initialization
//	void Start () {
//	
//	}
//	
//	// Update is called once per frame
//	void Update () {
//	
//	}

	void OnGUI()
	{
		GUI.Label (new Rect (10f, 0f, 500f, 100f), "Press W to move objects,\nPress Esc to delete some objects");
	}
}
