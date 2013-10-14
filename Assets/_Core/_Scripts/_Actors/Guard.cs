using UnityEngine;
using System.Collections;

public class Guard : Actor
{
	Weapon _weapon;
	public LayerMask patrolMask;
	
	Robber _currentTarget;
	
	public float fireWait = 1.0f;
	float fireElapse = 0.0f;
	
	public float fireDistance = 200.0f;
	
	void Start() {
		_weapon = GetComponent<Weapon>();	
	}	
	
	public override bool Interact(Actor a) {
		return base.Interact(a);	
	}
	
	public override void Update() {
		base.Update ();
			
		//Be a guard
		if (_currentTarget == null) {
			Collider[] hits = Physics.OverlapSphere(transform.position, fireDistance, patrolMask);
			foreach (Collider c in hits) {
				Robber rob = c.GetComponent<Robber>();
				if (rob != null) {
					_currentTarget = rob;
					break;
				}
			}
		}
		
		if (_currentTarget != null) {
			float dist = Vector3.Distance(_currentTarget.transform.position, transform.position);
			if (dist <= fireDistance) {
				Vector3 fwd = transform.right * -1.0f;
				Vector3 target = (_currentTarget.transform.position - transform.position).normalized; target.z = 0.0f;
				transform.up = -target;
				
				if (fireElapse > fireWait) {
					_weapon.Fire(transform.position, transform.up, transform);
					fireElapse = 0.0f;
				}
				else {
					fireElapse += Time.deltaTime;	
				}
			}
			else {
				fireElapse += Time.deltaTime;	
			}
		}
		//////////////////////
		
		//Handle Interaction State
		if (Health <= 0) {
			SetInteractionEnabled(false);
			//Also, show the dead animation
		}
	}
	
	public override bool IsRemoteInteractable() {
		return true;	
	}
	
	//void OnDrawGizmos() {
	//	Gizmos.DrawWireSphere(transform.position, interactBounds.size.y * 5.0f);
	//}
}

