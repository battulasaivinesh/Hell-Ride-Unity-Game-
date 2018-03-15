using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadController : MonoBehaviour {

	public GameObject explosionPrefab;
	public GameObject firePrefab;
	private GameObject fire;

	private Vector3 position;
	void OnCollisionEnter2D(Collision2D other)
	{
		ContactPoint2D contact = other.contacts[0];

		Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
		Vector3 position = contact.point;

		GameObject other_object = other.gameObject;
		if (other_object.CompareTag ("AirWeaponBottle")){
			Instantiate(explosionPrefab, position, rot);
			other.gameObject.GetComponent<BottleController> ().DestroySelf ();
		}
	}
		

}
