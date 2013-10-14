using UnityEngine;
using System.Collections;

public class FloatingTag : MonoBehaviour
{
	
	exSpriteFont _spriteText;
	exSprite _sprite;
	// Use this for initialization
	void Start ()
	{
		_spriteText = GetComponentInChildren<exSpriteFont>();
		_sprite = GetComponentInChildren<exSprite>();
		
		_sprite.enabled = false;
		_spriteText.enabled = false;
	}
	
	public void SetText(string text) {
		if (_sprite != null) {
			_sprite.enabled = false;
		}
		
		if (_spriteText != null) {
			_spriteText.enabled = true;
			_spriteText.renderer.enabled = true;
			_spriteText.text = text;
		}
	}
	
	public void SetAlert(string sprite_name) {
		if (_spriteText != null) {
			_spriteText.enabled = false;
		}
		if (_sprite != null) {
			_sprite.enabled = true;
			_sprite.renderer.enabled = true;
			_sprite.SetSprite(_sprite.atlas, _sprite.atlas.GetIndexByName(sprite_name), false);
		}
	}
	
	public void SetAlertWithText(string sprite_name, string text) {
		if (_spriteText != null) {
			_spriteText.enabled = true;
			_spriteText.text = text;
			_spriteText.renderer.enabled = true;
		}
		
		if (_sprite != null) {
			_sprite.enabled = true;
			_sprite.renderer.enabled = true;
			_sprite.SetSprite(_sprite.atlas, _sprite.atlas.GetIndexByName(sprite_name), false);
		}
	}
}

