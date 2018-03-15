using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class sample : MonoBehaviour {

	public float speed;

	public Animator hero_animator_legs;
	public Animator hero_animator_body_front;
	public Transform line_transform;
	public GameObject bottle_prefab;
	public Animator hero_animator_body_back;
	public float throw_speed;
	public GameObject anchorPosition;
	public GameObject TrajectoryPointPrefeb;
	private bool playAnimation;
	private Vector2 start, end, direction,touch_direction,unit_direction;
	private GameObject bottle;
	public bool AmmoCheck;

	private bool touch_started;
	private Rigidbody2D bottle_rigidbody;
	private bool slowCheck;

	private int numOfTrajectoryPoints = 5;
	private List<GameObject> trajectoryPoints;

	void Start(){
		trajectoryPoints = new List<GameObject>();
		slowCheck = false;

		for(int i=0;i<numOfTrajectoryPoints;i++)
		{
			GameObject dot= (GameObject) Instantiate(TrajectoryPointPrefeb);
			dot.GetComponent<Renderer>().enabled = false;
			trajectoryPoints.Insert(i,dot);
		}
		AmmoCheck = true;
		touch_started = false;

	}

	void disableProjectile(){
		for(int i=0;i<numOfTrajectoryPoints;i++)
		{
			trajectoryPoints [i].GetComponent<Renderer> ().enabled = false;
		}
	}

	public void selfDestroy(){
		Destroy (gameObject);
	}

	void Update () {
		transform.position = transform.position + new Vector3 (speed * Time.deltaTime, 0, 0);

		Vector3 camera_position = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, 0));
		speed = 0;
		hero_animator_legs.SetFloat ("speed", 1f);
		hero_animator_body_back.SetFloat ("speed", 1f);
		hero_animator_body_front.SetFloat ("speed", 1f);

//		if (GameManager.instance.Play == false) {
//			disableProjectile ();
//		}

		if (transform.position.x < camera_position.x + 3) {
			speed = 2;
			hero_animator_legs.SetFloat ("speed", 1.2f);
			hero_animator_body_back.SetFloat ("speed", 1.2f);
			hero_animator_body_front.SetFloat ("speed", 1.2f);
		} else if (GameManager.instance.CyclePlay == false) {
			hero_animator_legs.SetFloat ("speed", 0f);
			hero_animator_body_back.SetFloat ("speed", 0f);
			hero_animator_body_front.SetFloat ("speed", 0f);
			hero_animator_legs.Play ("HeroLegsIdle2", -1, 0);
			hero_animator_body_front.Play ("frontIdle", -1,0);
			hero_animator_body_back.Play ("idleBack", -1,0);
			disableProjectile ();
		}
			


		if (speed == 0 && slowCheck == false) {
			GameManager.instance.slowSpeed ();
			slowCheck = true;
		}
			
		if (Input.GetKeyDown (KeyCode.A)) {
			hero_animator_legs.Play ("HeroLegsAttack2", -1, 0);
			hero_animator_body_front.Play ("heroBodyAttack", -1,0);
			hero_animator_body_back.Play ("attackBack", -1,0);
		}

		if (Input.GetKeyUp (KeyCode.A)) {
			StartCoroutine(triggerAnimation());
		}


		if (Input.GetKeyDown (KeyCode.C)) {
			direction = new Vector3(40,40,0);
			//					upper_body_animator.SetBool ("touch", false);
			//					upper_body_animator.SetTrigger ("throw");

			StartCoroutine(triggerAnimation());
			if (direction.magnitude > 40) {
				bottle = Instantiate (bottle_prefab, new Vector3 (transform.GetChild(0).transform.position.x, transform.GetChild(0).transform.position.y + 0.5f, 0), Quaternion.Euler(0,0,Mathf.Rad2Deg*Mathf.Atan(direction.y/direction.x)));
				bottle_rigidbody = bottle.GetComponent<Rigidbody2D> ();
//				Debug.Log (direction.magnitude);
				bottle_rigidbody.AddForce (direction.normalized *190*throw_speed);
			}
		}


		if (Input.touchCount > 0 && GameManager.instance.Play == true && AmmoCheck) {
			
			if (Input.GetTouch (0).phase == TouchPhase.Began) {
				
				start = Input.GetTouch (0).position;
				playAnimation = true;
//				Debug.Log ("Touch Started");
				if(start.x < (2*Screen.width)/3 && start.y < (2*Screen.height)/2)
					touch_started = true;
			}

			if (Input.GetTouch (0).phase == TouchPhase.Moved && touch_started) {
//				Debug.Log ("Touch Moved");
				if (Input.GetTouch (0).position.x < start.x && Input.GetTouch (0).position.y < start.y) {
					touch_direction = Input.GetTouch (0).position - start;
					direction = start - Input.GetTouch (0).position;
					unit_direction = touch_direction / touch_direction.magnitude;
					Vector3 vel = direction.normalized * direction.magnitude * throw_speed;
					Vector3 bottle_position = new Vector3 (anchorPosition.transform.position.x, anchorPosition.transform.position.y, 0);
					line_transform.rotation = Quaternion.Euler (0, 0, Mathf.Rad2Deg * Mathf.Atan (unit_direction.y / unit_direction.x) - 30);
					if (playAnimation == true) {
						hero_animator_legs.Play ("HeroLegsAttack2", -1, 0);
						hero_animator_body_front.Play ("heroBodyAttack", -1, 0);
						hero_animator_body_back.Play ("attackBack", -1, 0);
						playAnimation = false;
					}
//					Debug.Log ("Drawing Trajectory");

					setTrajectoryPoints (bottle_position, vel);
				}
			}
			if (Input.GetTouch (0).phase == TouchPhase.Ended && touch_started) {
//				Debug.Log ("Touch Ended");
				line_transform.rotation = Quaternion.Euler(0,0,0);
				touch_started = false;
				end = Input.GetTouch (0).position;
				disableProjectile ();
				if (end.x < start.x) {
					direction = start-end;
//					upper_body_animator.SetBool ("touch", false);
//					upper_body_animator.SetTrigger ("throw");
					if (playAnimation == false) {
						StartCoroutine(triggerAnimation());
						if (direction.magnitude > 40) {
							Vector3 bottle_position = new Vector3 (anchorPosition.transform.position.x, anchorPosition.transform.position.y, 0);
							bottle = Instantiate (bottle_prefab, bottle_position, Quaternion.Euler(0,0,Mathf.Rad2Deg*Mathf.Atan(direction.y/direction.x)));
							bottle_rigidbody = bottle.GetComponent<Rigidbody2D> ();
//							Debug.Log (direction.magnitude);
							if (direction.magnitude < 190) {
								bottle_rigidbody.AddForce (direction.normalized * direction.magnitude * throw_speed);
							} else {
								bottle_rigidbody.AddForce (direction.normalized * 190  * throw_speed);
							}

							GameManager.instance.reloadSlider.value = 0;
							AmmoCheck = false;
							StartCoroutine (reloadAmmo ());
						}


					}
						
				}

			}

		}

	}

	IEnumerator reloadAmmo(){
		if (Loader.instance.currentLevel == 1) {
			GameManager.instance.reloadSliderAnim.SetFloat ("speed", 1);
			GameManager.instance.reloadSliderAnim.Play ("Increase", -1, -0);
			yield return new WaitForSeconds (1f);
			AmmoCheck = true;
		} else if (Loader.instance.currentLevel == 2) {
			GameManager.instance.reloadSliderAnim.SetFloat ("speed", 1.5f);
			GameManager.instance.reloadSliderAnim.Play ("Increase", -1, -0);
			yield return new WaitForSeconds (0.75f);
			AmmoCheck = true;
		} else {
			GameManager.instance.reloadSliderAnim.SetFloat ("speed", 2);
			GameManager.instance.reloadSliderAnim.Play ("Increase", -1, -0);
			yield return new WaitForSeconds (0.5f);
			AmmoCheck = true;
		}

	}
		
	void setTrajectoryPoints(Vector3 pStartPosition , Vector3 pVelocity )
	{
//		Debug.Log ("Drawing Trajectory Inside");

		float velocity = Mathf.Sqrt((pVelocity.x * pVelocity.x) + (pVelocity.y * pVelocity.y));
		float angle = Mathf.Rad2Deg*(Mathf.Atan2(pVelocity.y , pVelocity.x));
		float fTime = 0;

		fTime += 0.1f;
		for (int i = 0 ; i < numOfTrajectoryPoints ; i++)
		{
			float dx = velocity * fTime * Mathf.Cos(angle * Mathf.Deg2Rad)/230;
			float dy = velocity * fTime * Mathf.Sin (angle * Mathf.Deg2Rad)/230;
			Vector3 pos = new Vector3(pStartPosition.x + dx , pStartPosition.y + dy ,2);
			trajectoryPoints[i].transform.position = pos;
			trajectoryPoints[i].GetComponent<Renderer>().enabled = true;
			fTime += 0.1f;
		}
	}

	public void stopAnimation(){
		hero_animator_legs.Play ("HeroLegsIdle2", -1, 0);
		hero_animator_body_front.Play ("frontIdle", -1,0);
		hero_animator_body_back.Play ("idleBack", -1,0);
		disableProjectile ();
	}



	IEnumerator triggerAnimation(){
		hero_animator_body_front.Play("heroBodyAttackThrow",-1,0);
		yield return new WaitForSeconds (0.25f);
		hero_animator_legs.Play ("HeroLegsIdle2", -1, 0);
		hero_animator_body_front.Play ("frontIdle", -1,0);
		hero_animator_body_back.Play ("idleBack", -1,0);
	}
}
