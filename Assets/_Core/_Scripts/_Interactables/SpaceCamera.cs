using UnityEngine;
using System.Collections;

public class SpaceCamera : Interactable
{
	public float disableTime = 4.0f;
	public float alertTime = 6.0f;
	public LayerMask cameraAlertMask;
	
	float disableElapsed = 0.0f;
	
	public override bool Interact(Actor a) {
		if (base.Interact(a)) { //If we successfully interacted, do additional stuff
			return true;	
		}
		return false;
	}
	
	public override string GetActionName() {
		return "Disable\nCamera";
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (interactionEnabled) {
			//Be a camera
			if (Physics.CheckSphere(transform.position, interactBounds.size.x, cameraAlertMask)) {
				//Turn that camera red and start the detect timer?
				//Debug.Log ("Saw Someone!!!");
			}
			
			//Handle disable
			if (_currentActors.Count > 0) {
				disableElapsed += Time.deltaTime;	
				float per = Mathf.Floor(disableElapsed/disableTime * 100.0f);
				tagLabel.SetText(per.ToString() + "%");
			}
			else {
				disableElapsed = 0.0f;	
			}
		}
		
		if (disableElapsed >= disableTime) {
			SetInteractionEnabled(false);	
			tagLabel.SetText("Disabled");
		}
	}
}

