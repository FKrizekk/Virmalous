using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

//The Revolver is a basic hitscan handgun without any special stuff
public class Revolver : BaseWeapon
{
	public ParticleSystem rayParticleSystem;
	
	void Update()
	{
		WeaponUpdate();
		RotateParticleSystem();

        if (Input.GetMouseButtonDown(0) && Time.time - lastShotTime > 1 / firerate && !PlayerScript.isInteracting && !PlayerScript.isReloading && Time.timeScale != 0f)
        {
            if (PlayerScript.ammoCounts[weaponIndex] > 0) { Shoot(); } else { Reload(); }
        }

        if (Input.GetKeyDown("r") && !PlayerScript.isReloading)
        {
            Reload();
        }
    }

	void RotateParticleSystem()
	{
		RaycastHit hit;
		if(Physics.Raycast(cam.transform.position,cam.transform.forward, out hit, Mathf.Infinity, PlayerScript.layerMask))
		{
			rayParticleSystem.transform.rotation = Quaternion.LookRotation(hit.point - rayParticleSystem.transform.position);
		}
	}
	
	public void Shoot()
	{
		PlayerScript.ammoCounts[weaponIndex]--;
		anim.SetBool("Shoot",true);
		Shake(0.15f, 0.2f);
		lastShotTime = Time.time;
		muzzleFlash.Play();
		Instantiate(muzzleFlashLight, muzzleFlash.transform);
        rayParticleSystem.Play();
		
		//Shooting logic
		RaycastHit hit;
		if(Physics.SphereCast(cam.transform.position, 0.2f, cam.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, PlayerScript.layerMask))
		{
			if(hit.collider.tag == "Enemy")
			{
				hit.collider.gameObject.GetComponent<HitCollider>().Hit(damageInfo,hit.point);
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