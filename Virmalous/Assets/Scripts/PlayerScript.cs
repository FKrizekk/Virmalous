using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScript : Entity
{
	public static int[] ammoCounts =
	{
		0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
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
	public static bool canMove = true;
	
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
		base.Update();

        //Smoothly update healthBar
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, (float)health/maxHealth, 0.2f);
		//Smoothly update healthText
		//string currentHealth = Mathf.Lerp(float.Parse(healthText.text.Split(" / ")[0].Contains(',') ? healthText.text.Split(" / ")[0].Remove(healthText.text.Split(" / ")[0].IndexOf(','), 1) : healthText.text.Split(" / ")[0]), health, 0.2f).ToString();
		string maxHealthText = maxHealth.ToString(); if(maxHealthText.Length >= 4){maxHealthText = maxHealthText.Insert(maxHealthText.Length-3, ",");}
		//if(currentHealth.Contains('.')){currentHealth = currentHealth.Split('.')[0];}
		//if(Mathf.Abs(int.Parse(currentHealth) - health) < 10){currentHealth = health.ToString();}
		string currentHealth = health.ToString();
		if(currentHealth.Length >= 4){currentHealth = currentHealth.Insert(currentHealth.Length-3, ",");}
		healthText.text = currentHealth + " / " + maxHealthText;
		
		//Check if dead
		if(health <= 0)
		{
			Die();
		}
		
		InteractionCheck();
	}
	
	void InteractionCheck()
	{
		RaycastHit hit2;
		if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit2, 6, layerMask))
		{
			if(hit2.collider.gameObject.tag == "Interactable")
			{
				isInteracting = true;
			}else
			{
				isInteracting = false;
			}
		}
	}
	
	public void GotHit(DamageInfo hit)
	{
        float amount = hit.damage +
            (hit.stunDamage * entityState.stunDamageMultiplier) +
            (hit.fireDamage * entityState.fireDamageMultiplier) +
            (hit.freezeDamage * entityState.freezeDamageMultiplier) +
            (hit.electricityDamage * entityState.electricityDamageMultiplier);

        ChangeHealth(-(int)amount);

        entityState._stunned += hit.stunDamage * entityState.stunDamageMultiplier / 100f;
        entityState._onFire += hit.fireDamage * entityState.fireDamageMultiplier / 100f;
        entityState._frozen += hit.freezeDamage * entityState.freezeDamageMultiplier / 100f;
        entityState._electrified += hit.electricityDamage * entityState.electricityDamageMultiplier / 100f;
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
