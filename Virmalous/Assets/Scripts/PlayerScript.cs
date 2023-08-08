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
		
		healthBar = GameObject.Find("Player/Canvas/UIParent/HealthParent/HealthBar").GetComponent<Image>();
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
		
		if(Input.GetMouseButtonDown(1))
		{
			RaycastHit hit;
			if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Mathf.Infinity, layerMask))
			{
				currentObjectivePos = hit.point;
			}
		}
		
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
		
		/* // Check for crosshair-based UI interaction
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = cam.GetComponent<Camera>().ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

			if (Physics.Raycast(ray, out RaycastHit hit))
			{
				Debug.Log("Raycast hit: " + hit.collider.gameObject.name);
				// Check if the collider has a Unity UI button
				Button button = hit.collider.GetComponent<Button>();
				if (button != null)
				{
					// Check if the hit point is within the bounds of the button
					RectTransform buttonRectTransform = button.GetComponent<RectTransform>();
					Vector3 localHitPoint;
					if (RectTransformUtility.ScreenPointToWorldPointInRectangle(buttonRectTransform, hit.point, cam.GetComponent<Camera>(), out localHitPoint))
					{
						// Perform the button click
						button.onClick.Invoke();
					}
				}
			}
		} */
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
