using UnityEngine;
using System.Collections;

public class JumpObstacle : MonoBehaviour
{
	public float slowDownModifier = 0.90f;
	
	void OnTriggerEnter(Collider c) {
		Actor a = c.GetComponent<Actor>();	
		if (a != null) {
			a.movingOverObstacle = true;
		}
	}
	
	void OnTriggerExit(Collider c) {
		Actor a = c.GetComponent<Actor>();	
		if (a != null) {
			a.movingOverObstacle = false;
		}
	}
}

