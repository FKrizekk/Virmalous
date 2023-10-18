using System.Collections;
using System.Collections.Generic;
using TMPro;
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
	
	float MoveForce = 15f;
	float AirMoveForce = 12;
	float crouchForce = 7f;
	float SlideForce = 20f;
	float dashForce = 40f;
	float varSlideForce = 20f;
	float maxHorizontalVelocity = 20f;
	float maxAirHorizontalVelocity = 20f;
	float MouseSensitivity = 2f;
	float jumpForce = 13.5f;
	
	bool sliding = false;
	public static bool grounded = false;
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
		cam = GameObject.Find("CameraParent");
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
            PlayJumpSound(surface);

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
		
		Vector3 inputVector;
		
		//Get input
		float moveX = Input.GetAxisRaw("Horizontal");
		float moveY = Input.GetAxisRaw("Vertical");
		if(Mathf.Abs(moveX) == 1 && Mathf.Abs(moveY) == 1)
		{
			inputVector = new Vector3(moveX/1.41421f,0,moveY/1.41421f);
		}else if(moveX == 0 && moveY == 0)
		{
			inputVector = new Vector3(0,0,1);
		}else
		{
			inputVector = new Vector3(moveX, 0, moveY);
		}
		
		slideDirection = transform.TransformDirection(inputVector.normalized);
		
		if(grounded)
		{
			varSlideForce = MoveForce;
		}else{
			varSlideForce = 1f;
		}
		
		transform.localScale = new Vector3(0.5f,0.5f,0.5f);
	}

	void Slide()
	{

		Vector3 inputVector;

		//Get input
		float moveX = Input.GetAxisRaw("Horizontal");
		float moveY = Input.GetAxisRaw("Vertical");
		if (Mathf.Abs(moveX) == 1 && Mathf.Abs(moveY) == 1)
		{
			inputVector = new Vector3(moveX / 1.41421f, 0, moveY / 1.41421f);
		} else
		{
			inputVector = new Vector3(moveX, 0, moveY);
		}

		//Sliding
		if ((moveX != 0 || moveY != 0) && Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.z) > crouchForce+3)
		{
			// Define the source direction and the target direction
			Vector3 sourceDirection = slideDirection;
			Vector3 targetDirection = transform.TransformDirection(inputVector);

			// Calculate the rotation to align the source with the target
			Quaternion startRotation = Quaternion.LookRotation(sourceDirection);
			Quaternion endRotation = Quaternion.LookRotation(targetDirection);

			float step = 0.5f;

			// Interpolate between start and end rotation over several frames
			Quaternion currentRotation = Quaternion.Slerp(startRotation, endRotation, step);

			slideDirection += currentRotation * Vector3.forward;

			rb.velocity = new Vector3(slideDirection.x * varSlideForce, rb.velocity.y, slideDirection.z * varSlideForce);

			varSlideForce = Mathf.Clamp(rb.velocity.magnitude, 0, SlideForce);
		}//Crouching
		else if ((moveX != 0 || moveY != 0) && Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.z) <= crouchForce+3 && grounded)
		{
            Vector3 moveVector = transform.TransformDirection(inputVector) * crouchForce;

            rb.velocity = new Vector3(moveVector.x, rb.velocity.y, moveVector.z);
		}
		else if(moveX == 0 && moveY == 0 && grounded && Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.z) <= crouchForce + 3)
		{
			rb.velocity = new Vector3(0,rb.velocity.y, 0);
		}
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

		Vector3 moveVector = transform.TransformDirection(inputVector) * MoveForce;
		Vector3 airMoveVector = transform.TransformDirection(inputVector) * AirMoveForce;

        //Move Player
        if (grounded)
		{
			rb.velocity = new Vector3(moveVector.x, rb.velocity.y, moveVector.z);
		}else
		{
			rb.AddForce(new Vector3(airMoveVector.x, 0, airMoveVector.z));
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
		float angleDiff;
		if(cam.transform.eulerAngles.x >= 0 && cam.transform.eulerAngles.x <= 90)
		{
			angleDiff = Vector3.Angle(transform.forward, cam.transform.forward) + mouseY;
		}else
		{
			angleDiff = Vector3.Angle(transform.forward, cam.transform.forward) + mouseY*-1;
		}
		if(angleDiff < 90)
		{
			cam.transform.eulerAngles = cam.transform.eulerAngles + new Vector3(mouseY,0,0);
		}
	}

	public void Dash()
	{
		rb.AddForce(rb.velocity.normalized * dashForce, ForceMode.Impulse);
	}

	void OnCollisionEnter(Collision col)
	{
		if(col.gameObject.tag == "Wood" || col.gameObject.tag == "Stone")
		{
			grounded = true;
			PlayLandSound(col.gameObject.tag);
			PlayerScript.uIShake.Shake(0.1f,2f);
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
					source.PlayOneShot(stone.run[stepIndex]);
				}
				catch (System.Exception)
				{
					stepIndex = 0;
					source.PlayOneShot(stone.run[stepIndex]);
					break;
				}
				break;
			case "Wood":
				stepIndex+=1;
				try
				{
					source.PlayOneShot(wood.run[stepIndex]);
				}
				catch (System.Exception)
				{
					stepIndex = 0;
					source.PlayOneShot(wood.run[stepIndex]);
					break;
				}
				break;
			default:
				source.PlayOneShot(wood.run[stepIndex]);
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
					source.PlayOneShot(stone.jump[stepIndex]);
				}
				catch (System.Exception)
				{
					stepIndex = 0;
					source.PlayOneShot(stone.jump[stepIndex]);
					break;
				}
				break;
			case "Wood":
				stepIndex+=1;
				try
				{
					source.PlayOneShot(wood.jump[stepIndex]);
				}
				catch (System.Exception)
				{
					stepIndex = 0;
					source.PlayOneShot(wood.jump[stepIndex]);
					break;
				}
				break;
			default:
				source.PlayOneShot(wood.run[stepIndex]);
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
					source.PlayOneShot(stone.land[stepIndex]);
				}
				catch (System.Exception)
				{
					stepIndex = 0;
					source.PlayOneShot(stone.land[stepIndex]);
					break;
				}
				break;
			case "Wood":
				stepIndex+=1;
				try
				{
					source.PlayOneShot(wood.land[stepIndex]);
				}
				catch (System.Exception)
				{
					stepIndex = 0;
					source.PlayOneShot(wood.land[stepIndex]);
					break;
				}
				break;
			default:
				source.PlayOneShot(wood.run[stepIndex]);
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
		yield return new WaitForSeconds(0.3f);
		StartCoroutine(Footsteps());
	}
}