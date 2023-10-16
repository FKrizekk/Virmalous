using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public GameData data;
	
	
	void Update()
	{
		if(Input.GetKeyDown("t"))
		{
			SaveSystem.SaveGame(this);
		}
		if(Input.GetKeyDown("y"))
		{
			data = SaveSystem.LoadGame();
		}
	}
}