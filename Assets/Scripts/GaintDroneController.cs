using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaintDroneController : MonoBehaviour {

	public float speed;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		transform.position = transform.position + new Vector3 (speed * Time.deltaTime, 0, 0);
	}
}





