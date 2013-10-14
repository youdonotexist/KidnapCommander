using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;

public class Controls : MonoBehaviour {
	
	public AudioClip _battleMusic;
	
	public Transform[] spawnPoints;
	UILabel timeLabel;
	
	// Use this for initialization
	void Start () {
		Time.timeScale = 0.0f;
		AudioManager.Instance.PlayMusic(_battleMusic);
		timeLabel = GameObject.Find("TimeLabel").GetComponent<UILabel>();
		
		SpawnRobbers();
		BuildZones();
		
		planElapsed = 0.0f;
	}
	
	List<GameObject> spawnedRobbers = new List<GameObject>();
	
	Vector3 currentStart = Vector3.zero;
	Vector3 currentEnd = Vector3.zero;
	
	VectorLine currentLine = null;
	
	public Texture2D beginCap;
	public Texture2D endCap;
	
	public Material arrowMaterial;
	public Material capMaterial;
	
	public LayerMask interactableMask;
	public LayerMask actorMask;
	
	public AudioClip pickPath;
	public AudioClip pickInteractable;
	public AudioClip pickEnd;
	
	public Actor _currentActor;
	
	int arrowIndex = 0;
	
	Interactable _currentInteractable = null;
	
	List<InteractStep> _currentSteps = new List<InteractStep>();
	
	float planTime = 10.0f;
	float planElapsed = 0.0f;
	
	public float actionTime = 10.0f;
	float actionElapsed = 0.0f;
	
	void OnDrawGizmos() {
		Vector3 end = Camera.main.ScreenToWorldPoint(Input.mousePosition); end.z = 0.0f;
		Vector3 begin = new Vector3(end.x, end.y, Camera.main.transform.position.z);
		Debug.DrawLine(begin, begin + ((end - begin).normalized * 1000.0f));
	}
	
	// Update is called once per frame
	void Update () {
		if (GameState.Instance.IsPlanning()) {
			if (planElapsed >= planTime) {
				//If you're in the middle of something, we're save it out, bro
				ForceActorSteps();
				
				//Go, get, get out of here
				StartNormal ();
			}
			else {
				StartPlanning();
				DrawSomething();	
			}
			
			planElapsed += TimeKeeper.Instance().AnimationTime();
			timeLabel.text = "Planning: " + (planTime - Mathf.Ceil(planElapsed)).ToString() + "s";
			
		}
		else if (GameState.Instance.IsActionPause()) {
			if (Input.GetKeyDown(KeyCode.Space)) { //Return to normal
				ForceActorSteps();
				StartNormal ();
			}
			else {
				if (actionElapsed >= actionTime) {
					StartNormal();	
				}
				else {
					DrawSomething();	
					actionElapsed += TimeKeeper.Instance().AnimationTime();
					timeLabel.text = "Planning: " + (actionTime - Mathf.Ceil(actionElapsed)).ToString() + "s";
				}
			}
		}
		else { //We're simulating
			if (Input.GetKeyDown(KeyCode.Space) && actionElapsed < actionTime) { //Go to action pause
				StartActionPause ();
			}
			
			timeLabel.text = "";
		}
		
		CheckForDeath();
	}
	
	void StartNormal() {
		Time.timeScale = 1.0f;
		GameState.Instance._gameState = GameState.GAMESTATE.NORMAL;
	}
	
	void StartActionPause() {
		Time.timeScale = 0.0f;
		GameState.Instance._gameState = GameState.GAMESTATE.ACTION_PAUSE;
	}
	
	void StartPlanning() {
		Time.timeScale = 0.0f;
		GameState.Instance._gameState = GameState.GAMESTATE.PLANNING;
	}
	
	void DrawSomething() {
		if (Input.GetMouseButtonDown(0)) {
			if (currentEnd != Vector3.zero) { //This isn't the first point
				
				if (_currentInteractable == null || (_currentInteractable != null && !_currentInteractable.IsRemoteInteractable())) {
					InteractStep step = new InteractStep(InteractStep.StepType.MOVE);
					step.PutValue("from", currentStart);
					step.PutValue("to", currentEnd);
					_currentSteps.Add(step);
				
					currentStart = currentEnd;	
				}
				
				if (_currentInteractable) {
					InteractStep iStep = new InteractStep(InteractStep.StepType.INTERACT);
					iStep.PutValue("interactable", _currentInteractable);
					_currentSteps.Add (iStep);
					_currentInteractable.OnInteractableSelected();
					_currentInteractable = null;
					
					AudioManager.Instance.PlayOneShot(pickInteractable);
				}
				else {
					AudioManager.Instance.PlayOneShot(pickPath);		
				}
				
				currentStart = currentEnd;
				currentLine = new VectorLine("Line", new Vector3[] {currentStart, currentStart}, arrowMaterial, 5.0f);
				currentLine.vectorObject.gameObject.tag = "lineItem";
			}
			else { //This is our first point, we should make sure they click on a Robber first
				Vector3 end = Camera.main.ScreenToWorldPoint(Input.mousePosition); end.z = 0.0f;
				Vector3 begin = new Vector3(end.x, end.y, Camera.main.transform.position.z);
				RaycastHit hit;
				if(Physics.Raycast(new Ray(begin, (end - begin).normalized), out hit, Mathf.Infinity, actorMask)) {
					Robber rob = hit.collider.GetComponent<Robber>();
					if (rob != null) {
						currentStart = end;
						currentStart.z = -1.0f;	
				
						currentLine = new VectorLine("Line", new Vector3[] {currentStart, currentStart}, arrowMaterial, 5.0f);	
						currentLine.continuousTexture = true;
						currentLine.vectorObject.gameObject.tag = "lineItem";
						
						_currentActor = rob;
					}
				}
			}
		}
		else if (Input.GetMouseButtonDown(1)) { //Terminate this path
			currentStart = Vector3.zero;
			currentEnd = Vector3.zero;
			Vectrosity.VectorLine.Destroy(ref currentLine);
			currentLine = null;
			
			//Shove the current list of segments into the Robber actor
			if (_currentActor != null) {
				_currentActor.SetSteps(_currentSteps);
			}
			_currentSteps = new List<InteractStep>();
			_currentActor = null;
			
			AudioManager.Instance.PlayOneShot(pickEnd);
			
			List<GameObject> lineItems = new List<GameObject>(GameObject.FindGameObjectsWithTag("lineItem"));
			for (var i = 0; i < lineItems.Count; i++) {
				Destroy(lineItems[0]);
				lineItems.RemoveAt(0);
				i--;
			}
		}
		else if (currentStart != Vector3.zero) {
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); mousePos.z = 0.0f;
			Ray world = new Ray(currentStart, (mousePos - currentStart).normalized);
			
			//Look to see if we're inside an interactable
			RaycastHit hit;
			if (Physics.Raycast(world, out hit, Vector3.Distance(mousePos, currentStart), interactableMask)) {
				currentEnd = hit.point;
				currentEnd.z = -1.0f;	
				InteractableBounds bounds = hit.collider.GetComponent<InteractableBounds>();
				Interactable newInteractable = bounds.GetInteractable();
				if (newInteractable != _currentInteractable && _currentInteractable != null) {
					_currentInteractable.OnInteractableHoverOff();	
				}
				
				if (newInteractable != null) {
					_currentInteractable = newInteractable;
					_currentInteractable.OnInteractableHoverOn();
				}
			}
			else {
				currentEnd = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				currentEnd.z = -1.0f;	
				
				if (_currentInteractable != null) {
					_currentInteractable.OnInteractableHoverOff();	
					_currentInteractable = null;
				}
			}
			
			Vector3[] pts = new Vector3[] {currentEnd, currentStart};
			currentLine.points3 = pts;
			currentLine.Draw3D();
		}	
	}
	
	void BuildZones() {
		GameObject zones = GameObject.Find("Zones");
		Zone[] zoneArray = zones.GetComponentsInChildren<Zone>();
		List<Zone> zoneList = new List<Zone>(zoneArray);
		int zoneCount = Random.Range(3, 5);
		for (int i = 0; i < zoneCount; i++) {
			int randomZone = Random.Range(0, zoneList.Count - 1);
			Zone z = zoneList[randomZone];
			z.Spawn();
			zoneList.Remove(z);
		}
		
		foreach (Zone z in zoneList) {
			Destroy (z.gameObject);		
		}
		
		
	}
	
	void ForceActorSteps() {
		if (_currentActor != null && _currentSteps.Count > 0) {
			_currentActor.SetSteps(_currentSteps);
		}
		_currentSteps = new List<InteractStep>();
		_currentActor = null;
		
		currentStart = Vector3.zero;
		currentEnd = Vector3.zero;
		Vectrosity.VectorLine.Destroy(ref currentLine);
		currentLine = null;
		
		List<GameObject> lineItems = new List<GameObject>(GameObject.FindGameObjectsWithTag("lineItem"));
		for (var i = 0; i < lineItems.Count; i++) {
			Destroy(lineItems[0]);
			lineItems.RemoveAt(0);
			i--;
		}
	}
	
	void SpawnRobbers() {
		GameObject go = GameObject.Find("StartingRobbers");
		StartingRobbers sr = go.GetComponent<StartingRobbers>();
		GameObject[] startprefabs = sr.GetPrefabs();
		for (int i = 0; i < startprefabs.Length; i++) {
			GameObject rob = (GameObject) Instantiate(startprefabs[i], spawnPoints[i].position, startprefabs[i].transform.rotation);	
			spawnedRobbers.Add (rob);
		}
	}
	
	void CheckForDeath() {
		bool allDead = true;
		foreach (GameObject r in spawnedRobbers) {
			if (r != null) {
				allDead = false;
				break;
			}
		}
		
		if (allDead) {
			Application.LoadLevel("Lose");	
		}
	}
}
