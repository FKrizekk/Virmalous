using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScript : MonoBehaviour
{
	bool isCursorLocked = true; // Initial state: cursor is locked
	
	//Volume
	public static float MasterVol = 0.5f;
	public static float SfxVol = 0.5f;
	public static float MusicVol = 0.5f;
	
	public static GameObject cam;
	
	//Layermask for raycasting
	public static int layerMask;
	
	//public GameObject transfererPrefab;
	
	public GameObject transferer;
	
	public static CameraShake cameraShake;
	
	
	// Start is called before the first frame update
	void Start()
	{
		//Layermask against everything except layer 8 (player)
		layerMask = ~LayerMask.GetMask("Player");
		
		transferer.SetActive(true);
		
		//Get cam
		cam = GameObject.Find("CameraParent");
		
		cameraShake = GameObject.Find("CameraParent/Camera").GetComponent<CameraShake>();
		
		LockCursor();
		
		//Disable vsync
		QualitySettings.vSyncCount = 0;
	}

	// Update is called once per frame
	void Update()
	{
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
		}
	}
	
	void FinishLevel()
	{
		TransfererScript.StartTransfer(gameObject);
	}
}
