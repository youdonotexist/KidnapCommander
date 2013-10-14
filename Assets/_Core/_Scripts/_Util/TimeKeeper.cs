using UnityEngine;
using System.Collections;

public class TimeKeeper : MonoBehaviour
{
	float deltaTime;
	float lastTime = Time.realtimeSinceStartup;
	
	static TimeKeeper _instance = null;
	
	public static TimeKeeper Instance() {
		if (_instance == null) {
			GameObject go = GameObject.Find("TimeKeeper");
			if (go == null) {
				go = new GameObject("TimeKeeper");
				_instance = go.AddComponent<TimeKeeper>();
				Instantiate(go);	
			}
			else {
				_instance = go.GetComponent<TimeKeeper>();	
			}
		}
		
		return _instance;
	}
	
	void Update () {
		deltaTime = Time.realtimeSinceStartup - lastTime;
		lastTime = Time.realtimeSinceStartup;
	}
	
	public float AnimationTime() {
		return deltaTime;		
	}
}

