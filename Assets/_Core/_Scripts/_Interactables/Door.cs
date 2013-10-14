using UnityEngine;
using System.Collections;

public class Door : Interactable {
	
	public GameObject left;
	public GameObject right;
	
	bool opening = false;
	float moveTime = 1.0f;
	float moveElapsed = 0.0f;
	
	// Use this for initialization
	void Start () {
		
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	IEnumerable OpenDoors() {
		while (moveElapsed < moveTime) {
			
			
			yield return null;
		}
	}
}
