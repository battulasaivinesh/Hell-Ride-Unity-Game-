using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

	void Update () {
		if (transform.position.x < 0 && GameManager.instance.checkpointResume == false) {
			GetComponent<ScrollLeft> ().parallaxSpeed = 0;
			GameManager.instance.stop (false);
		}

		if (transform.position.x < -15 && GameManager.instance.checkpointResume == true) {
			GetComponent<ScrollLeft> ().parallaxSpeed = 0;
			GameManager.instance.slowSpeed ();
			GameManager.instance.checkpointResume = false;
			transform.position = new Vector3 (22.047f, transform.position.y, transform.position.z);
		}
	}

}
