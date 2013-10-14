using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Actor : Interactable {
	List<InteractStep> _steps;
	public bool movingOverObstacle = false;
	
	public float baseMoveSpeed = 100.0F;
	public float hinderedMoveSpeed = 50.0F;
	
	protected exSpriteAnimation _anim;
	protected exSprite _sprite;
	
	private InteractStep _currentInteractStep;
	
	public float MaxHealth = 5;
	protected float Health = 5;
	
	public string animationPrefix = "";
	
	public GameObject deathAnimation;
	
	public override string GetActionName() {return "Kill Guard";}
	
	Color savedColor;
	
	void OnDrawGizmos() {
		Debug.DrawLine(transform.position, transform.position + (-transform.up * baseMoveSpeed), Color.red);
	}
	
	public void SetSteps(List<InteractStep> steps) {
		_steps = steps;	
		AdvanceStep();
	}
	
	public override void Awake() {
		base.Awake();
		Health = MaxHealth;
		_anim = GetComponent<exSpriteAnimation>();
		_sprite = GetComponent<exSprite>();
		savedColor = _sprite.color;
	}
	
	public virtual void Update() {
		if (Health <= 0) {
			Instantiate(deathAnimation, transform.position, deathAnimation.transform.rotation);
			Destroy (this.gameObject);
			return;
		}
		else {
			Color c = _sprite.color;
			c.r = savedColor.r * (Health/MaxHealth);
			c.g = savedColor.g * (Health/MaxHealth);
			c.b = savedColor.b * (Health/MaxHealth);
			_sprite.color = c;
		}
		
		if ( (_steps != null && _steps.Count != 0) || _currentInteractStep != null) {
			if (_currentInteractStep.GetStepType() == InteractStep.StepType.MOVE) {
				Vector3 end = (Vector3) _currentInteractStep.GetValue("to");
				float curMoveSpeed;
				
				curMoveSpeed = movingOverObstacle ? hinderedMoveSpeed : baseMoveSpeed;
				Vector3 forward = Vector3.Normalize(transform.position - end); forward.z = 0.0f;
				transform.up = forward;
				
				if (Vector3.Distance(transform.position, end) <= Vector3.Magnitude(-transform.up * (curMoveSpeed * Time.deltaTime * 2.0f))) {
					transform.position = end;
					AdvanceStep();
				}
				else {
					var curPosition = transform.position;
					transform.position = curPosition + (-transform.up * (curMoveSpeed * Time.deltaTime));
					
					//We're walking
					string walkAnim = animationPrefix + "Walk";
					if (!_anim.IsPlaying(walkAnim)) {
						_anim.Play(walkAnim);	
					}
				}
			}
			else if (_currentInteractStep.GetStepType() == InteractStep.StepType.INTERACT) {
				Interactable i = (Interactable) _currentInteractStep.GetValue("interactable");
				if (i.Interact(this) && !i.IsInteractionFinished()) {
					HandleInteractableStep(_currentInteractStep);
				}
				else {
					i.Uninteract(this);
					AdvanceStep();	
				}
			}
		}
		else {
			_anim.Play(animationPrefix + "Idle");	
		}
	}
	
	public virtual void HandleInteractableStep(InteractStep step) {}
	
	public void DamaageActor(int amount) {
		Health -= amount;	
	}
	
	public void AdvanceStep() {
		if (_steps != null && _steps.Count > 0) {
			_currentInteractStep = _steps[0];	
			_steps.RemoveAt(0);	
		}
		else {
			_currentInteractStep = null;	
		}
	}
	
	void OnDestroy() {
		if (_currentInteractStep != null && _currentInteractStep.GetStepType() == InteractStep.StepType.INTERACT) {
			Interactable i = (Interactable) _currentInteractStep.GetValue("interactable");
			i.Uninteract(this);
		}
	}
}
