using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public GameData data;

    private void Start()
    {
        data = SaveSystem.LoadGame();
        Debug.Log(data.MasterVol);
    }

    private void Update()
    {
        if (Input.GetKeyDown("t"))
        {
            data.MasterVol = 20f;
        }
        if (Input.GetKeyDown("i"))
        {
            SaveSystem.SaveGame(this);
        }
    }
}