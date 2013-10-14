using UnityEngine;
using System.Collections;

public class Death : MonoBehaviour
{
	public float speed = 100.0f;
	public AudioClip deathSFX;
	exSprite _sprite;
	
	float fadeTime = 1.0f;
	float fadeElapse = 0.0f;
	
	// Update is called once per frame
	void Start() {
		AudioManager.Instance.PlayOneShot(deathSFX);	
		_sprite = GetComponent<exSprite>();
	}
	
	void Update ()
	{
		Vector3 pos = transform.position;
		pos.y += speed * Time.deltaTime;
		transform.position = pos;
		
		Color c = _sprite.color;
		c.a = 1.0f - (fadeElapse/fadeTime);
		_sprite.color = c;
		
		fadeElapse += Time.deltaTime;
		
		if (!renderer.isVisible) {
			Destroy(this.gameObject);	
		}
	}
	
}

