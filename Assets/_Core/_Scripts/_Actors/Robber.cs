using UnityEngine;
using System.Collections;

public class Robber : Actor
{
	Weapon _weapon;
	public LayerMask patrolMask;
	Guard _currentTarget;
	
	public float fireWait = 1.0f;
	float fireElapsed = 0.0f;
	
	public float fireDistance = 200.0f;
	
	public override void Awake() {
		base.Awake();	
		
		_weapon = GetComponent<Weapon>();	
	}
	
	public override void Update() {
		base.Update();	
	}
	
	public override void HandleInteractableStep(InteractStep step) {
		Interactable i = (Interactable) step.GetValue("interactable");
		if (i.IsRemoteInteractable()) { //For now, this means shooting people
			if (_weapon != null) {
				float dist = Vector3.Distance(i.transform.position, transform.position);
				Debug.Log(dist);
				if (dist <= fireDistance) {
					Vector3 direction = (i.transform.position - transform.position).normalized; direction.z = 0.0f;
					transform.up = -direction;
					if (fireElapsed >= fireWait) { //
						_anim.Play(animationPrefix + "Fire");
						_weapon.Fire(transform.position, transform.up, transform);
						fireElapsed = 0.0f;
					}
					else {
						fireElapsed += Time.deltaTime;	
					}
					
					tagLabel.SetText("");	
				}
				else {
					tagLabel.SetText("Enemy too far away");	
					AdvanceStep();
				}
			}
		}
		else {
			tagLabel.SetText("");		
		}
	}
}

