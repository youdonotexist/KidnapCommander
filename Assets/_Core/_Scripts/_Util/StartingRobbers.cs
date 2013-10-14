using UnityEngine;
using System.Collections;

public class StartingRobbers : MonoBehaviour
{
	
	public GameObject[] robberPrefabs;
	public GameObject[] defaultPrefabs;
	
	// Use this for initialization
	void Start ()
	{
		DontDestroyOnLoad(this.gameObject);
	}
	
	public GameObject[] GetPrefabs() {
		if (robberPrefabs == null || robberPrefabs.Length == 0) {
			return defaultPrefabs;
			
		}
		
		return robberPrefabs;
	}
}

