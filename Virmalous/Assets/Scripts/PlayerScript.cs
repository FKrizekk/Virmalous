using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScript : MonoBehaviour
{
	public static int[] ammoCounts =
	{
		0,0,0,0
	};

	public Announcer announcer;
	
	//Layermask for raycasting
	public static int layerMask;
	
	public static GameObject cam;

	public static GameManager game;
	
	public GameObject transferer;
	
	public static CameraShake cameraShake;
	public static UIShake uIShake;
	
	
	public static int health = 2000;
	public static int maxHealth = 10000;
	
	Vector3 currentObjectivePos;
	
	//UI
	Image healthBar;
	TMP_Text healthText;
	public static Animator damageOverlayAnim;
	public static Animator healOverlayAnim;

	public static bool isInteracting = false;
	public static bool isReloading = false;
	
	//Start is called before the first frame update
	void Start()
	{
		//Layermask against everything except layer 8 (player)
		layerMask = LayerMask.GetMask("Player") | (1 << 2);
		layerMask = ~layerMask;
		
		transferer.SetActive(true);
		
		//Get cam
		cam = GameObject.Find("CameraParent"); 
		
		cameraShake = GameObject.Find("CameraParent/Camera").GetComponent<CameraShake>();
		uIShake = GameObject.Find("Player/Canvas/UIParent").GetComponent<UIShake>();
		
		healthBar = GameObject.Find("Player/Canvas/UIParent/HealthParent/HealthBar").GetComponent<Image>(); //The Health Bar Image changed based on health
		healthText = GameObject.Find("Player/Canvas/UIParent/HealthParent/HealthText").GetComponent<TMP_Text>();

		healOverlayAnim = GameObject.Find("HealOverlay").GetComponent<Animator>();
        damageOverlayAnim = GameObject.Find("DamageOverlay").GetComponent<Animator>();

		game = GameObject.Find("Level").GetComponent<GameManager>();

        LockCursor(true);
		
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
		healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, (float)health/maxHealth, Time.deltaTime*10);
		//Smoothly update healthText
		string currentHealth = Mathf.Lerp(float.Parse(healthText.text.Split(" / ")[0].Contains(',') ? healthText.text.Split(" / ")[0].Remove(healthText.text.Split(" / ")[0].IndexOf(','), 1) : healthText.text.Split(" / ")[0]), health, Time.deltaTime*10).ToString();
		string maxHealthText = maxHealth.ToString(); if(maxHealthText.Length >= 4){maxHealthText = maxHealthText.Insert(maxHealthText.Length-3, ",");}
		if(currentHealth.Contains('.')){currentHealth = currentHealth.Split('.')[0];}
		if(int.Parse(currentHealth) > (health - 300)){currentHealth = health.ToString();}
		if(currentHealth.Length >= 4){currentHealth = currentHealth.Insert(currentHealth.Length-3, ",");}
		healthText.text = currentHealth + " / " + maxHealthText;
		
		//Check if dead
		if(health <= 0)
		{
			Die();
		}
			
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
		
		InteractionCheck();
	}
	
	void InteractionCheck()
	{
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
	
	public void GotHit(int amount)
	{
		ChangeHealth(-amount);
	}
	
	public static void ChangeHealth(int amount)
	{
		health = Mathf.Clamp(health + amount, 0, maxHealth);
		if(Mathf.Abs(amount) == amount)
		{
			healOverlayAnim.SetBool("GotHealed", true);
		}
		else
		{
			damageOverlayAnim.SetBool("GotHit", true);
		}
	}
	
	
	public static void LockCursor(bool locked)
	{
		Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
		Cursor.visible = !locked;
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
	
	private void OnTriggerEnter(Collider col) {
		if(col.gameObject.tag == "Health")
		{
			ItemsManager.medkitCount++;
			announcer.AnnounceItem("medkit");
			Destroy(col.gameObject);
		}
	}
	
	void FinishLevel()
	{
		TransfererScript.StartTransfer(gameObject);
	}
}
