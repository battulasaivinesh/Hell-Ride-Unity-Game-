using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour {

	public Camera camera2;
	private Vector3 v3Pos;
	// Use this for initialization
	void Start () {
		v3Pos = new Vector3(0.9f, 0.2f, 1f);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.position = (camera2.GetComponent<Camera> ().ViewportToWorldPoint (v3Pos));
	}






}
