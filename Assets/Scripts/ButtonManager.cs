using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour {

	public GameObject inGame;
	public GameObject main;
	public Slider slide;
	public GameObject MenuCity;
	public GameObject MenuBase;
	public GameObject Road;
	public GameObject Clouds;
	public GameObject City;


	private bool spawn;

	void Start(){
		spawn = false;
	}



	public void slow(){
		Road.GetComponent<ScrollingBackground> ().parallaxSpeed = 6;
		Clouds.GetComponent<ScrollingBackground> ().parallaxSpeed = 0.3f;
		City.GetComponent<ScrollingBackground> ().parallaxSpeed = 0.5f;
	}

	public void ride(){
		MenuCity.GetComponent<ScrollLeft> ().parallaxSpeed = 8;
		MenuBase.GetComponent<ScrollLeft> ().parallaxSpeed = 12;
		Road.GetComponent<ScrollingBackground> ().parallaxSpeed = 12;
		Clouds.GetComponent<ScrollingBackground> ().parallaxSpeed =6;
		City.GetComponent<ScrollingBackground> ().parallaxSpeed = 8;
//		main.SetActive (false);
		inGame.SetActive (true);
		GameManager.instance.SpawnHero ();
	}

	public void sleep(){
		Application.Quit ();
	}


}

