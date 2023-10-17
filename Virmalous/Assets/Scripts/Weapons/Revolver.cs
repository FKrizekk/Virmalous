using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The Revolver is a basic hitscan handgun without any special stuff
public class Revolver : BaseWeapon
{
	public ParticleSystem rayParticleSystem;
	
	void Update()
	{
		WeaponUpdate();

        if (Input.GetMouseButtonDown(0) && Time.time - lastShotTime > 1 / firerate && !PlayerScript.isInteracting && !PlayerScript.isReloading)
        {
            if (PlayerScript.ammoCounts[weaponIndex] > 0) { Shoot(); } else { Reload(); }
        }

        if (Input.GetKeyDown("r") && !PlayerScript.isReloading)
        {
            Reload();
        }
    }
	
	public void Shoot()
	{
		PlayerScript.ammoCounts[weaponIndex]--;
		anim.SetBool("Shoot",true);
		lastShotTime = Time.time;
		muzzleFlash.Play();
        rayParticleSystem.Play();
		
		//Shooting logic
		RaycastHit hit;
		if(Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, PlayerScript.layerMask))
		{
			if(hit.collider.tag == "Enemy")
			{
				hit.collider.gameObject.GetComponent<HitCollider>().Hit(damage,hit.point);
			}else if(hit.collider.tag == "EnemyCrit")
			{
				hit.collider.gameObject.transform.parent.gameObject.GetComponent<HitCollider>().Hit(damage*2,hit.point);
			}
		}
	}

	void Reload()
	{
		anim.SetBool("Reload", true);
	}

	public void ReloadAmmo()
	{
		PlayerScript.ammoCounts[weaponIndex] = maxAmmo;
	}
}