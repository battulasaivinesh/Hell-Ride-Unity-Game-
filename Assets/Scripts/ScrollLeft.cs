using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollLeft : MonoBehaviour {

	public float parallaxSpeed;

	
	// Update is called once per frame
	void Update () {
		transform.position += Vector3.left  * parallaxSpeed * Time.deltaTime;
		if(transform.gameObject.CompareTag("LevelComplete")){
			if(transform.position.x < 0){
				parallaxSpeed = 0;
				GameManager.instance.stop (true);
			}
		}
	}
}
