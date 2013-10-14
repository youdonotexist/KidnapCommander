using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InteractStep
{
	public enum StepType {
		INTERACT,
		MOVE
	}
	
	protected StepType stepType;
	public bool rangedStep = false;
	
	Dictionary<string, object> data = new Dictionary<string, object>();
	
	private InteractStep() {}
	public InteractStep(StepType type) {
		stepType = type;	
	}
	
	public void PutValue(string key, object val) {
		if (val != null) {
			data[key] = val;	
		}
	}
	
	public object GetValue(string key) {
		return data[key];
	}
	
	public StepType GetStepType() {
		return stepType;	
	}
}

