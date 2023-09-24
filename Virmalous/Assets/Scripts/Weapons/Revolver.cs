using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The Revolver is a basic hitscan handgun without any special stuff
public class Revolver : MonoBehaviour
{
	//STATS
	int damage = 50; //How much damage 1 bullet does
	int maxAmmo = 9; //Magazine capacity
	int ammo = 9; //Current amount of loaded bullets
	float reloadTime = 2f; //Reload time in seconds
	float fireRate = 1f; //Shots per seconds
	
	//Vector3 relativePos = new Vector3(0.349999994f,-0.389999986f,0.610000014f); //Position of the gun relative to the camera
	Vector3 relativePos = new Vector3(0.379999995f,-0.360000014f,0.790000021f);
	public AudioClip[] clips;
	public AudioSource source;
	public Animator anim;
	
	public ParticleSystem particleSystem;
	
	GameObject cam;
	GameObject player;
	Rigidbody rb;
	
	bool shooting = false;
	
	float swaySpeed = 5f;
	float swayAmount = 0.2f;
	
	public GameObject barrel;
	
	void Start()
	{
		cam = GameObject.Find("CameraParent/Camera");
		
		player = GameObject.Find("Player");
		rb = player.GetComponent<Rigidbody>();
	}
	
	void Update()
	{
		SwayAndRotate();
		
		if(Input.GetMouseButtonDown(0))
		{
			if(!shooting && !PlayerScript.isInteracting)
			{
				Shoot();
			}
		}
	}
	
	void SwayAndRotate()
	{
		//Rotate gun towards reticle world position	
		RaycastHit hit;
		if(Physics.Raycast(PlayerScript.cam.transform.position, PlayerScript.cam.transform.forward, out hit, Mathf.Infinity, PlayerScript.layerMask))
		{
			// Calculate the direction from the gun's current position to the hit point
			Vector3 directionToTarget = hit.point - barrel.transform.position;
			
			// Calculate the rotation needed to face the target point
			Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
			
			// Rotate the gun smoothly towards the target point using Slerp (Spherical Linear Interpolation)
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10);
		}
		
		float distanceToPoint = Vector3.Distance(hit.point, PlayerScript.cam.transform.position);
		
		//Sway gun
		Vector3 swayDirection;
		// Calculate the opposite direction of player's velocity
		if(Mathf.Abs(rb.velocity.x) > 1 || Mathf.Abs(rb.velocity.y) > 1 || Mathf.Abs(rb.velocity.z) > 1)
		{
			swayDirection = player.transform.InverseTransformDirection(-rb.velocity.normalized);
		}else
		{
			swayDirection = Vector3.zero;
		}

		float distanceThreshold = 1.5f;
		// Calculate the target position based on the sway direction and the distance to the hit.point
		Vector3 targetPosition = relativePos + (swayDirection * swayAmount) + (distanceToPoint < distanceThreshold ? -Vector3.forward * (distanceThreshold-distanceToPoint) : Vector3.zero);

		// Smoothly move the gun towards the target position
		transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, swaySpeed * Time.deltaTime);
	}
	
	public void Shoot()
	{
		anim.SetBool("Shoot",true);
		shooting = true;
		particleSystem.Play();
		
		//Shooting logic
		RaycastHit hit;
		if(Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, PlayerScript.layerMask))
		{
			if(hit.collider.tag == "Enemy")
			{
				hit.collider.gameObject.GetComponent<EnemyScript>().GotHit(damage,hit.point);
			}else if(hit.collider.tag == "EnemyCrit")
			{
				hit.collider.gameObject.transform.parent.gameObject.GetComponent<EnemyScript>().GotHit(damage*2,hit.point);
			}
		}
		
		/* //Shooting Logic
		RaycastHit[] hits = Physics.SphereCastAll(cam.transform.position, 1, cam.transform.TransformDirection(Vector3.forward), Mathf.Infinity, PlayerScript.layerMask);
		foreach(var hit in hits)
		{
			if(hit.collider.tag == "Enemy")
			{
				hit.collider.gameObject.GetComponent<EnemyScript>().GotHit(damage,hit.point);
			}else if(hit.collider.tag == "EnemyCrit")
			{
				hit.collider.gameObject.transform.parent.gameObject.GetComponent<EnemyScript>().GotHit(damage*2,hit.point);
			}
		} */
	}
	
	public void StopShoot() //This is called at the end of the shoot animation so that the script know that it can shoot
	{
		anim.SetBool("Shoot", false);
		shooting = false;
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
	
}