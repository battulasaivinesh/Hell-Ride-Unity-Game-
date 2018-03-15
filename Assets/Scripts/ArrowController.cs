using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour {

	public Rigidbody2D arrow_rigidbody;
	public int speed;
	public int RotateSpeed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if(arrow_rigidbody.velocity != Vector2.zero)
		{
			float rot_z = Mathf.Atan2(arrow_rigidbody.velocity.y, arrow_rigidbody.velocity.x) * Mathf.Rad2Deg;
			transform.rotation = 
				transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f, 0f, rot_z),Time.deltaTime * RotateSpeed);
		}
	}

	public void DestroySelf(){
		Destroy (gameObject);
	}
}
