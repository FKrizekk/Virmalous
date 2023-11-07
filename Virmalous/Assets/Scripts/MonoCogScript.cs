using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonoCogScript : BaseEnemy
{
	GameObject head;
	public GameObject wheel;
	
	// Start is called before the first frame update
	void Start()
	{
		head = GameObject.Find(gameObject.name + "/monocog/Head");
		nav = GetComponent<NavMeshAgent>();
		source = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update()
	{
		RotateHead();
		
		RotateAgent();
		
		
		nav.SetDestination(PlayerScript.cam.transform.position);

		wheel.transform.eulerAngles += new Vector3(0, 0, Time.deltaTime * nav.velocity.magnitude * 20);

        EnemyUpdate();

		//Start smoke VFX when health low
		if(health <= maxHealth / 4) { VFXParents[4].SetActive(true); }
    }
	
	void RotateHead()
	{
		//Get direction to player camera
		Vector3 directionToTarget = PlayerScript.cam.transform.position - head.transform.position;

		//Get quaternion from direction
		Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
		
		//Offset angles so it isnt fucked
		Vector3 targetEuler = new Vector3(-90, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z+90);
		
		//Apply custom rotation
		head.transform.rotation = Quaternion.Lerp(head.transform.rotation, Quaternion.Euler(targetEuler), Time.deltaTime * 20);
	}
	
	void RotateAgent()
	{
		if (nav.velocity.magnitude > 0.1f) // Check if the agent is moving
		{
			// Get the normalized direction of movement
			Vector3 moveDirection = nav.velocity.normalized;

			// Calculate the rotation angle in degrees based on the movement direction
			float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;

			// Rotate the agent smoothly towards the target angle
			Quaternion targetRotation = Quaternion.Euler(0f, targetAngle+90, 0f);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
		}
	}

    void OnCollisionStay(Collision col)
    {
        foreach (ContactPoint contact in col.contacts)
        {
            //Check if can attack and attack
            if (contact.otherCollider.gameObject.tag == "Player" && (Time.time - lastAttackTime) >= 1 / attackRate)
            {
                contact.otherCollider.GetComponentInParent<PlayerScript>().GotHit(damage);
                lastAttackTime = Time.time;
            }
        }
    }

    protected override void Death()
    {
        //Instantiates ragdoll, adds a force away from player to it and destroys the original enemy object
        GameObject ragdollObj = Instantiate(ragdoll, transform.position, Quaternion.identity);
        ragdollObj.transform.GetChild(0).GetChild(1).GetComponent<Rigidbody>().AddForce((transform.position - player.transform.position).normalized * 30, ForceMode.VelocityChange);
        ragdollObj.transform.GetChild(0).GetChild(0).GetComponent<Rigidbody>().AddForce((transform.position - player.transform.position).normalized * 30, ForceMode.VelocityChange);
        Destroy(gameObject);
    }
}