using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
	NavMeshAgent nav;
	public GameObject player;
	public Animator anim;
	
	//Audio
	public AudioSource source;
	public AudioClip[] clips;
	
	int health = 100;
	int damage = 500;
	float attackCooldown = 1.5f;
	float lastAttackTime = 0;
	
	public GameObject ragdoll;
	public GameObject blood;
	
	Vector3 killPoint;
	
	// Start is called before the first frame update
	void Start()
	{
		nav = GetComponent<NavMeshAgent>();
		player = GameObject.Find("Player");
		
		StartCoroutine(Screaming());
	}
	
	IEnumerator Screaming()
	{
		yield return new WaitForSeconds(Random.Range(2.5f, 9f));
		PlaySound(0);
	}

	// Update is called once per frame
	void Update()
	{
		//Update animator state
		anim.SetBool("Walking", nav.hasPath);

		int layerMask = 1 << 6 | 1 << 2;
		layerMask = ~layerMask;
		
		RaycastHit hit;
		if(Physics.Raycast(transform.position, player.transform.position-transform.position, out hit, Mathf.Infinity, layerMask))
		{
			if(hit.collider.gameObject.tag == "Player")
			{
				MoveTo(player.transform.position);
			}
		}
		
		//Check if ded
		if(health <= 0)
		{
			Death();
		}
	}
	
	private void OnCollisionStay(Collision col) {
		foreach (ContactPoint contact in col.contacts)
		{
			Debug.Log(contact.otherCollider.gameObject.name);
			if(contact.otherCollider.gameObject.tag == "Player" && (Time.time - lastAttackTime) >= attackCooldown)
			{
				contact.otherCollider.gameObject.transform.parent.gameObject.GetComponent<PlayerScript>().GotHit(damage);
				lastAttackTime = Time.time;
			}
		}
	}
	
	void MoveTo(Vector3 targetPos)
	{
		nav.SetDestination(targetPos);
	}
	
	public void GotHit(int amount, Vector3 point)
	{
		killPoint = point;
		health -= amount;
		Bleed();
	}
	
	void Bleed()
	{
		Instantiate(blood, killPoint, Quaternion.identity);
	}
	
	void Death()
	{
		GameObject deadguy = Instantiate(ragdoll, transform.position, Quaternion.identity);
		deadguy.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Rigidbody>().AddForce((transform.position - player.transform.position).normalized * 100, ForceMode.VelocityChange);
		Destroy(gameObject);
	}
	
	public void PlaySound(int index)
	{
		source.PlayOneShot(clips[index], PlayerScript.MasterVol*PlayerScript.SfxVol);
	}
}
