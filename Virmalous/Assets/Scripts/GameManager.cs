using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public GameData data;
    public AudioMixer mixer;

    

    private void Awake()
    {
        data = SaveSystem.LoadGame();
    }

    private void Update()
    {
        //Update audio mixer
        mixer.SetFloat("MasterVol", data.MasterVol);
        mixer.SetFloat("SfxVol", data.SfxVol);
        mixer.SetFloat("MusicVol", data.MusicVol);
        mixer.SetFloat("SpeechVol", data.SpeechVol);
    }
}