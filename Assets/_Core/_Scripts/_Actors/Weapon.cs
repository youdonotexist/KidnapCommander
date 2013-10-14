using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
	public GameObject projectilePrefab;
	public AudioClip fireEffect;
	
	public void Fire(Vector3 muzzlePoint, Vector3 direction, Transform owner) {
		if (projectilePrefab != null) {
			GameObject go = (GameObject) Instantiate(projectilePrefab, muzzlePoint, Quaternion.identity);
			go.transform.up = direction;
			go.rigidbody.velocity = direction * -1000.0f;
			Physics.IgnoreCollision(go.collider, owner.collider);
			AudioManager.Instance.PlayOneShot(fireEffect);
		}
	}
}

