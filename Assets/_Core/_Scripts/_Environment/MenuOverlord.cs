using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuOverlord : MonoBehaviour {
	public UIButton playButton;
	public GameObject sniperPrefab;
	public GameObject marinePrefab;
	public GameObject demoPrefab;
	public GameObject startingRobbers;
	public int maxNumPeople = 3;
	
	private int usedPeople = 0;
	private int marines = 0;
	private int snipers = 0;
	private int demos = 0;

	// Use this for initialization
	void Start () {
		playButton.isEnabled = false;
		playButton.defaultColor = Color.white;
	}
	
	void Awake () {
		StartingRobbers robberController = startingRobbers.GetComponentInChildren<StartingRobbers>();
		robberController.robberPrefabs = new GameObject[]{};
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public bool addPerson (string personType) {
		if (usedPeople < maxNumPeople) {
			usedPeople++;
			if (personType == "marine") {
				marines++;
			}
			if (personType == "sniper") {
				snipers++;
			}
			if (personType == "demo") {
				demos++;
			}
			if (usedPeople == maxNumPeople) {
				playButton.isEnabled = true;
			}
			return true;
		}
		return false;
	}
	
	public bool subtractPerson (string personType) {
		if (usedPeople > 0) {
			if (personType == "marine" && marines > 0) {
				marines--;
				usedPeople--;
				playButton.isEnabled = false;
				return true;
			}
			if (personType == "sniper" && snipers > 0) {
				snipers--;
				usedPeople--;
				playButton.isEnabled = false;
				return true;
			}
			if (personType == "demo" && demos > 0) {
				demos--;
				usedPeople--;
				playButton.isEnabled = false;
				return true;
			}
			return false;
		}
		return false;
	}
	
	public void Play () {
		StartingRobbers robberController = startingRobbers.GetComponentInChildren<StartingRobbers>();
		List<GameObject> robberFabs = new List<GameObject>();
		
		for (var i = 0; i < marines; i++) {
			robberFabs.Add(marinePrefab);
		}
		for (var i = 0; i < snipers; i++) {
			robberFabs.Add(sniperPrefab);
		}
		for (var i = 0; i < demos; i++) {
			robberFabs.Add(demoPrefab);
		}
		robberController.robberPrefabs = robberFabs.ToArray();
		Application.LoadLevel("Game");
	}
}
