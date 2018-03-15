using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour {

	public GameObject explosionPrefab;
	private GameObject fireHit;
	public bool shieldHit;

	void Start(){
	
		shieldHit = false;
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		ContactPoint2D contact = other.contacts[0];

		Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
		Vector3 pos = contact.point;

		GameObject other_object = other.gameObject;
		if (other_object.CompareTag ("AirWeaponBottle")){
			fireHit  = Instantiate(explosionPrefab, pos, rot);
			other.gameObject.GetComponent<BottleController> ().DestroySelf ();
		}

	}

}
