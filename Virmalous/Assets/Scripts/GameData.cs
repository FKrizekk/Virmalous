using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level
{
	public bool isCompleted = false;
	
	public bool chestLooted = false;
	public int enemiesKilled = 0;
	public int maxEnemies = 99;
	
	public float time = 0;
}

[System.Serializable]
public class Assignment
{
	public Level[] levels;
	
	public bool isCompleted = false;
}

[System.Serializable]
public class GameData
{
	//Controls
	public float MouseSensitivity = 1.5f;

	//Audio
	public float MasterVol = -30f;
	public float SfxVol = -30f;
	public float MusicVol = -30f;
	public float SpeechVol = -30f;
	
	//Progress
	
	//IsWeaponUnlocked by index (in docs)
	public List<bool> weaponStatus = new List<bool>
	{
		false,
		false,
		false,
		false,
		false,
		false,
		false,
		false,
		false,
		false,
		false,
		false,
		false,
		false,
		false,
		false,
		false,
		false
	};
	public List<int> equippedVariants = new List<int>
	{
		0,
		0,
		0,
		0
	};
	
	//Assignments
	public Assignment as0 = new Assignment
	{
		levels = new Level[]
		{
			new Level
			{
				isCompleted = false,
				chestLooted = false,
				enemiesKilled = 0,
				maxEnemies = 99,
				time = 0f
			},
			new Level
			{
				isCompleted = false,
				chestLooted = false,
				enemiesKilled = 0,
				maxEnemies = 99,
				time = 0f
			}
		},
		
		isCompleted = false
	};
}