using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Text1Controller : MonoBehaviour {
	public Text textBox;
	public GameObject next;
	public GameObject info;

	//Store all your text in this string array
	string[] goatText = new string[]{
		"Millions Dead and the rest of the World struggling to stay alive.",
		"2 Days since the Alien invasion,\nwith Hillwood down to rumbles,\nOnly one place left to survive,\n \"Hailing Hallows\".",
		"The last remaining Flight takes off soon.....",
		"Reach the Airport before Willowfield becomes Zombie Land. ",
		"RIDE THROUGH HELL"
	};
	int currentlyDisplayingText = 0;

	public void startAnimation(){
		next.SetActive (false);
		info.SetActive (true);
		StartCoroutine(AnimateText());
	}


	//This is a function for a button you press to skip to the next text
	public void SkipToNextText(){ 
		next.SetActive (false);
		currentlyDisplayingText++;
		//If we've reached the end of the array, do anything you want. I just restart the example text
		if (currentlyDisplayingText >= goatText.Length) {
			info.SetActive (false);
			currentlyDisplayingText = 0;
		} else {
			StartCoroutine (AnimateText ());
		}
	}
	//Note that the speed you want the typewriter effect to be going at is the yield waitforseconds (in my case it's 1 letter for every      0.03 seconds, replace this with a public float if you want to experiment with speed in from the editor)
	IEnumerator AnimateText(){

		for (int i = 0; i < (goatText[currentlyDisplayingText].Length+1); i++)
		{
			if (currentlyDisplayingText == 4) {
				textBox.color = new Color(255.0f/255.0f, 56.0f/255.0f, 56.0f/255.0f);
				textBox.fontSize = 80;
				textBox.alignment = TextAnchor.MiddleCenter;
			}
			textBox.text = goatText[currentlyDisplayingText].Substring(0, i);
			yield return new WaitForSeconds(.05f);
		}
		next.SetActive (true);
	}



}
