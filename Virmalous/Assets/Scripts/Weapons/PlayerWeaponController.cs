using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



public class PlayerWeaponController : MonoBehaviour
{
	public GameObject[] weapons; //Prefabs for all weapons (indexed in the ideas doc)
	GameObject obj; //Instantiated/Equipped weapon
	
	GameObject gunParent;

	GameManager game;
	int currentIndex = -1;
	
	void Start()
	{
		gunParent = GameObject.Find("GUNPARENT");
		game = GameObject.Find("Level").GetComponent<GameManager>();
	}
	
	void Update()
	{
		if(Input.GetKeyDown("1") && currentIndex != 0 + game.data.equippedVariants[0])
		{
			SetGun(0 + game.data.equippedVariants[0]);
		}
		else if(Input.GetKeyDown("2") && currentIndex != 4 + game.data.equippedVariants[1])
		{
            SetGun(4 + game.data.equippedVariants[1]);
        }
	}
	
	void SetGun(int index)
	{
		currentIndex = index;
		PlayerScript.isReloading = false;

		if(obj != null)
		{
			Destroy(obj);
		}
		
		obj = Instantiate(weapons[index], transform.TransformPoint(new Vector3(0,-1,-1)), Quaternion.Euler(new Vector3(0,0,90)), transform);
		obj.transform.SetParent(gunParent.transform);
	}
	
	
	
}
