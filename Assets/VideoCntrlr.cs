using UnityEngine;
using System.Collections;

public class VideoCntrlr : MonoBehaviour {
	
	// Use this for initialization
	void Awake () {
		Renderer r = GetComponent<Renderer>();
		MovieTexture movie = (MovieTexture)r.material.mainTexture;
		movie.loop = true;
		movie.Play();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
