using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour {

	public float backgoundSize;
	public float parallaxSpeed;

	private Transform cameraTransform;
	private Transform[] layers;
	private float prevPos;
	private float viewZone = 10f;
	private int leftIndex;
	private int rightIndex;
	private float lastCameraX;

	private void Start()
	{
		cameraTransform = Camera.main.transform;
		lastCameraX = cameraTransform.position.x;
		layers = new Transform[transform.childCount];
		for (int i = 0; i < transform.childCount; i++) {
			layers [i] = transform.GetChild (i);
		}

		prevPos = layers [0].position.x;
		leftIndex = 0;
		rightIndex =  layers.Length - 1;
	}

	private void Update()
	{
		transform.position += Vector3.left  * parallaxSpeed * Time.deltaTime;

		if (prevPos - layers[leftIndex].position.x > backgoundSize) {
			
			layers [leftIndex].position  =  Vector3.right * (layers [rightIndex].position.x + backgoundSize);
			rightIndex = leftIndex;
			leftIndex++;
			if (leftIndex == layers.Length) {
				leftIndex = 0;
			}
			prevPos = layers [leftIndex].position.x;

		}
		
	}

	private void ScrollLeft()
	{
		int lastRight = rightIndex;
		layers [rightIndex].position = Vector3.right * (layers [leftIndex].position.x - backgoundSize);
		leftIndex = rightIndex;
		rightIndex--;
		if (rightIndex < 0) 
			rightIndex = layers.Length - 1;
	}

	private void ScrollRight()
	{
		int lastLeft = leftIndex;
		layers [leftIndex].position = Vector3.right * (layers [rightIndex].position.x + backgoundSize);
		rightIndex = leftIndex;
		leftIndex++;
		if (leftIndex == layers.Length)
			leftIndex = 0;
	}

	public void reset(){
		Debug.Log ("Resetting Positions");
		layers[0].position = new Vector3(-22.047f, 0f, 0f);
		layers[1].position = new Vector3(22.047f, 0f, 0f);
		layers[2].position = new Vector3(44.094f, 0f, 0f);
		prevPos = layers [0].position.x;
		leftIndex = 0;
		rightIndex =  layers.Length - 1;
	}
}	


