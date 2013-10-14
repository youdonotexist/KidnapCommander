using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour
{
	bool destroyOnNextUpdate = false;
	
	void OnDrawGizmos() {
		Debug.DrawLine(transform.position, transform.position + (transform.up * 100.0f));	
	}
	
	void OnCollisionEnter(Collision c) {
		Actor a = c.transform.GetComponent<Actor>();
		if (a != null) {
			a.DamaageActor(1);	
		}
		
		destroyOnNextUpdate = true;
	}
	
	void Update() {
		if (destroyOnNextUpdate) {
			Destroy(gameObject);
		}
	}
}

