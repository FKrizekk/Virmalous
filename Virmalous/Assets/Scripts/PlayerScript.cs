using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScript : MonoBehaviour
{
	bool isCursorLocked = true; // Initial state: cursor is locked
	
	//Layermask for raycasting
	public static int layerMask;
	
	//Volume
	public static float MasterVol = 0.5f;
	public static float SfxVol = 0.5f;
	public static float MusicVol = 0.5f;
	
	public static GameObject cam;
	
	public GameObject transferer;
	
	public static CameraShake cameraShake;
	public static UIShake uIShake;
	
	
	int health = 500;
	
	Vector3 currentObjectivePos;
	
	//UI
	Image healthBar;
	Image objectiveMarker;

	public static bool isInteracting = false;
	
	//Start is called before the first frame update
	void Start()
	{
		//Layermask against everything except layer 8 (player)
		layerMask = ~LayerMask.GetMask("Player");
		
		transferer.SetActive(true);
		
		//Get cam
		cam = GameObject.Find("CameraParent"); 
		
		cameraShake = GameObject.Find("CameraParent/Camera").GetComponent<CameraShake>();
		uIShake = GameObject.Find("Player/Canvas/UIParent").GetComponent<UIShake>();
		
		healthBar = GameObject.Find("Player/Canvas/UIParent/HealthParent/HealthBar").GetComponent<Image>(); //The Health Bar Image changed based on health
		objectiveMarker = GameObject.Find("Player/Canvas/UIParent/ObjectiveParent/ObjectiveMarker").GetComponent<Image>();
		
		LockCursor();
		
		//Disable vsync
		QualitySettings.vSyncCount = 0;
	}
	
	void Die()
	{
		Application.Quit();
	}

	//Update is called once per frame
	void Update()
	{
		//Smoothly update healthBar
		healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, (float)health/500f, Time.deltaTime*10);
		
		//Update Objective Marker Position
		Vector3 screenPos = cam.transform.GetChild(0).gameObject.GetComponent<Camera>().WorldToScreenPoint(currentObjectivePos); //Convert world coordinates to screen coordinates
		objectiveMarker.transform.position = screenPos;
		
		//Check if dead
		if(health <= 0)
		{
			Die();
		}
		
		//-------------------------OBJECTIVE DEBUGGGGG-----------------------------
		if(Input.GetMouseButtonDown(1))
		{
			RaycastHit hit;
			if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Mathf.Infinity, layerMask))
			{
				currentObjectivePos = hit.point;
			}
		}
		//-------------------------OBJECTIVE DEBUGGGGG-----------------------------
		
		
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
		
		RaycastHit hit2;
		if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit2, 6, layerMask))
		{
			if(hit2.collider.gameObject.tag == "Screen")
			{
				isInteracting = true;
			}else
			{
				isInteracting = false;
			}
		}
	}
	
	public void ChangeHealth(int amount)
	{
		health = Mathf.Clamp(health + amount, 0, 500);
	}
	
	
	private void LockCursor()
	{
		Cursor.lockState = isCursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
		Cursor.visible = !isCursorLocked;
	}
	
	void OnCollisionEnter(Collision col) {
		if(col.gameObject.tag == "Finish")
		{
			//Touched crystal pedestal
			FinishLevel();
			col.gameObject.GetComponent<PedestalScript>().GetCrystal();
		}
		if(col.gameObject.tag == "Lava")
		{
			//Touched LAVA
			ChangeHealth(-100);
		}
	}
	
	void FinishLevel()
	{
		TransfererScript.StartTransfer(gameObject);
	}
}
