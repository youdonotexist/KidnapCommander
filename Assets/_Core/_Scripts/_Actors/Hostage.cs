using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hostage : Actor
{
	public bool willRun = false;
	public bool willCall = false;
	
	public float attemptWait = 4.0f;
	float attemptElapsed;
	
	bool isRunning = false;
	bool isCalling = false;
	
	Door[] _doors;
	Door _runningDoor;
	
	public float callDuration = 5.0f;
	float callElapsed = 0.0f;
	
	public float intimidateDuration = 5.0f;
	float intimidateElapsed = 0.0f;
	
	Zone _zone;
	
	public GameObject[] policeSpawn;
	public GameObject policePrefab;
	
	public override string GetActionName() {return "Intimidate";}
	
	public override void Awake() {
		base.Awake();
		GameObject go = GameObject.Find("_Level");
		_doors = go.GetComponentsInChildren<Door>();
		
		policeSpawn = GameObject.FindGameObjectsWithTag("PoliceSpawn");
	}
	
	void Start() {
		_zone = transform.parent.GetComponent<Zone>();	
	}
	
	public override bool Interact(Actor a) {
		return base.Interact(a);
		// If we're running or calling, stop	
		//Clear the current door
	}
	
	
	// Update is called once per frame
	public override void Update ()
	{
		base.Update();
		
		if (_zone.IsInteractionEnabled()) {
			if (_zone.CurrentActorCount() == 0) {
				intimidateElapsed = 0.0f;
				if (attemptElapsed >= attemptWait) {
					//Be a hostage
					if (willRun || willCall && !(isRunning || isCalling) && _currentActors.Count == 0) {
						float runChance = Random.Range(0.0f, 1.0f);
						float callChance = Random.Range(0.0f, 1.0f);
						
						bool run = false;
						bool call = callChance > 0.95f ? true : false;
						
						if (run) {
							foreach (Door d in _doors) {
								if (d.CurrentActorCount() == 0) {
									_runningDoor = d;
									break;
								}
							}
							
							if (_runningDoor) {
								InteractStep step = new InteractStep(InteractStep.StepType.MOVE);
								step.PutValue("from", transform.position);
								step.PutValue ("to", _runningDoor.transform.position);
								
								List<InteractStep> steps = new List<InteractStep>();
								steps.Add (step);
								
								this.SetSteps(steps);
								
								isRunning = true;
							}
						}
						else if (call) {
							isCalling = true;
						}
						else {
							//Debug.Log ("All Attempts Failed");	
						}
					}
					attemptElapsed = 0.0f;
				}
				else {
					attemptElapsed += Time.deltaTime;	
				}
				
				if (isCalling) {
					Call ();	
				}
			}
			else {
				if (intimidateElapsed >= intimidateDuration) {
					_zone.tagLabel.SetText("Scared");
					_zone.SetInteractionEnabled(false);	
				}
				else {
					intimidateElapsed += Time.deltaTime;
					_zone.tagLabel.SetText("Intimidating: " + Mathf.Floor( (intimidateElapsed/intimidateDuration) * 100.0f).ToString() + "%");
				}
			}
			
			//Check for being watched
		}
		else {
			_zone.tagLabel.SetText("");	
		}
		
		
	}
	
	void Run() {
		tagLabel.SetText("Escaping!");
	}
	
	void Call() {
		if (_zone.IsInteractionEnabled()) {
			if (_zone.CurrentActorCount() > 0) {
				if (!_anim.IsPlaying(animationPrefix + "Idle")) {
					_anim.Play(animationPrefix + "Idle");	
				}
				tagLabel.SetText("");
				isCalling = false;
				return;	
			}
			
			callElapsed += Time.deltaTime;
			_zone.tagLabel.SetAlertWithText("help.png", Mathf.Floor( (callElapsed/callDuration) * 100.0f).ToString() + "%" );
			
			if (!_anim.IsPlaying(animationPrefix + "Call")) {
				_anim.Play(animationPrefix + "Call");	
			}
			
			if (callElapsed >= callDuration) {
				GameObject spawn = policeSpawn[Random.Range(0, policeSpawn.Length)];
				Instantiate (policePrefab, spawn.transform.position, policePrefab.transform.rotation);
				_zone.SetInteractionEnabled(false);
			}
		}
	}
}

