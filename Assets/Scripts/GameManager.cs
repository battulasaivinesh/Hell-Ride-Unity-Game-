using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic; 
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public GameObject air_zombie_prefab;
	public GameObject gaint_zombie_prefab;
	public GameObject inGame;
	public Animator inGameAnimator;
	public GameObject pauseCanvas;
	public GameObject MenuCity;
	public GameObject MenuBase;
	public Animator main_anim;
	public static GameManager instance = null;    //Static instance of GameManager which allows it to be accessed by any other script.
//	public Text score_display;
//	public Text player_health;
//	public Text final_score;
//	public GameObject game_over_panel;
	public Slider progress;
	public GameObject hero_prefab;
	public GameObject Road;
	public GameObject Tutorial1;
	public GameObject Clouds;
	public GameObject City;
	public float zombieFireWaitTime;
	public float zombieFireRate;
	public Slider HealthSlider;
	public GameObject Checkpoint;
	public GameObject CheckpointPanel;
	public bool checkpointResume;
	public GameObject LevelComplete;
	public GameObject LevelCompletePanel;
	public Animator LevelCompletePanelAnim;
	public GameObject LoadingPanel;
	public Animator LoadingPanelAnimator;
	public Slider loader_slide;
	public int currentZombieCount;
	public GameObject healthPanel;
	public bool booster;
	public Text LeftText;
	public Text RightText;
	private bool StopOnce; 
	private Coroutine destroyZombie;
	public Animator checkpointAnimator;
	public GameObject HealthBooster;
	public GameObject backStory;

	public bool CyclePlay{
		get{
			return cyclePlay;
		}
	}

	public bool Play{
		get{
			return play;
		}
	}

	public Slider reloadSlider;
	public Animator reloadSliderAnim;

	private bool cyclePlay;
	private bool play;
	private int player_score;
	private GameObject[] zombies;
	private GameObject zombie;
	private GameObject air_zombie;
	private int health;
	public Coroutine healthSpawn;
	private GameObject hero;
	private int HealthBoosterNumber;

	//Awake is always called before any Start functions
	void Awake()
	{
		Scene scene = SceneManager.GetActiveScene();
		//Check if instance already exists
		if (instance == null)

			//if not, set instance to this
			instance = this;

		//If instance already exists and it's not this:
		else if (instance != this)

			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy(gameObject);    

		//Sets this to not be destroyed when reloading scene
//		DontDestroyOnLoad(gameObject);
		InitGame();
	}

	//Initializes the game for each level.
	void InitGame()
	{
		health = 100;
		player_score = 0;
		progress.maxValue = 100;
		progress.minValue = 0;
		progress.value = 0;

		HealthSlider.maxValue = 100;
		HealthSlider.minValue = 0;
		HealthSlider.value = 100;
//		currentLevel = 1;
//		currentSubLevel = 1;  
		currentZombieCount = 0;
		play = false;
		booster = false;
		cyclePlay = true;
		CheckpointPanel.SetActive (false);
		checkpointResume = false;
		main_anim.Play ("GoBottom", -1, 0);
		Loader.instance.SaveFile ();
		if (Loader.instance.currentLevel == 3) {
			HealthBoosterNumber = 2;
		} else {
			HealthBoosterNumber = 1;
		}

		if (Loader.instance.first_time) {
			StartCoroutine (backStoryAnimation ());
		}

		setText ();
	}

	IEnumerator backStoryAnimation(){
		yield return new WaitForSeconds (1f);
		backStory.GetComponent<Text1Controller> ().startAnimation ();
	}



	public void setText(){
		if (Loader.instance.currentLevel == 1) {
			if (Loader.instance.currentSubLevel == 1) {
				LeftText.text = "1.1";
				RightText.text = "1.2";
			} else if (Loader.instance.currentSubLevel == 2) {
				LeftText.text = "1.2";
				RightText.text = "1.3";
			} else if (Loader.instance.currentSubLevel == 3) {
				LeftText.text = "1.3";
				RightText.text = "2.1";
			}
		} else if (Loader.instance.currentLevel == 2) {
			
			if (Loader.instance.currentSubLevel == 1) {
				LeftText.text = "2.1";
				RightText.text = "2.2";
			} else if (Loader.instance.currentSubLevel == 2) {
				LeftText.text = "2.2";
				RightText.text = "2.3";
			} else if (Loader.instance.currentSubLevel == 3) {
				LeftText.text = "2.3";
				RightText.text = "3.1";
			}
		} else if (Loader.instance.currentLevel == 3) {
			if (Loader.instance.currentSubLevel == 1) {
				LeftText.text = "3.1";
				RightText.text = "3.2";
			} else if (Loader.instance.currentSubLevel == 2) {
				LeftText.text = "3.2";
				RightText.text = "3.3";
			} else if (Loader.instance.currentSubLevel == 3) {
				LeftText.text = "3.3";
				RightText.text = "A";
			}
		}
	}

	IEnumerator spawnHeathBooster(){
		for (;;) {
			yield return new WaitForSeconds (2f);
//			Debug.Log ("IN Booster Coroutine");
			if (Loader.instance.currentLevel == 3) {
				if (progress.value > 30 && booster == false && HealthBoosterNumber > 1) {
//					Debug.Log ("Booster Ready!!!!!");
					float height = Random.Range (2f, 3.5f);
					Camera cam = Camera.main.GetComponent<Camera> ();
					Instantiate (HealthBooster, new Vector3 (Camera.main.transform.position.x + 15f, height, 0), Quaternion.identity);
					booster = true;
					HealthBoosterNumber--;
				} else if (progress.value > 60 && booster == false && HealthBoosterNumber > 0) {
//					Debug.Log ("Booster Ready!!!!!");
					float height = Random.Range (2f, 3.5f);
					Camera cam = Camera.main.GetComponent<Camera> ();
					Instantiate (HealthBooster, new Vector3 (Camera.main.transform.position.x + 15f, height, 0), Quaternion.identity);
					booster = true;
					HealthBoosterNumber--;
				}
			} else {
				if (progress.value > 50 && booster == false && HealthBoosterNumber > 0) {
//					Debug.Log ("Booster Ready!!!!!");
					float height = Random.Range (2f, 3.5f);
					Camera cam = Camera.main.GetComponent<Camera> ();
					Instantiate (HealthBooster, new Vector3 (Camera.main.transform.position.x + 15f, height, 0), Quaternion.identity);
					booster = true;
					HealthBoosterNumber--;
				}
			}
		}
	}

	public void boostHealth(){
		health = health + 50;
		HealthSlider.value = HealthSlider.value + 50;
	}

	public void slowSpeed(){
		Road.GetComponent<ScrollingBackground> ().parallaxSpeed = 6;
		Clouds.GetComponent<ScrollingBackground> ().parallaxSpeed = 0.3f;
		City.GetComponent<ScrollingBackground> ().parallaxSpeed = 0.5f;
		Debug.Log ("Start");
		healthSpawn = StartCoroutine (spawnHeathBooster());

		if (Loader.instance.first_time) {
			Tutorial1.SetActive (true);
			Debug.Log ("Running first time");
		}

		if (Loader.instance.currentLevel == 3) {
			HealthBoosterNumber = 2;
		} else {
			HealthBoosterNumber = 1;
		}

		Debug.Log ("Middle");


		play = true;
		inGame.SetActive (true);
		inGameAnimator.Play ("GoBottomInGame", -1, 0);
		if (SceneManager.GetActiveScene ().name == "Level1") {
			if (Loader.instance.first_time == false) {
				GameManager.instance.spawn ();
			}
			if (Loader.instance.currentSubLevel == 1) {
				zombieFireWaitTime = 3f;
				zombieFireRate = 4f;
			} else if (Loader.instance.currentSubLevel == 2) {
				zombieFireWaitTime = 2f;
				zombieFireRate = 3f;
			} else if (Loader.instance.currentSubLevel == 3) {
				zombieFireWaitTime = 1f;
				zombieFireRate = 2f;
			}
		} else if (SceneManager.GetActiveScene ().name == "Level2") {
			StartCoroutine (spawn2());
			if (Loader.instance.currentSubLevel == 1) {
				zombieFireWaitTime = 2f;
				zombieFireRate = 2f;
			} else if (Loader.instance.currentSubLevel == 2) {
				zombieFireWaitTime = 1.5f;
				zombieFireRate = 2f;
			} else if (Loader.instance.currentSubLevel == 3) {
				zombieFireWaitTime = 1f;
				zombieFireRate = 2f;
			}
		} else if (SceneManager.GetActiveScene ().name == "Level3") {
			spawnLevel3 ();
			if (Loader.instance.currentSubLevel == 1) {
				zombieFireWaitTime = 2f;
				zombieFireRate = 2f;
			} else if (Loader.instance.currentSubLevel == 2) {
				zombieFireWaitTime = 2f;
				zombieFireRate = 3f;
			} else if (Loader.instance.currentSubLevel == 3) {
				zombieFireWaitTime = 1.75f;
				zombieFireRate = 2f;
			}
		}
		Debug.Log ("End");
	}

	public void returnMenu(){
		hero.GetComponent<sample> ().selfDestroy ();
		Road.GetComponent<ScrollingBackground> ().parallaxSpeed = 0f;
		Road.GetComponent<ScrollingBackground> ().reset ();
		City.GetComponent<ScrollingBackground> ().parallaxSpeed = 0f;
		City.GetComponent<ScrollingBackground> ().reset ();
		MenuCity.GetComponent<ScrollLeft> ().parallaxSpeed = 0;
		MenuBase.GetComponent<ScrollLeft> ().parallaxSpeed = 0;
		MenuCity.transform.position = new Vector3 (0, 0, 0);
		MenuBase.transform.position = new Vector3 (0, 0, 0);
		inGameAnimator.Play ("GoUpInGame", -1, 0);
//		ride_anim.Play ("idle", -1, 0);
//		sleep_anim.Play ("idle", -1, 0);
		main_anim.Play ("GoBottom", -1, 0);

		health = 100;
		player_score = 0;
		progress.maxValue = 100;
		progress.minValue = 0;
		progress.value = 0;
		HealthSlider.maxValue = 100;
		HealthSlider.minValue = 0;
		HealthSlider.value = 100;
		currentZombieCount = 0;
		play = false;
		cyclePlay = true;
		healthPanel.SetActive (false);
		checkpointResume = false;

		destroyZombie = StartCoroutine (DestroyZombies ());

		resumeGame ();
	}

	IEnumerator DestroyZombies(){
		for (;;) {
			GameObject[] gameObjects;
			gameObjects = GameObject.FindGameObjectsWithTag ("ZombieDead");
			for (var i = 0; i < gameObjects.Length; i++) {
				Destroy (gameObjects [i]);
			}
			gameObjects = GameObject.FindGameObjectsWithTag ("Booster");
			for (var i = 0; i < gameObjects.Length; i++) {
				Destroy (gameObjects [i]);
			}
			yield return new WaitForSeconds (0.1f);
		}
	}
		

	public void spawn(){
		float height = Random.Range (-1.5f, 3.5f);
		Camera cam = Camera.main.GetComponent<Camera> ();
		air_zombie = Instantiate(air_zombie_prefab, new Vector3(Camera.main.transform.position.x+15f,height,0),Quaternion.identity);
	}

	public void pauseGame(){
		Time.timeScale = 0;
		pauseCanvas.SetActive (true);
	}

	public void resumeGame(){
		Time.timeScale = 1;
		pauseCanvas.SetActive (false);
	}

	public void removeFirst(){
		if (Loader.instance.first_time == true) {
			Loader.instance.first_time = false;
			slowSpeed ();
		}
			
	}

	void OnApplicationFocus(bool hasFocus)
	{
		if (!hasFocus && play == true)
			pauseGame();
		
	}

	void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus && play == true)
			pauseGame();
		
	} 

	IEnumerator spawn2(){
		float height = Random.Range (-1.5f, 3.5f);
		air_zombie = Instantiate(air_zombie_prefab, new Vector3(Camera.main.transform.position.x+15f,height,0),Quaternion.identity);
		currentZombieCount++;

		yield return new WaitForSeconds (1.5f);

		float height2;
		if(height < 2.5)
			height2 = Random.Range (2.5f, 3.5f);
		else 
			height2 = Random.Range (-1.5f, 2f);
		air_zombie = Instantiate(air_zombie_prefab, new Vector3(Camera.main.transform.position.x+15f,height2,0),Quaternion.identity);
		currentZombieCount++;
	}

	IEnumerator spawn3(){
		if (Loader.instance.currentSubLevel == 1) {
			float height = Random.Range (-1.5f, 3.5f);
			air_zombie = Instantiate (air_zombie_prefab, new Vector3 (Camera.main.transform.position.x + 15f, height, 0), Quaternion.identity);
			currentZombieCount++;

			yield return new WaitForSeconds (1.5f);

			float height2;
			if(height < 2.5)
				height2 = Random.Range (2.5f, 3.5f);
			else 
				height2 = Random.Range (-1.5f, 2f);
			air_zombie = Instantiate (gaint_zombie_prefab, new Vector3 (Camera.main.transform.position.x + 15f, height2, 0), Quaternion.identity);
			currentZombieCount++;
		} else if (Loader.instance.currentSubLevel == 2) {
			float height = Random.Range (-1.5f, 3.5f);
			air_zombie = Instantiate (gaint_zombie_prefab, new Vector3 (Camera.main.transform.position.x + 15f, height, 0), Quaternion.identity);
			currentZombieCount++;

			yield return new WaitForSeconds (1.5f);

			float height2;
			if(height < 2.5)
				height2 = Random.Range (2.5f, 3.5f);
			else 
				height2 = Random.Range (-1.5f, 2f);
			air_zombie = Instantiate (gaint_zombie_prefab, new Vector3 (Camera.main.transform.position.x + 15f, height2, 0), Quaternion.identity);
			currentZombieCount++;
		} else if (Loader.instance.currentSubLevel == 3) {
			float height = Random.Range (-1.5f, 3.5f);
			air_zombie = Instantiate (gaint_zombie_prefab, new Vector3 (Camera.main.transform.position.x + 15f, height, 0), Quaternion.identity);
			currentZombieCount++;

			yield return new WaitForSeconds (1.5f);

			float height2;
			if(height < 2.5)
				height2 = Random.Range (2.5f, 3.5f);
			else 
				height2 = Random.Range (-1.5f, 2f);
			air_zombie = Instantiate (gaint_zombie_prefab, new Vector3 (Camera.main.transform.position.x + 15f, height2, 0), Quaternion.identity);
			currentZombieCount++;
		}

	}

	public void spawnLevel2(){
		StartCoroutine (spawn2());
	}

	public void spawnLevel3(){
		StartCoroutine (spawn3());
	}

	public void ResumePlay(){
		Road.GetComponent<ScrollingBackground> ().parallaxSpeed = 6;
		Checkpoint.GetComponent<ScrollLeft> ().parallaxSpeed = 6;
		Clouds.GetComponent<ScrollingBackground> ().parallaxSpeed = 0.3f;
		City.GetComponent<ScrollingBackground> ().parallaxSpeed = 0.5f;
		CheckpointPanel.SetActive (true);
		checkpointAnimator.Play ("GoUpCheckpoint", -1, 0);
		cyclePlay = true;
		checkpointResume = true;
		HealthSlider.value = 100;
		health = 100;
	}

	public void SpawnHero(){
		main_anim.Play ("GoUp", -1, 0);
		Vector3 pos = Camera.main.ViewportToWorldPoint (new Vector3(0, 0, 0));
		if (destroyZombie != null)
			StopCoroutine (destroyZombie);

		hero = Instantiate (hero_prefab, new Vector3 (pos.x - 4,-3.83f,0),Quaternion.Euler(0,0,0));
	}

	public void stop(bool complete){
		if (StopOnce) {
			Debug.Log ("Game Stopped");
			Road.GetComponent<ScrollingBackground> ().parallaxSpeed = 0;
			Clouds.GetComponent<ScrollingBackground> ().parallaxSpeed = 0;
			City.GetComponent<ScrollingBackground> ().parallaxSpeed = 0;
			cyclePlay = false;
			if (complete) {
				LevelCompletePanel.SetActive (true);
				LevelCompletePanelAnim.Play ("MoveDownFinish", -1, 0);
			}
			else {
				Debug.Log("Playing Animation");
				CheckpointPanel.SetActive (true);
				checkpointAnimator.Play ("GoDownCheckpoint", -1, 0);
			}
//			Debug.Log ("After Hell");
			if (healthSpawn != null) {
				StopCoroutine (healthSpawn);
			}
			progress.value = 0;
			StopOnce = false;
		}


		cyclePlay = false;

	}

	public void resetGame(){
		Loader.instance.currentLevel = 1;
		Loader.instance.currentSubLevel = 1;
		Loader.instance.SaveFile ();
		LoadingPanel.SetActive (true);
		LoadingPanelAnimator.Play ("FadeIn", -1, -0);
		StartCoroutine(AsynchronousLoad("Level1"));
	}


	public void Hurt(){
		if (Loader.instance.currentLevel == 1) {
			health = health - 10;
			if (health <= 0) {
				healthPanel.SetActive (true);
				Time.timeScale = 0;
			}
			HealthSlider.value = HealthSlider.value - 10;
		} else if (Loader.instance.currentLevel == 2) {
			health = health - 8;
			if (health <= 0) {
				healthPanel.SetActive (true);
				Time.timeScale = 0;
			}
			HealthSlider.value = HealthSlider.value - 8;
		} else {
			health = health - 6;
			if (health <= 0) {
				healthPanel.SetActive (true);
				Time.timeScale = 0;
			}
			HealthSlider.value = HealthSlider.value - 6;
		}

	}

	public void increaseScore(){
		player_score++;
		StopOnce = true;
		if (SceneManager.GetActiveScene ().name == "Level1") {
			if (Loader.instance.currentSubLevel == 1) {
				progress.value += 10;
				if (progress.value == 100) {
					Checkpoint.GetComponent<ScrollLeft> ().parallaxSpeed = 6;
					play = false;
					hero.GetComponent<sample> ().stopAnimation ();
					Loader.instance.currentLevel = 1;
					Loader.instance.currentSubLevel = 2;
					Loader.instance.SaveFile ();

					inGameAnimator.Play ("GoUpInGame",-1,0);
				}
			} else if (Loader.instance.currentSubLevel == 2) {
				progress.value += 100/15;
				if (progress.value == 100) {
					Checkpoint.GetComponent<ScrollLeft> ().parallaxSpeed = 6;
					play = false;
					hero.GetComponent<sample> ().stopAnimation ();
					Loader.instance.currentSubLevel = 3;
					Loader.instance.currentLevel = 1;
					Loader.instance.SaveFile ();
					inGameAnimator.Play ("GoUpInGame",-1,0);
				}
			} else if (Loader.instance.currentSubLevel == 3) {
				progress.value += 5;

				if (progress.value == 100) {
					LevelComplete.GetComponent<ScrollLeft> ().parallaxSpeed = 6;
					play = false;
					Loader.instance.currentLevel = 2;
					Loader.instance.currentSubLevel = 1;
					hero.GetComponent<sample> ().stopAnimation ();
					Loader.instance.SaveFile ();
					inGameAnimator.Play ("GoUpInGame",-1,0);
				}
			}
		} else if (SceneManager.GetActiveScene ().name == "Level2") {
			if (Loader.instance.currentSubLevel == 1) {
				if (currentZombieCount == 0)
					progress.value += 10;
				if (progress.value == 100) {
					Checkpoint.GetComponent<ScrollLeft> ().parallaxSpeed = 6;
					play = false;
					Debug.Log ("Checkpoint Reached");
					hero.GetComponent<sample> ().stopAnimation ();
					Loader.instance.currentLevel = 2;
					Loader.instance.currentSubLevel = 2;
					Loader.instance.SaveFile ();
					inGameAnimator.Play ("GoUpInGame",-1,0);
				}
			} else if (Loader.instance.currentSubLevel == 2) {
				if (currentZombieCount == 0)
					progress.value += 100/15;
				if (progress.value == 100) {
					Checkpoint.GetComponent<ScrollLeft> ().parallaxSpeed = 6;
					play = false;
					hero.GetComponent<sample> ().stopAnimation ();
					Loader.instance.currentLevel = 2;
					Loader.instance.currentSubLevel = 3;
					Loader.instance.SaveFile ();
					inGameAnimator.Play ("GoUpInGame",-1,0);
				}
			} else if (Loader.instance.currentSubLevel == 3) {
				if (currentZombieCount == 0)
					progress.value += 5;
				if (progress.value == 100) {
					LevelComplete.GetComponent<ScrollLeft> ().parallaxSpeed = 6;
					play = false;
					Loader.instance.currentLevel = 3;
					Loader.instance.currentSubLevel = 1;
					hero.GetComponent<sample> ().stopAnimation ();
					Loader.instance.SaveFile ();
					inGameAnimator.Play ("GoUpInGame",-1,0);
				}
			}
		} else if (SceneManager.GetActiveScene ().name == "Level3") {
			if (Loader.instance.currentSubLevel == 1) {
				if (currentZombieCount == 0)
					progress.value += 10;
				if (progress.value == 100) {
					Checkpoint.GetComponent<ScrollLeft> ().parallaxSpeed = 6;
					play = false;
					hero.GetComponent<sample> ().stopAnimation ();
					Loader.instance.currentLevel = 3;
					Loader.instance.currentSubLevel = 2;
					Loader.instance.SaveFile ();
					inGameAnimator.Play ("GoUpInGame",-1,0);
				}
			} else if (Loader.instance.currentSubLevel == 2) {
				if (currentZombieCount == 0)
					progress.value += 100/15;
				if (progress.value == 100) {
					Checkpoint.GetComponent<ScrollLeft> ().parallaxSpeed = 6;
					play = false;
					hero.GetComponent<sample> ().stopAnimation ();
					Loader.instance.currentLevel = 3;
					Loader.instance.currentSubLevel = 3;
					Loader.instance.SaveFile ();
					inGameAnimator.Play ("GoUpInGame",-1,0);
				}
			} else if (Loader.instance.currentSubLevel == 3) {
				if (currentZombieCount == 0)
					progress.value +=5;
				if (progress.value == 100) {
					LevelComplete.GetComponent<ScrollLeft> ().parallaxSpeed = 6;
					play = false;
					hero.GetComponent<sample> ().stopAnimation ();
					Loader.instance.SaveFile ();
					inGameAnimator.Play ("GoUpInGame",-1,0);
				}
			}
		}

		setText ();	

	}


	public void replay(){
		Time.timeScale = 1;
		zombie = GameObject.FindGameObjectWithTag ("Zombie");
		zombie.GetComponent<AirZombieController> ().SelfDestroy ();
//		Debug.Log ("replay function()");
		InitGame ();
	}

	public void loadLevel(){
		LoadingPanel.SetActive (true);
		LoadingPanelAnimator.Play ("FadeIn", -1, -0);
		LevelCompletePanel.SetActive (false);
		LevelCompletePanelAnim.Play ("MoveUpFinish", -1, 0);
//		progress.value = 0;
//		currentSubLevel = 1;
//		play = false;
//		cyclePlay = true;
//		checkpointResume = false;
		if (SceneManager.GetActiveScene ().name == "Level1") {
			Loader.instance.currentLevel = 2;
			Loader.instance.currentSubLevel = 1;
			StartCoroutine (AsynchronousLoad ("Level2"));
		} else if (SceneManager.GetActiveScene ().name == "Level2") {
			StartCoroutine (AsynchronousLoad ("Level3"));
			Loader.instance.currentLevel = 3;
			Loader.instance.currentSubLevel = 1;
		}
	}


	IEnumerator AsynchronousLoad (string scene)
	{
		yield return new WaitForSeconds(2);

		AsyncOperation ao = SceneManager.LoadSceneAsync(scene);
		ao.allowSceneActivation = false;


		while (! ao.isDone)
		{
			// [0, 0.9] > [0, 1]
			float progress = Mathf.Clamp01(ao.progress / 0.9f);



			//			Debug.log("Loading progress: " + (progress * 100) + "%");
			loader_slide.value = progress;
			// Loading completed
			if (ao.progress == 0.9f)
			{
				//				Debug.Log("Press a key to start");
				//				if (Input.AnyKey())
				ao.allowSceneActivation = true;
			}

			yield return null;
		}
	}
}