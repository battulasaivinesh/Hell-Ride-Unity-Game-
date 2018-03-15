using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleController : MonoBehaviour {

	public Transform fire_prefab;

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag ("Ground")) {
			DestroySelf();
		}
	}

	void Update(){
		transform.Rotate (0, 0, -15);
	}


	public void DestroySelf(){
		Destroy (gameObject);
	}

}


