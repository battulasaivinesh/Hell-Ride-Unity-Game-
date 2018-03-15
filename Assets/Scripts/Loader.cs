using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;


public class Loader : MonoBehaviour {
	public static Loader instance = null;  
	//Awake is always called before any Start functions


	public int currentLevel;
	public Slider progressSlider;
	public int currentSubLevel;
	public bool first_time;
	void Awake()
	{
		//Check if instance already exists
		if (instance == null)

			//if not, set instance to this
			instance = this;

		//If instance already exists and it's not this:
		else if (instance != this)

			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy(gameObject);    

		//Sets this to not be destroyed when reloading scene
			DontDestroyOnLoad(gameObject);

		InitGame();

	}

	//Initializes the game for each level.
	void InitGame()
	{
		//Init Variables
		LoadFile();

		if (currentLevel == 1) {
			StartCoroutine (AsynchronousLoad ("Level1"));
		} else if (currentLevel == 2) {
			StartCoroutine (AsynchronousLoad ("Level2"));
		} else {
			StartCoroutine (AsynchronousLoad ("Level3"));
		}
	}





	public void SaveFile()
	{
//		Debug.Log ("Saving");
		string destination = Application.persistentDataPath + "/HellRideSaveFile.dat";
		FileStream file;

		if(File.Exists(destination)) file = File.OpenWrite(destination);
		else file = File.Create(destination);

		int level;

	
//		if (SceneManager.GetActiveScene ().name == "Level1")
//			currentLevel = 1;
//		else if (SceneManager.GetActiveScene ().name == "Level2")
//			currentLevel = 2;
//		else
//			currentLevel = 3;

		GameData data = new GameData(currentSubLevel, currentLevel);
		BinaryFormatter bf = new BinaryFormatter();
		bf.Serialize(file, data);
		file.Close();
	}

	public void LoadFile()
	{
		string destination = Application.persistentDataPath + "/HellRideSaveFile.dat";
		FileStream file;
//		Debug.Log ("LOAD FILE");
		if (File.Exists (destination)) {
//			Debug.Log ("FILE EXISTS");
			file = File.OpenRead (destination);
			BinaryFormatter bf = new BinaryFormatter();
			GameData data = (GameData) bf.Deserialize(file);
			file.Close();

			currentSubLevel = data.currentSubLevel;
			currentLevel = data.currentLevel;
			first_time = false;

//			Debug.Log ("Current Level form File - " + data.currentLevel);
//			Debug.Log ("Current Sub Level form File - " + data.currentSubLevel);
		}
		else
		{
//			Debug.Log ("NEW FILE");
			file = File.Create(destination);
			GameData data = new GameData(1, 1);
			currentSubLevel = 1;
			currentLevel = 1;
			BinaryFormatter bf = new BinaryFormatter();
			bf.Serialize(file, data);
			file.Close();
			first_time = true;
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

			progressSlider.value = progress;

			//			Debug.log("Loading progress: " + (progress * 100) + "%");
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
