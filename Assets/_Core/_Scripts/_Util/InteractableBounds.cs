using UnityEngine;
using System.Collections;

public class InteractableBounds : MonoBehaviour
{
	Interactable _interactable;
	
	void Awake() {
		_interactable = transform.parent.GetComponent<Interactable>();	
	}
	
	public Interactable GetInteractable() {
		return _interactable;	
	}
}

