using Assets.Scripts.Weapons;
using System.Collections;
using UnityEngine;
using UnityEngine.ProBuilder;


public class ShotgunBasic : BaseWeapon
{
    [Header("Weapon Specific")]
    public float spread;
    public GameObject bulletPrefab;
    public Transform shootPoint;

    private void Update()
    {
        WeaponUpdate();

        if (Input.GetMouseButtonDown(0) && Time.time - lastShotTime > 1/firerate && !PlayerScript.isInteracting && !PlayerScript.isReloading && Time.timeScale != 0f)
        {
            if (PlayerScript.ammoCounts[weaponIndex] > 0) { Shoot(); } else { Reload(); }
        }

        if (Input.GetKeyDown("r") && !PlayerScript.isReloading)
        {
            Reload();
        }
    }

    void Shoot()
    {
        lastShotTime = Time.time;
        anim.SetBool("Shoot", true);
        muzzleFlash.Play();
        Shake(0.15f, 0.5f);
        PlayerScript.ammoCounts[weaponIndex] -= 1;

        //Shooting logic
        Ray[] rays = new Ray[]
        {
			//Middle row
			new Ray(cam.transform.position, cam.transform.position + (cam.transform.forward*1f + transform.right*-0.25f * spread).normalized - cam.transform.position),
            new Ray(cam.transform.position, cam.transform.position + (cam.transform.forward*1f).normalized - cam.transform.position),
            new Ray(cam.transform.position, cam.transform.position + (cam.transform.forward*1f + transform.right*0.25f * spread).normalized - cam.transform.position),
			//Top row
            new Ray(cam.transform.position, cam.transform.position + (cam.transform.forward*1f + transform.right * -0.25f * spread + transform.up*-0.25f * spread).normalized - cam.transform.position),
            new Ray(cam.transform.position, cam.transform.position + (cam.transform.forward*1f + transform.up * -0.25f * spread).normalized - cam.transform.position),
            new Ray(cam.transform.position, cam.transform.position + (cam.transform.forward*1f + transform.right * 0.25f * spread + transform.up * -0.25f * spread).normalized - cam.transform.position),
			//Bottom row
            new Ray(cam.transform.position, cam.transform.position + (cam.transform.forward*1f + transform.right * -0.25f * spread + transform.up * 0.25f * spread).normalized - cam.transform.position),
            new Ray(cam.transform.position, cam.transform.position + (cam.transform.forward*1f + transform.up * 0.25f * spread).normalized - cam.transform.position),
            new Ray(cam.transform.position, cam.transform.position + (cam.transform.forward*1f + transform.right * 0.25f * spread + transform.up * 0.25f * spread).normalized - cam.transform.position)
        };

        foreach (Ray ray in rays)
        {
            //Instantiate a bullet for each ray and set its damage
            var bullet = Instantiate(bulletPrefab, shootPoint.transform.position, Quaternion.LookRotation(ray.direction)).GetComponent<ShotgunBulletController>();
            bullet.damage = damage;
            bullet.piercing = piercing;
        }
    }

    void Reload()
    {
        //Tells the Animator to play the reloading animation
        anim.SetBool("Reload", true);
        PlayerScript.isReloading = true;
    }

    public void ChangeAmmo(int amount)
    {
        if(PlayerScript.ammoCounts[weaponIndex] + amount <= maxAmmo)
        {
            PlayerScript.ammoCounts[weaponIndex] += amount;
        }
    }
}