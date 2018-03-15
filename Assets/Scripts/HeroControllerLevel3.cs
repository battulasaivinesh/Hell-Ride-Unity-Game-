using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroControllerLevel3 : MonoBehaviour {


	public int speed;
	public int max_velocity;
	public GameObject zombie_prefab;
	public GameObject bottle_prefab;
	public Transform line_transform;
	public float throw_speed;
	public float fire_rate = 0;
	public Transform fire_position;
	public LayerMask WhatToHit;
	public Transform bulletTrail;

	private float time_to_fire = 0;
	private Rigidbody2D wheel_rigidbody;
	private Animator front_legs_animator;
	private Animator back_legs_animator;
	private Animator upper_body_animator;
	private GameObject[] zombies;
	private Vector2 start, end, direction,touch_direction,unit_direction;
	private GameObject bottle;
	private Rigidbody2D bottle_rigidbody;
	private bool zombie_presence;


	void Start() {
		wheel_rigidbody = transform.GetChild (2).gameObject.GetComponent<Rigidbody2D>();
		front_legs_animator = transform.GetChild (0).GetChild (0).gameObject.GetComponent<Animator> ();
		back_legs_animator = transform.GetChild (0).GetChild (1).gameObject.GetComponent<Animator> ();
		upper_body_animator = transform.GetChild (0).GetChild (2).GetChild(0).gameObject.GetComponent<Animator> ();
		front_legs_animator.SetInteger ("run", 1);
		back_legs_animator.SetInteger ("run", 1);
	}

	void Update() {

		if (wheel_rigidbody.velocity.x < max_velocity && zombie_presence == false)
			wheel_rigidbody.AddTorque (-speed);

		front_legs_animator.SetFloat ("speed", wheel_rigidbody.velocity.x / max_velocity);
		back_legs_animator.SetFloat ("speed", wheel_rigidbody.velocity.x / max_velocity);

		if (Input.GetKeyDown (KeyCode.C)) {
			upper_body_animator.SetBool ("touch", true);
		}

		if (Input.GetKeyUp (KeyCode.C)) {
			upper_body_animator.SetBool ("touch", false);
			upper_body_animator.SetTrigger ("throw");
		}

		Vector2 fire_position_vector = new Vector2 (fire_position.position.x, fire_position.position.y);

		Vector2 mouse_position = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);

		if (Input.GetKeyDown(KeyCode.A)) {
			shoot (new Vector2(0,0));
		}




	}

	public void shoot_button(){

		Debug.Log ("CALLED");
		if (fire_rate == 0) {
			if (Input.touchCount > 0) {
				start = Input.GetTouch (0).position;
				shoot (start);
			}

		} else {
			if (Input.touchCount > 0) {
				if (Time.time > time_to_fire) {
					time_to_fire = Time.time + 1 / fire_rate;
					start = Input.GetTouch (0).position;
					shoot (start);
				}
			}
		}
	}


	void shoot(Vector2 touch_position){
		Vector2 firepoint = new Vector2 (fire_position.position.x, fire_position.position.y);
		Vector2 touch_world_position = new Vector2 (Camera.main.ScreenToWorldPoint (touch_position).x, Camera.main.ScreenToWorldPoint (touch_position).y);
//		Vector2 mouse_world_position = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);
		RaycastHit2D hit = Physics2D.Raycast(firepoint,firepoint - touch_world_position,100,WhatToHit);
		Vector2 direction = firepoint - touch_world_position;
		Instantiate (bulletTrail, firepoint, Quaternion.Euler(0,0,Mathf.Rad2Deg*Mathf.Atan(direction.y/direction.x)));
		if (hit.collider != null) {
			hit.collider.gameObject.GetComponent<AirZombieController> ().SelfDestroy ();
		}
	}
		
	IEnumerator checkForZombies(){
		Vector3 relative_distance;

		for (;;) {
			zombie_presence = false;
			zombies = GameObject.FindGameObjectsWithTag ("Zombie");
			foreach (GameObject zombie in zombies) {
				relative_distance = transform.GetChild(0).transform.position - zombie.transform.position;

				if (relative_distance.magnitude < 0.8) {
					zombie_presence = true;

					break;
				}
			}


			if (zombie_presence == true) {
				wheel_rigidbody.freezeRotation = true;	
			} else {
				wheel_rigidbody.freezeRotation = false;
			}

			yield return null;

		}
	}

	public void attack(){
		Vector3 relative_distance;
		ZombieController zombie_controller_script;
		zombies = GameObject.FindGameObjectsWithTag ("Zombie");
		upper_body_animator.SetTrigger ("attack");
		foreach (GameObject zombie in zombies) {
			relative_distance = transform.GetChild(0).transform.position - zombie.transform.position;
			if (relative_distance.magnitude < 0.8) {
				zombie_controller_script = zombie.GetComponent<ZombieController> ();
				zombie_controller_script.dead ();
			}
		}

	}

	public void spawn(){
		Instantiate(zombie_prefab, new Vector3(Camera.main.transform.position.x+10f,-2.04f,0),Quaternion.identity);
	}

}