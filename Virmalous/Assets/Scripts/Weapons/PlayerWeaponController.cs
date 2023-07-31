using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerWeaponController : MonoBehaviour
{
	public GameObject[] weapons; //Prefabs for all weapons (indexed in the ideas doc)
	GameObject obj; //Instantiated/Equipped weapon
	
	GameObject gunParent;
	
	void Start()
	{
		gunParent = GameObject.Find("GUNPARENT");
	}
	
	void Update()
	{
		if(Input.GetKeyDown("1"))
		{
			SetGun(0);
		}
	}
	
	void SetGun(int index)
	{
		if(obj != null)
		{
			Destroy(obj);
		}
		
		obj = Instantiate(weapons[index], transform.TransformPoint(new Vector3(0,-1,-1)), Quaternion.EulerAngles(new Vector3(0,0,90)), transform);
		obj.transform.SetParent(gunParent.transform);
	}
	
	
	
}
