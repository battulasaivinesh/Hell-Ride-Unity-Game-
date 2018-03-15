using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireballController : MonoBehaviour {

	private Transform hero_transform;
	private Rigidbody2D fireball_rigidbody;
	private Vector3 direction;
	// Use this for initialization
	void Start () {
		hero_transform = GameObject.FindGameObjectWithTag ("Hero").transform.GetChild (0).transform;
		fireball_rigidbody = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		direction = hero_transform.position - transform.position;
		fireball_rigidbody.velocity = 15 * (direction / direction.magnitude);

		if (direction.magnitude < 0.2) {
//			GameManager.instance.Hurt ();
			Destroy (gameObject);
		}
	}
}
