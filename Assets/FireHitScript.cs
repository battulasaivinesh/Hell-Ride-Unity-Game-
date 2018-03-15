using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireHitScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (SelfDestory ());
	}

	IEnumerator SelfDestory(){
		yield return new WaitForSeconds (0.6f);
		Destroy (gameObject);
	}

}
