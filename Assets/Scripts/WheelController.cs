using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour {

	// Public

	public int speed;

	public int brake;

	public int max_velocity;

	public Transform bottle_prefab;

	public Transform zombie_prefab;

	public Transform bottle;

	public Rigidbody2D bottle_rigidbody;

	public GameObject front_wheel;

	public GameObject back_legs;

	public GameObject front_legs;

	public GameObject upper_body;

	// Private

	private Rigidbody2D wheel_rigidbody;

	private Rigidbody2D wheel2_rigidbody;

	private Animator front_legs_animator;

	private Animator back_legs_animator;

	private Animator upper_body_animator;

	private Vector2 start, end, direction;

	private float screen_width;

	// Use this for initialization
	void Start () {
		
		wheel_rigidbody = GetComponent<Rigidbody2D> ();
		front_legs_animator = front_legs.GetComponent<Animator> ();
		back_legs_animator = back_legs.GetComponent<Animator> ();
		upper_body_animator = upper_body.GetComponent<Animator> ();
		wheel2_rigidbody = front_wheel.GetComponent<Rigidbody2D> ();
		start = new Vector2 (0, 0);
		screen_width = Screen.width;
	}

	// Update is called once per frame
	void Update () {
		upper_body_animator.SetInteger ("mul_attack", 0);

		#if SHOW_DEBUG_MESSAGES 

//		Debug.Log("hi");

		#endif
			

//		if (Input.GetKey(KeyCode.RightArrow)) {
			if (wheel_rigidbody.velocity.x < max_velocity)
				wheel_rigidbody.AddTorque (-speed);
			front_legs_animator.SetInteger ("run", 1);
			back_legs_animator.SetInteger ("run", 1);
//		}

		front_legs_animator.SetFloat ("speed", wheel_rigidbody.velocity.x / max_velocity);
		back_legs_animator.SetFloat ("speed", wheel_rigidbody.velocity.x / max_velocity);

		if (Input.GetKeyDown (KeyCode.X)) {
			Instantiate(zombie_prefab, new Vector3(Camera.main.transform.position.x+10f,-2.04f,0),Quaternion.identity);
		}

//		if (Input.GetKey(KeyCode.Space)) {
//			front_legs_animator.SetInteger ("run", 0);
//			back_legs_animator.SetInteger ("run", 0);
//			wheel_rigidbody.rotation = 0;
//			wheel2_rigidbody.rotation = 0;
//			if (wheel_rigidbody.velocity.x > 0) {
//				wheel_rigidbody.AddForce( new Vector2(-brake,0));
//			}
//		}

		if(Input.GetKeyDown(KeyCode.Z)){
			upper_body_animator.SetInteger ("attack", 0);
			upper_body_animator.SetInteger ("attack", 1);
		}

		if (Input.GetKeyUp (KeyCode.Z)) {
			upper_body_animator.SetInteger ("attack", 0);
		}

		if (Input.GetKeyDown (KeyCode.C)) {
			upper_body_animator.SetInteger ("bottle_attack", 0);
			upper_body_animator.SetInteger ("bottle_attack", 1);
		}

		if (Input.GetKeyUp (KeyCode.C)) {
			upper_body_animator.SetInteger ("bottle_attack", 0);
		}



			
		if (wheel_rigidbody.velocity.x <= 0) {
			wheel_rigidbody.velocity = new Vector2 (0, 0);
		}

		if (Input.GetKeyDown (KeyCode.C)) {
			bottle = Instantiate(bottle_prefab, new Vector3(upper_body.transform.position.x,upper_body.transform.position.y+0.5f,0),Quaternion.identity);
			bottle_rigidbody = bottle.GetComponent<Rigidbody2D> ();
			bottle_rigidbody.AddForce (new Vector2 (500, 300));
		}

		if (Input.touchCount > 0) {

			if (Input.GetTouch (0).position.x > screen_width * 0.75 && Input.GetTouch (0).phase == TouchPhase.Began) {
//				upper_body_animator.SetInteger ("attack", 0);
//				upper_body_animator.SetInteger ("attack", 1);
			} else {
				if (Input.GetTouch (0).phase == TouchPhase.Began && Input.GetTouch (0).position.x < screen_width*0.75) {
					Debug.Log (Input.GetTouch (0).position);
					start = Input.GetTouch (0).position;
				}
				if (Input.GetTouch (0).phase == TouchPhase.Ended && Input.GetTouch (0).position.x < screen_width* 0.75) {
					Debug.Log (Input.GetTouch (0).position);
					end = Input.GetTouch (0).position;
					direction = end - start;
					bottle = Instantiate (bottle_prefab, new Vector3 (upper_body.transform.position.x, upper_body.transform.position.y + 0.5f, 0), Quaternion.identity);
					bottle_rigidbody = bottle.GetComponent<Rigidbody2D> ();
					bottle_rigidbody.AddForce (direction.normalized * direction.magnitude);
					Debug.Log (direction);
					bottle_rigidbody.velocity = wheel_rigidbody.velocity;
					upper_body_animator.SetInteger ("bottle_attack", 0);
					upper_body_animator.SetInteger ("bottle_attack", 1);
				}
			}


//			start = Input.GetTouch (0).position;



		}

//		if (Input.touchCount == 0) {
//			upper_body_animator.SetInteger ("attack", 0);
//			upper_body_animator.SetInteger ("bottle_attack", 0);
//		}
	}
}