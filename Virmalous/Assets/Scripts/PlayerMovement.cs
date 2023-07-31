using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class FootstepsPreset
{
	public AudioClip[] run;
	public AudioClip[] jump;
	public AudioClip[] land;
}

public class PlayerMovement : MonoBehaviour
{
	public static bool canMove = true;
	
	public GameObject cam;
	public Rigidbody rb;
	
	float MoveForce = 20f;
	float AirMoveForce = 40f;
	float SlideForce = 25f;
	float maxHorizontalVelocity = 4f;
	float maxAirHorizontalVelocity = 20f;
	float maxVerticalVelocity = 100f;
	float MouseSensitivity = 2f;
	public float groundSpeed = 0f;
	float jumpForce = 13.5f;
	
	bool sliding = false;
	bool grounded = false;
	bool isBoosting = false;

	Vector3 slideDirection;
	
	//Audio
	public FootstepsPreset stone;
	public FootstepsPreset wood;
	
	public AudioSource source;
	int stepIndex = 0;
	string surface = "Stone";
	
	
	// Start is called before the first frame update
	void Start()
	{
		cam = transform.GetChild(0).GetChild(0).gameObject;
		rb = GetComponent<Rigidbody>();
		
		StartCoroutine(Footsteps());
	}

	// Update is called once per frame
	void Update()
	{
		UpdateCamera();
		
		//Check jump
		if(Input.GetKeyDown(KeyCode.Space) && grounded)
		{
			// Keep the horizontal momentum when jumping
			Vector3 horizontalVelocity = rb.velocity;
			horizontalVelocity.y = 0f;
			
			rb.AddForce(new Vector3(0,jumpForce,0), ForceMode.VelocityChange);
			
			rb.velocity += horizontalVelocity;
		}
		
		//Check slide
		if(Input.GetKeyDown("c"))
		{
			SlideStart();
		}
		
		if(Input.GetKeyUp("c"))
		{
			SlideStop();
		}
		
	}
	
	void SlideStart()
	{
		canMove = false;
		sliding = true;
		
		slideDirection = transform.forward;
		Debug.Log(slideDirection);
		
		transform.localScale = new Vector3(0.5f,0.5f,0.5f);
	}
	
	void Slide()
	{
		rb.AddForce(slideDirection * SlideForce, ForceMode.VelocityChange);
	}
	
	void SlideStop()
	{
		canMove = true;
		sliding = false;
		
		transform.localScale = new Vector3(1f,1f,1f);
	}
	
	void FixedUpdate()
	{
		if(canMove)
		{
			Move();
		}else if(sliding)
		{
			Slide();
		}
		
		if(!Input.GetKey("w") && !Input.GetKey("a") && !Input.GetKey("s") && !Input.GetKey("d") && grounded && !sliding)
		{
			rb.AddForce(new Vector3(-rb.velocity.x/3, 0, -rb.velocity.z/3), ForceMode.VelocityChange);
		}else if(!Input.GetKey("w") && !Input.GetKey("a") && !Input.GetKey("s") && !Input.GetKey("d") && !grounded)
		{
			//rb.AddForce(new Vector3(-rb.velocity.x/15, 0, -rb.velocity.z/15));
		}
		
		// Limit the velocity when not boosting
		if (!isBoosting)
		{
			if(grounded)
			{
				// Limit the horizontal velocity on the ground
				Vector3 horizontalVelocity = rb.velocity;
				horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, maxHorizontalVelocity);
				horizontalVelocity.y = rb.velocity.y;
				rb.velocity = horizontalVelocity;
			}else
			{
				// Limit the horizontal velocity in the air
				Vector3 horizontalVelocity = rb.velocity;
				horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, maxAirHorizontalVelocity);
				horizontalVelocity.y = rb.velocity.y;
				rb.velocity = horizontalVelocity;
			}
		}
		
		
	}
	
	void Move()
	{
		Vector3 inputVector;
		
		//Get input
		float moveX = Input.GetAxisRaw("Horizontal");
		float moveY = Input.GetAxisRaw("Vertical");
		if(Mathf.Abs(moveX) == 1 && Mathf.Abs(moveY) == 1)
		{
			inputVector = new Vector3(moveX/1.41421f,0,moveY/1.41421f);
		}else
		{
			inputVector = new Vector3(moveX, 0, moveY);
		}
		
		//Move Player
		if(grounded)
		{
			rb.AddRelativeForce(inputVector * MoveForce, ForceMode.VelocityChange);
		}else
		{
			rb.AddRelativeForce(inputVector * AirMoveForce , ForceMode.Force);
		}
		
		
	}
	
	void UpdateCamera()
	{
		//Get input
		float mouseX = Input.GetAxis("Mouse X") * MouseSensitivity;
		float mouseY = Input.GetAxis("Mouse Y") * -MouseSensitivity;
		
		//Rotate Player (yaw)
		transform.eulerAngles += new Vector3(0,mouseX,0);
		
		//Rotate Camera (pitch)
		cam.transform.eulerAngles = cam.transform.eulerAngles + new Vector3(mouseY,0,0);
	}

	
	void OnCollisionEnter(Collision col)
	{
		if(col.gameObject.tag == "Wood" || col.gameObject.tag == "Stone")
		{
			grounded = true;
			PlayLandSound(col.gameObject.tag);
		}
	}
	
	void OnCollisionStay(Collision collision)
	{
		if (collision.gameObject.CompareTag("Wood"))
		{
			grounded = true;
			surface = "Wood";
		}else if(collision.gameObject.CompareTag("Stone"))
		{
			grounded = true;
			surface = "Stone";
		}
	}
	
	void OnCollisionExit(Collision col)
	{
		if(col.gameObject.tag == "Wood" || col.gameObject.tag == "Stone")
		{
			grounded = false;
			PlayJumpSound(col.gameObject.tag);
		}
	}
	
	void PlayFootstep()
	{
		switch (surface)
		{
			case "Stone":
				stepIndex+=1;
				try
				{
					source.PlayOneShot(stone.run[stepIndex], PlayerScript.MasterVol*PlayerScript.SfxVol);
				}
				catch (System.Exception)
				{
					stepIndex = 0;
					source.PlayOneShot(stone.run[stepIndex], PlayerScript.MasterVol*PlayerScript.SfxVol);
					break;
				}
				break;
			case "Wood":
				stepIndex+=1;
				try
				{
					source.PlayOneShot(wood.run[stepIndex], PlayerScript.MasterVol*PlayerScript.SfxVol);
				}
				catch (System.Exception)
				{
					stepIndex = 0;
					source.PlayOneShot(wood.run[stepIndex], PlayerScript.MasterVol*PlayerScript.SfxVol);
					break;
				}
				break;
			default:
				source.PlayOneShot(wood.run[stepIndex], PlayerScript.MasterVol*PlayerScript.SfxVol);
				break;
		}
	}
	
	void PlayJumpSound(string tag)
	{
		switch (tag)
		{
			case "Stone":
				stepIndex+=1;
				try
				{
					source.PlayOneShot(stone.jump[stepIndex], PlayerScript.MasterVol*PlayerScript.SfxVol);
				}
				catch (System.Exception)
				{
					stepIndex = 0;
					source.PlayOneShot(stone.jump[stepIndex], PlayerScript.MasterVol*PlayerScript.SfxVol);
					break;
				}
				break;
			case "Wood":
				stepIndex+=1;
				try
				{
					source.PlayOneShot(wood.jump[stepIndex], PlayerScript.MasterVol*PlayerScript.SfxVol);
				}
				catch (System.Exception)
				{
					stepIndex = 0;
					source.PlayOneShot(wood.jump[stepIndex], PlayerScript.MasterVol*PlayerScript.SfxVol);
					break;
				}
				break;
			default:
				source.PlayOneShot(wood.run[stepIndex], PlayerScript.MasterVol*PlayerScript.SfxVol);
				break;
		}
	}
	
	void PlayLandSound(string tag)
	{
		switch (tag)
		{
			case "Stone":
				stepIndex+=1;
				try
				{
					source.PlayOneShot(stone.land[stepIndex], PlayerScript.MasterVol*PlayerScript.SfxVol);
				}
				catch (System.Exception)
				{
					stepIndex = 0;
					source.PlayOneShot(stone.land[stepIndex], PlayerScript.MasterVol*PlayerScript.SfxVol);
					break;
				}
				break;
			case "Wood":
				stepIndex+=1;
				try
				{
					source.PlayOneShot(wood.land[stepIndex], PlayerScript.MasterVol*PlayerScript.SfxVol);
				}
				catch (System.Exception)
				{
					stepIndex = 0;
					source.PlayOneShot(wood.land[stepIndex], PlayerScript.MasterVol*PlayerScript.SfxVol);
					break;
				}
				break;
			default:
				source.PlayOneShot(wood.run[stepIndex], PlayerScript.MasterVol*PlayerScript.SfxVol);
				break;
		}
	}
	
	IEnumerator Footsteps()
	{
		yield return new WaitUntil(() => Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d"));
		if(grounded && canMove)
		{
			PlayFootstep();
		}
		yield return new WaitForSeconds(0.2f);
		StartCoroutine(Footsteps());
	}
}