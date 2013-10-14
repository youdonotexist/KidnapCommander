using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour {
	public List<GameObject> guardPrefabs;
	public int maxGuards = 3;
	// Use this for initialization
	void Start () {
		foreach (Transform child in transform) {
			if (Random.Range(0F, 1F) > 0.5F) {
				Instantiate(guardPrefabs[Random.Range(0, guardPrefabs.Count)], child.transform.position, Quaternion.identity);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnDrawGizmos() {
		foreach (Transform child in transform) {
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(child.transform.position, 20);
		}
	}
	
}
