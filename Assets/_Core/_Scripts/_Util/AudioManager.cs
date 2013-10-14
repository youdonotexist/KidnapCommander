using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{

	private static AudioManager instance;
 
	public static AudioManager Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject go = GameObject.Find("AudioManager");
				if (go == null) {
					instance = new GameObject ("AudioManager").AddComponent<AudioManager> ();
				}
				else {
					instance = go.GetComponent<AudioManager>();	
				}
				
				instance.audioSource = instance.gameObject.AddComponent<AudioSource>();
				instance.musicSource = instance.gameObject.AddComponent<AudioSource>();
				instance.audioSource.loop = false;
			}
 
			return instance;
		}
	}
	
	public AudioSource audioSource = null;
	public AudioSource musicSource = null;
	
	public void PlayMusic(AudioClip music) {
		musicSource.volume = 0.2f;
		musicSource.clip = music;
		musicSource.Play();
	}
	
	public void PlayOneShot(AudioClip effect) {
		audioSource.PlayOneShot(effect);	
	}
}

