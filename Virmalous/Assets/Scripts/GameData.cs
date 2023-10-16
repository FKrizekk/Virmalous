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
	public float MouseSensitivity = 1f;
	
	//Audio
	public float MasterVol = 0.5f;
	public float SfxVol = 0.5f;
	public float MusicVol = 0.5f;
	public float SpeechVol = 0.5f;
	
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
	
	public GameData(GameManager game)
	{
		MouseSensitivity = game.data.MouseSensitivity;
		
		MasterVol = game.data.MasterVol;
		SfxVol = game.data.SfxVol;
		MusicVol = game.data.MusicVol;
		SpeechVol = game.data.SpeechVol;
		
		weaponStatus = game.data.weaponStatus;
		equippedVariants = game.data.equippedVariants;
		
		as0 = game.data.as0;
	}
}