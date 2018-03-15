using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour {


	public int speed;
	public int max_velocity;
	public GameObject zombie_prefab;
	public GameObject bottle_prefab;
	public Transform line_transform;
	public float throw_speed;


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


		if (Input.touchCount > 0) {
			if (Input.GetTouch (0).phase == TouchPhase.Began) {
				start = Input.GetTouch (0).position;

			}

			if (Input.GetTouch (0).phase == TouchPhase.Moved) {
				if (Input.GetTouch(0).position.x < start.x && Input.GetTouch(0).position.y < start.y) {
					touch_direction = Input.GetTouch (0).position - start;
					unit_direction = touch_direction / touch_direction.magnitude;
					line_transform.rotation = Quaternion.Euler(0,0,Mathf.Rad2Deg*Mathf.Atan(unit_direction.y/unit_direction.x)-30);
					upper_body_animator.SetBool ("touch", true);
				}
			}
			if (Input.GetTouch (0).phase == TouchPhase.Ended) {
				line_transform.rotation = Quaternion.Euler(0,0,0);

				end = Input.GetTouch (0).position;

				if (end.x < start.x) {
					direction = start-end;
					upper_body_animator.SetBool ("touch", false);
					upper_body_animator.SetTrigger ("throw");
					if (direction.magnitude > 40) {
						bottle = Instantiate (bottle_prefab, new Vector3 (transform.GetChild(0).transform.position.x, transform.GetChild(0).transform.position.y + 0.5f, 0), Quaternion.Euler(0,0,Mathf.Rad2Deg*Mathf.Atan(direction.y/direction.x)));
						bottle_rigidbody = bottle.GetComponent<Rigidbody2D> ();
						Debug.Log (direction.magnitude);
						if (direction.magnitude < 230) 
							bottle_rigidbody.AddForce (direction.normalized *direction.magnitude*throw_speed);
						else
							bottle_rigidbody.AddForce (direction.normalized *230*throw_speed);
						bottle_rigidbody.velocity = wheel_rigidbody.velocity;
					}

				}

			}

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