using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
	public int currentLevel;
	public int currentSubLevel;

	public GameData(int subL, int cL)
	{
		currentLevel = cL;
		currentSubLevel = subL;
	}
}
