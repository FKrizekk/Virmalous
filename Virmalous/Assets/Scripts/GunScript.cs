using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class Gun
{
	public GameObject prefab;
	public AudioClip[] clips;
}

public class GunScript : MonoBehaviour
{
	//VARIABLE WEAPONS STATS
	GameObject obj; //The instantiated gun gameobject
	
	GameObject prefab; //The prefab to be instantiated
	Vector3 relativePos; //Relative position of the gun to the camera
	int ammo; //Current ammo count
	int maxAmmo; //Max ammo/Full reloaded mag size
	float shotsPerSec; //--------------------------------------------MAYBE DELETE THIS IDK KINDA SAME AS RECHAMBER TIME BUT OPPOSITE
	float rechamberTime; //Time between shots in seconds
	int bulletCount; //Amount of bullets fired in one shot
	int bulletDamage; //Damage each bullet does
	float spread;
	
	bool isHitscan;
	
	ParticleSystem particleSystem; //ParticleSystem for hitscan weapons
	
	AudioSource source; //AudioSource of the current weapon
	Animator anim; //Animator of the current weapon
	AudioClip[] clips; //AudioClips of the current weapon
	
	
	//WEAPON stuff	
	public Gun gun0;
	public Gun gun1;
	public Gun gun2;
	public Gun gun3;
	
	PlayerScript player;
	GameObject cam;
	
	
	//Sway settings
	float swayAmount = 0.2f;
	float swaySpeed = 7f;
	
	//Deletes old gun, Instantiates new one and sets stats
	public void SetGun(int index)
	{
		if(obj != null)
		{
			Destroy(obj);
		}
		
		switch (index)
		{
			case 0:
				prefab = gun0.prefab;
				relativePos = new Vector3(0.460000008f,-0.540000021f,0.980000019f);
				
				maxAmmo = 9;
				ammo = maxAmmo;
				shotsPerSec = 1f;
				rechamberTime = 1f;
				bulletCount = 1;
				bulletDamage = 50;
				spread = 0;
				
				isHitscan = true;
				
				clips = gun0.clips;
				break;
				
			case 1:
				
				break;
			
			
		}
		
		obj = Instantiate(prefab, transform.TransformPoint(new Vector3(0,-1,-1)), Quaternion.EulerAngles(new Vector3(0,0,90)), transform);
		
		if(isHitscan)
		{
			particleSystem = GameObject.Find(obj.name+"/Particle System").GetComponent<ParticleSystem>();
		}
		source = obj.GetComponent<AudioSource>();
		anim = obj.GetComponent<Animator>();
	}
	
	//Start shooting animation
	public void Shoot()
	{
		anim.SetBool("Shoot",true);
	}
	
	void Start()
	{
		player = GameObject.Find("Player").GetComponent<PlayerScript>();
		cam = GameObject.Find("Camera");
		
		SetGun(0);
	}
	
	public void PlaySound(int index)
	{
		float customVol = 1f;
		
		if(index == 1)
		{
			customVol = 0.3f;
		}
		
		float vol = PlayerScript.MasterVol * PlayerScript.SfxVol * customVol;
		source.PlayOneShot(clips[index], vol);
	}
	
	public void SetPitch(float pitch)
	{
		source.pitch = pitch;
	}
	
	public void StopShoot()
	{
		anim.SetBool("Shoot", false);
	}
	
	public void RealShoot()
	{
		RaycastHit[] hits = Physics.SphereCastAll(cam.transform.position, 1, cam.transform.TransformDirection(Vector3.forward), Mathf.Infinity, PlayerScript.layerMask);
		foreach(var hit in hits)
		{
			if(hit.collider.tag == "Enemy")
			{
				hit.collider.gameObject.GetComponent<Enemy1Script>().GotHit(hit.point);
				Debug.Log("HIT");
				Debug.Log(hit.collider.tag);
			}else if(hit.collider.tag == "EnemyCrit")
			{
				hit.collider.gameObject.transform.parent.gameObject.GetComponent<Enemy1Script>().GotHitCrit(hit.point);
				Debug.Log("CRIT");
				Debug.Log(hit.collider.tag);
			}
		}
		particleSystem.Play();
	}
	
	void Update()
	{
		//Sway gun
		GameObject player = transform.parent.parent.parent.gameObject;
		Rigidbody rb = player.GetComponent<Rigidbody>();
		
		// Calculate the opposite direction of player's velocity
		Vector3 swayDirection = player.transform.InverseTransformDirection(-rb.velocity.normalized);
		
		// Calculate the target position based on the sway direction
		Vector3 targetPosition = relativePos + swayDirection * swayAmount;

		// Smoothly move the gun towards the target position
		obj.transform.localPosition = Vector3.Lerp(obj.transform.localPosition, targetPosition, swaySpeed * Time.deltaTime);
		
		RaycastHit hit;
		if(Physics.Raycast(PlayerScript.cam.transform.position, PlayerScript.cam.transform.forward, out hit, Mathf.Infinity, PlayerScript.layerMask))
		{
			Debug.DrawRay(PlayerScript.cam.transform.position, PlayerScript.cam.transform.forward*100);
			//obj.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation((hit.point - transform.position), Vector3.up), Time.deltaTime*10);
			// Calculate the direction from the gun's current position to the hit point
			Vector3 directionToTarget = hit.point - obj.transform.position;
			
			// Calculate the rotation needed to face the target point
			Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
			
			// Rotate the gun smoothly towards the target point using Slerp (Spherical Linear Interpolation)
			obj.transform.rotation = Quaternion.Slerp(obj.transform.rotation, targetRotation, Time.deltaTime * 10);
		}
	}
}