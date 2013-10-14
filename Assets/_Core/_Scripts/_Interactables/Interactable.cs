using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Interactable : MonoBehaviour {
	
	public Bounds interactBounds;
	public exSprite interactSprite;
	public Collider interactCollider;
	
	public FloatingTag tagLabel;
	public FloatingTag tagLabelPrefab;
	
	Color normal = Color.white;
	Color selected = Color.green;
	Color hover = Color.blue;
	
	public int maxInteractors = 1;
	protected List<Actor> _currentActors = new List<Actor>();
	
	protected bool interactionEnabled = true;
	
	public bool IsInteractionEnabled() {return interactionEnabled;}
	
	public virtual void Awake() {
		InteractableBounds b = GetComponentInChildren<InteractableBounds>();
		
		if (b != null) {
			interactBounds = b.collider.bounds;
			interactSprite = b.GetComponent<exSprite>();
			interactCollider = b.collider;
		}
		
		if (tagLabelPrefab != null) {
			Vector3 pos = transform.position; pos.y += (interactBounds.size.y * 0.5f) + 20.0f;
			tagLabel = (FloatingTag) Instantiate(tagLabelPrefab, pos, tagLabelPrefab.transform.rotation);
			tagLabel.SetText("Test");
			tagLabel.transform.parent = transform;
			
		}
	}
	
	public virtual bool Interact(Actor a) {
		bool exists = DoesActorExist(a);
		
		if (interactionEnabled && exists) {
			return true;	
		}
		
		if (interactionEnabled && _currentActors.Count < maxInteractors) {
			if (!exists) {
				_currentActors.Add(a);
			}
			return true;
		}
		else {
			return false;	
		}
	}
	public virtual void Uninteract(Actor a) {
		_currentActors.Remove(a);
	}
	
	public virtual void SetInteractionEnabled(bool enabled) {
		interactionEnabled = enabled;
		_currentActors.Clear();
	}
	
	public virtual bool IsInteractionFinished() { return !interactionEnabled; }
	public virtual string GetActionName() {return "unnamed";}
	
	public void Bounds() {}
	
	
	public virtual void OnInteractableHoverOn() {
		if (tagLabel != null) {
			tagLabel.SetText(GetActionName());	
		}
		interactSprite.color = hover;
	}
	public virtual void OnInteractableSelected() {
		interactSprite.color = selected;
	}
	public virtual void OnInteractableHoverOff() {
		if (tagLabel != null) {
			tagLabel.SetText("");	
		}
		interactSprite.color = normal;
	}
			
	bool DoesActorExist(Actor a) {
		foreach (Actor actor in _currentActors) {
			if (actor == a) {
				return true;
				break;
			}
		}
			
		return false;
	}
	
	public virtual bool IsRemoteInteractable() {
		return false;	
	}
	
	public virtual bool IsTerminatingInteractable() {
		return false;
	}
	
	public int CurrentActorCount() {
		return _currentActors.Count;	
	}
}
