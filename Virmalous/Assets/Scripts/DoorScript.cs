using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
	public AudioSource source;
	public AudioClip doorSound;
	
	public void AnimationStart()
	{
		source.PlayOneShot(doorSound, PlayerScript.MasterVol*PlayerScript.SfxVol);
		PlayerScript.cameraShake.Shake(1f,0.2f);
	}
}
