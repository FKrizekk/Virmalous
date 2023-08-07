using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jukebox : MonoBehaviour
{
	AudioSource source;
	
	public AudioClip[] clips;
	
	void Start()
	{
		source = GameObject.Find("CameraParent/Camera").GetComponent<AudioSource>();
		
		//source.PlayOneShot(clips[0], PlayerScript.MasterVol*PlayerScript.MusicVol);
	}
}
