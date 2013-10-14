using UnityEngine;
using System.Collections;

public class ButtonHandler : MonoBehaviour
{
	void OnClick() {
    	GameObject current = UICamera.currentTouch.current;
    	string buttonName = "";
    	if (current != null) {
        	buttonName = current.name;
		}
		
		if (buttonName == "StartOver") {
			Application.LoadLevel("Mainmenu");	
		}
	}
}

