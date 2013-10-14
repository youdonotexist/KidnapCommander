using UnityEngine;
using System.Collections;

public class Vault : Interactable {
	
	public float crackTime = 10.0f;
	float crackElapsed = 0.0f;
	exSprite _sprite;
	
	public override void Awake() {
		base.Awake();
		
		Vector3 pos = transform.position; pos.z -= 0.2f;
		tagLabel.transform.position = pos;
		
		_sprite = GetComponent<exSprite>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (interactionEnabled) {
			//Handle disable
			if (_currentActors.Count > 0) {
				crackElapsed += Time.deltaTime;	
			}
			else {
				crackElapsed = 0.0f;	
			}
			
			if (crackElapsed >= crackTime) { 
				
				SetInteractionEnabled(false);
				_sprite.SetSprite(_sprite.atlas, _sprite.atlas.GetIndexByName("throne-shielddown"), false);
				Application.LoadLevel ("Win");
			}
			else {
				tagLabel.SetText(Mathf.Floor( (crackElapsed/crackTime) * 100.0f ).ToString() + "%");
			}
		}
	}
	
	public override string GetActionName() {
		return interactionEnabled ? "Crack Vault" : "CRACKED!";
	}
}
