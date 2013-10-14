using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Zone : Interactable
{
	private bool isSuppressed = false;
	
	public List<Hostage> hostages;
	public Vector3 tagLabelPosition;
	
	public GameObject[] hostagePrefabs;	
	
	public override string GetActionName ()
	{
		return "Intimidate";
	}
	
	public void Suppress ()
	{
		isSuppressed = true;
	}
	
	public bool getSuppressionState ()
	{
		return isSuppressed;
	}
	
	public override void Awake ()
	{
		base.Awake ();
		/*if (Random.Range (0F, 1F) > 0.5F) {
			Destroy (gameObject);
			for (var i = 0; i < hostages.Count; i++) {
				Destroy (hostages [i].gameObject);
				hostages.RemoveAt (i);
				i--;
			}
		} else {
			for (var i = 0; i < hostages.Count; i++) {
				var deleteChance = Random.Range (0F, 1F);
				if (deleteChance >= 0.5F) {
					Destroy (hostages [i].gameObject);
					hostages.RemoveAt (i);
					i--;
				}
			}
			tagLabel.transform.position = transform.position + tagLabelPosition;
			
			if (hostages.Count == 0) {
				Destroy(gameObject);
			}
			else {
				Bounds b = new Bounds();
				for (int i = 0; i < hostages.Count; i++) {
					Hostage h = hostages[i];
					if (i == 0) {
						b = new Bounds(h.transform.position, h.collider.bounds.size);	
					}
					else {
						b.Encapsulate(h.collider.bounds);	
					}
				}
				
				SphereCollider c = (SphereCollider) interactCollider;
				c.radius = b.size.x * 0.6f;
			}
		}*/
	}
	
	public void Spawn() {
		int hostageCount = Random.Range (2, 4);
		SphereCollider c = (SphereCollider) interactCollider;
		
		Vector3[] zones = new Vector3[] {
			new Vector3(0.8f, 0.8f, 0.0f),
			new Vector3(-0.8f, 0.8f, 0.0f),
			new Vector3(0.0f, -1.0f, 0.0f)
		};
		
		hostages = new List<Hostage>();
		
		for (int i = 0; i < hostageCount; i++) {
			Vector3 zone = zones[i];
			int randomPrefab = Random.Range(0, hostagePrefabs.Length);
			GameObject prefab = hostagePrefabs[randomPrefab];
			
			Vector2 randomPos = Random.insideUnitCircle;
			Vector3 rim = zone * c.radius;
			Vector3 pos = transform.position + rim;
			
			GameObject go = (GameObject) GameObject.Instantiate(prefab, pos, prefab.transform.rotation);
			hostages.Add (go.GetComponent<Hostage>());
			go.transform.parent = transform;
			go.transform.up = zone;
		}
		
		/*Bounds b = new Bounds();
		for (int i = 0; i < hostages.Count; i++) {
			Hostage h = hostages[i];
			if (i == 0) {
				b = new Bounds(h.transform.position, h.collider.bounds.size);
			}
			else {
				b.Encapsulate(h.collider.bounds);	
			}
		}
		
		c.radius = b.size.x *  0.3f;*/
	}
}
