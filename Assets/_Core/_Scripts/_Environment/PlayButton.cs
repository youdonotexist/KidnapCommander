using UnityEngine;
using System.Collections;

public class PlayButton : MonoBehaviour {
	public MenuOverlord menu;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnClick () {
		menu.Play();
	}
}
