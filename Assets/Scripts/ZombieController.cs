using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour {

	// Enemy Info
//	private string name;
	public bool is_land;

	// Collider blocking zombie and hero
	public EdgeCollider2D zombie_blocker;

	// Zombie transform
	private Transform zombie_transform;
	//Zombie Animator
	private Animator zombie_animator;
	//Zombie running speed
	private int zombie_speed;
	//Hero transform
	private Transform hero_transform;
	//current Zombie state
	private bool attack_state;
	private bool dead_state;


	void Start () {
//		name = "Enemy 1";
		is_land = true;
		attack_state = false;
		dead_state = false;
		zombie_transform = GetComponent<Transform> ();
		zombie_speed = 1;
		zombie_animator = GetComponentInChildren<Animator> ();
		hero_transform = GameObject.FindGameObjectWithTag ("Hero").transform.GetChild (0).GetComponent<Transform> ();
	}

	void Update () {
		zombie_transform.Translate (new Vector3(-1.4f,0,0)*Time.deltaTime*zombie_speed);
		Vector3 relative_distance = hero_transform.position - transform.position;

		if (relative_distance.magnitude < 1.5 && attack_state == false) {
			attack ();
		}
			
	}

	void attack(){
		zombie_animator.SetBool ("attack", true);
		zombie_speed = 0;
		attack_state = true;
	}

	public void dead(){
		if (attack_state == true  && dead_state == false) {
			StartCoroutine (trigger_dead());
			dead_state = true;
		}
	}


	IEnumerator trigger_dead(){
		GameManager.instance.increaseScore ();
		zombie_animator.SetBool ("dead", true);
		dead_state = true;
		zombie_blocker.isTrigger = true;
		gameObject.tag = "ZombieDead";
		yield return new WaitForSeconds (2);
		Destroy (gameObject);
	}
}
