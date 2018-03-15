using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GaintZombieController : MonoBehaviour {

	public GameObject explosionPrefab;
	public GameObject firePrefab;
	public GameObject fireball_prefab;
	public float zombie_speed;
	public Transform zombie_cover;
	public Animator zombie_animator;
	public GameObject Shield;


	private GameObject fireHit;
	private GameObject fire;
	private Transform hero_transform;
	private Vector3 relative_distance;
	private float relative;
	private bool move;
	private Animator air_zombie_animator;
	private Vector3 third,top,bottom;
	private Rigidbody2D air_zombie_rigidbody;
	private Coroutine fire_coroutine,attack_coroutine;
	private float length;
	private bool hit;
	private bool collision;
	private int health = 2;
	private bool dead;
	public EdgeCollider2D edge_collider;

	void Start () {
		length = Random.Range (-2f, 7f);
		hero_transform = GameObject.FindGameObjectWithTag ("Hero").transform.GetChild (0).transform;
		move = false;
		air_zombie_animator = GetComponent<Animator> ();
		air_zombie_rigidbody = GetComponent<Rigidbody2D> ();
		fire_coroutine = null;
		hit = false;
		collision = false;
		dead = false;
	}

	void LateUpdate () {
		if (move == false) {
			zombie_cover.position = zombie_cover.position - Vector3.left * zombie_speed * Time.deltaTime;
			if (transform.position.x < length) {
				move = true;
				Destroy (Shield);
				attack_coroutine = StartCoroutine (attack());
				zombie_animator.Play ("idle", -1, 0);
			}
		}

		if (hit == true) {
			zombie_cover.position = zombie_cover.position - Vector3.left *1.5f*zombie_speed * Time.deltaTime;
			Vector3 fire_pos = new Vector3 (transform.position.x, transform.position.y - 0.3f, transform.position.z);
			if (fire) {
				fire.transform.position = fire_pos;
			}

		}

	}





	void OnCollisionEnter2D(Collision2D other)
	{
		ContactPoint2D contact = other.contacts[0];

		Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
		Vector3 pos = contact.point;

		GameObject other_object = other.gameObject;
		if (other_object.CompareTag ("AirWeaponBottle") && move == true && collision == false) {
			health--;
			fireHit  = Instantiate(explosionPrefab, pos, rot);
			other.gameObject.GetComponent<BottleController> ().DestroySelf ();
			StartCoroutine(destroyAnimation());
			if (health == 0) {
				dead = true;
				edge_collider.enabled = false;
				StopCoroutine (attack_coroutine);
				GameManager.instance.currentZombieCount--;
				GameManager.instance.increaseScore ();
				collision = true;
				StartCoroutine (respawn ());
			} 
		}

	}

	IEnumerator respawn(){
		yield return new WaitForSeconds (1f);
		hit = true;
		yield return new WaitForSeconds (1f);
		if (GameManager.instance.Play && SceneManager.GetActiveScene().name == "Level1")
			GameManager.instance.spawn ();
		if (GameManager.instance.Play && SceneManager.GetActiveScene ().name == "Level2" 
			&& GameManager.instance.currentZombieCount == 0)
			GameManager.instance.spawnLevel2 ();
		if (GameManager.instance.Play && SceneManager.GetActiveScene ().name == "Level3" 
			&& GameManager.instance.currentZombieCount == 0)
			GameManager.instance.spawnLevel3 ();
		yield return new WaitForSeconds (2f);
		Destroy (zombie_cover.gameObject);
		Destroy (fire);
	}

	IEnumerator destroyAnimation(){
		yield return new WaitForSeconds (0.2f);
		if (health == 0) {
			Vector3 fire_pos = new Vector3 (transform.position.x, transform.position.y - 0.3f, transform.position.z);
			zombie_animator.Play ("none", -1, 0);
			fire = Instantiate (firePrefab, fire_pos, Quaternion.Euler (0, 0, 0));
		}
	}

	public void SelfDestroy(){
	}



	IEnumerator attack(){
		yield return new WaitForSeconds (GameManager.instance.zombieFireWaitTime);
		if (dead == false) {
			for (;;) {
				if(fire_coroutine != null)
					StopCoroutine (fire_coroutine);
				third = new Vector3 (transform.position.x, hero_transform.position.y, transform.position.z);
				top =  hero_transform.position - third;
				bottom = transform.position - hero_transform.position + new Vector3 (0, 4, 0); 
				air_zombie_animator.SetTrigger ("attack");
				StartCoroutine (triggerAnimation ());
				fire_coroutine =  StartCoroutine (fire_poision());
				yield return new WaitForSeconds (GameManager.instance.zombieFireRate);
			}
		}

	}

	IEnumerator fire_poision(){
		yield return new WaitForSeconds (0.4f);
		if (dead == false) {
			Instantiate (fireball_prefab, transform.position - new Vector3(0.3f,0.6f,0) ,Quaternion.Euler(0,0,-Mathf.Rad2Deg*(Mathf.Asin(top.magnitude/bottom.magnitude))));
			GameManager.instance.Hurt ();
		}

	}


	IEnumerator triggerAnimation(){
		air_zombie_animator.Play ("zombieAttack", -1, 0);
		yield return new WaitForSeconds (1f);
		air_zombie_animator.Play ("zombieDrone", -1, 0);
	}
}
