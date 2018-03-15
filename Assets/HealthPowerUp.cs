using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPowerUp : MonoBehaviour {

	public GameObject parent;

	void Update(){
		if (transform.position.x < -25) {
			GameManager.instance.booster = false;                                                                                                                                                                                                         
			Destroy (parent);
		}
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		ContactPoint2D contact = other.contacts[0];

		Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
		Vector3 position = contact.point;

		GameObject other_object = other.gameObject;
		if (other_object.CompareTag ("AirWeaponBottle")){
			GameManager.instance.boostHealth ();
			Debug.Log ("Destroying Booster");
			other.gameObject.GetComponent<BottleController> ().DestroySelf ();
			GameManager.instance.booster = false;
			Destroy (parent);
		}
	}
}
