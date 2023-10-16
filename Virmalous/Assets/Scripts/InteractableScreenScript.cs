using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractableScreenScript : MonoBehaviour
{
	public GameObject obj;
	
	public string status1;
	public string status2;
	
	string currentStatus;
	
	//Text on the interactable button
	public TMP_Text buttonText;
	
	//Animator for turning off and on
	Animator anim;
	
	//Main camera for ui interaction stuff
	Camera eventCamera;
	
	public AudioSource source;
	public AudioClip clip;
	
	void Start()
	{
		currentStatus = status1;
		
		// Get the Canvas component from the Canvas GameObject
		Canvas canvas = transform.GetChild(0).gameObject.GetComponent<Canvas>();

		// Get the Camera component from the Camera GameObject
		eventCamera = GameObject.Find("Camera").GetComponent<Camera>();

		// Assign the Event Camera
		canvas.worldCamera = eventCamera;
		
		//Assing the Animator
		anim = GetComponent<Animator>();
		
		StartCoroutine(stateCheck());
	}
	
	//Checks if player is close or not and turns on or off based on that
	IEnumerator stateCheck()
	{
		//Wait until player is close
		yield return new WaitUntil(() => Vector3.Distance(eventCamera.gameObject.transform.position, transform.position) < 10);
		anim.SetBool("isOn", true);
		
		//Wait until player is not close
		yield return new WaitUntil(() => Vector3.Distance(eventCamera.gameObject.transform.position, transform.position) > 10);
		anim.SetBool("isOn", false);
		
		StartCoroutine(stateCheck());
	}
	
	//The function the button on the canvas calls
	public void ButtonPressed()
	{
		source.PlayOneShot(clip);
		
		if(currentStatus == status1)
		{
			currentStatus = status2;
			if(obj.tag == "Door")
			{
				obj.GetComponent<Animator>().SetBool("isOpen", true);
				
			}
		}else
		{
			currentStatus = status1;
			if(obj.tag == "Door")
			{
				obj.GetComponent<Animator>().SetBool("isOpen", false);
				
			}
		}
		buttonText.text = currentStatus;
	}
}
