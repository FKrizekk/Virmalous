using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalGunScript : MonoBehaviour
{
	GunScript gunParent;
	
	void Start()
	{
		gunParent = transform.parent.gameObject.GetComponent<GunScript>();
	}
	
	public void PlaySound(int index)
	{
		gunParent.PlaySound(index);
	}
	
	public void SetPitch(float pitch)
	{
		gunParent.SetPitch(pitch);
	}
	
	public void RealShoot()
	{
		gunParent.RealShoot();
	}
	
	public void StopShoot()
	{
		gunParent.StopShoot();
	}
}
