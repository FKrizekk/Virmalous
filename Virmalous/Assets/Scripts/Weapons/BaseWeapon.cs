using OpenCover.Framework.Model;
using System.Collections;
using System.ComponentModel;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class SoundClip
{
    public AudioClip clip;
    public float volumeMultiplier = 1;
}

public abstract class BaseWeapon : MonoBehaviour
{
    [Header("Stats")]
    [Tooltip("Base damage per projectile")]
    public int damage;
    public int maxAmmo;
    [Tooltip("Amount of shots per second")]
    public float firerate;
    [Tooltip("How many targets can one shot damage")]
    public int piercing;

    [Header("Config")]
    [Tooltip("The weapons index (It's in the google doc)")]
    public int weaponIndex;
    [Tooltip("The target relative position to the camera")]
    public Vector3 relativePos;
    public float swayAmount;
    public float swaySpeed;


    [Header("References")]
    protected TMP_Text ammoCountText;
    [Tooltip("Point used for calculating the distance between the gun and what's in front of the player")]
    public Transform barrel;
    [Tooltip("All AudioClips for use by the weapon")]
    public SoundClip[] clips;
    protected AudioSource source;
    protected Animator anim;
    protected GameObject player;
    protected GameObject cam;
    protected Rigidbody rb;
    protected float lastShotTime = -100;

    void Awake()
    {
        //Get all the references
        source = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        player = GameObject.Find("Player");
        cam = PlayerScript.cam;
        rb = player.GetComponent<Rigidbody>();
        ammoCountText = GameObject.Find("WeaponInfo/AmmoCount").GetComponent<TMP_Text>();
    }

    protected void WeaponUpdate()
    {
        ammoCountText.text = PlayerScript.ammoCounts[weaponIndex].ToString() + "/" + maxAmmo;
        SwayAndRotate();
    }

    public virtual void SwayAndRotate()
    {
        //Rotate gun towards reticle world position	
        RaycastHit hit;
        if (Physics.Raycast(PlayerScript.cam.transform.position, PlayerScript.cam.transform.forward, out hit, Mathf.Infinity, PlayerScript.layerMask))
        {
            // Calculate the direction from the gun's current position to the hit point
            Vector3 directionToTarget = hit.point - barrel.position;

            // Calculate the rotation needed to face the target point
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);

            // Rotate the gun smoothly towards the target point using Slerp (Spherical Linear Interpolation)
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10);
        }

        float distanceToPoint = Vector3.Distance(hit.point, PlayerScript.cam.transform.position);

        //Sway gun
        Vector3 swayDirection;
        // Calculate the opposite direction of player's velocity
        if (Mathf.Abs(rb.velocity.x) > 1 || Mathf.Abs(rb.velocity.y) > 1 || Mathf.Abs(rb.velocity.z) > 1)
        {
            swayDirection = player.transform.InverseTransformDirection(-rb.velocity.normalized);
        }
        else
        {
            swayDirection = Vector3.zero;
        }

        float distanceThreshold = 1.5f;
        // Calculate the target position based on the sway direction and the distance to the hit.point
        Vector3 targetPosition = relativePos + (swayDirection * swayAmount) + (distanceToPoint < distanceThreshold ? -Vector3.forward * (distanceThreshold - distanceToPoint) : Vector3.zero);

        // Smoothly move the gun towards the target position
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, swaySpeed * Time.deltaTime);



    }

    public void PlaySound(int index)
    {
        source.PlayOneShot(clips[index].clip, clips[index].volumeMultiplier);
    }

    public void SetPitch(float pitch)
    {
        source.pitch = pitch;
    }

    public void Shake(float magnitude)
    {
        PlayerScript.cameraShake.Shake(magnitude, magnitude);
        PlayerScript.uIShake.Shake(magnitude, magnitude);
    }
}