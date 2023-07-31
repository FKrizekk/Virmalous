using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	
	public GameObject transfererPrefab;
	
	GameObject transferer;
	
	
	// Start is called before the first frame update
	void Start()
	{
		//Layermask against everything except layer 8 (player)
		layerMask = ~LayerMask.GetMask("Player");
		
		transferer = GameObject.Find("Transferer");
		
		//Get cam
		cam = transform.GetChild(0).GetChild(0).gameObject;
		
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
