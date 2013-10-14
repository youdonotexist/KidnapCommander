using UnityEngine;
using System.Collections;

public class Protection : Interactable
{

	public override string GetActionName() {return "Cover";}
	public override bool IsInteractionFinished() { return true; }
}

