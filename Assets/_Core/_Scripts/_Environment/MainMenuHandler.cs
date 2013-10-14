using UnityEngine;
using System.Collections;

public class MainMenuHandler : MonoBehaviour
{
	public MenuOverlord menu;
	public UILabel targetLabel;
	public string personType;
	public bool isPositive;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	void OnClick ()
	{
		if (isPositive) {
			if (menu.addPerson(personType)) {
				targetLabel.text = (int.Parse(targetLabel.text) + 1).ToString();
			}
		}
		else {
			if (menu.subtractPerson(personType)) {
				targetLabel.text = (int.Parse(targetLabel.text) - 1).ToString();
			}
		}
	}
}
