using JetBrains.Annotations;
using OpenCover.Framework.Model;
using System.Collections;
using System.ComponentModel;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

[System.Serializable]
public class SoundClip
{
    public AudioClip clip;
    public float volumeMultiplier = 1;
}

[System.Serializable]
public class SwayConfig
{
    [Header("Sway")]
    public float step = 0.01f;
    public float maxStepDistance = 0.06f;
    public Vector3 swayPos;

    [Header("SwayRotation")]
    public float rotationStep = 4f;
    public float maxRotationStep = 5f;
    public Vector3 swayEulerRot;

    [Header("Bobbing")]
    public float speedCurve;
    public float curveSin { get => Mathf.Sin(speedCurve); }
    public float curveCos { get => Mathf.Cos(speedCurve); }
    public Vector3 travelLimit = Vector3.one * 0.025f;
    public Vector3 bobLimit = Vector3.one * 0.01f;
    public Vector3 bobPosition;

    [Header("Bob Rotation")]
    public Vector3 multiplier;
    public Vector3 bobEulerRotation;
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

    [Space]
    [Header("Config")]
    [Tooltip("The weapons index (It's in the google doc)")]
    public int weaponIndex;
    [Tooltip("The target relative position to the camera")]
    public Vector3 relativePos;

    public SwayConfig swayConfig;

    [Space]
    [Header("Refferences")]
    protected TMP_Text ammoCountText;
    public GameObject bulletHitPrefab;
    public VisualEffect muzzleFlash;
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

    [Space]
    public float fovModifier = 0f;
    GameManager game;

    void Awake()
    {
        //Get all the references
        source = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        player = GameObject.Find("Player");
        cam = PlayerScript.cam;
        rb = player.GetComponent<Rigidbody>();
        ammoCountText = GameObject.Find("WeaponInfo/AmmoCount").GetComponent<TMP_Text>();
        game = GameObject.Find("Level").GetComponent<GameManager>();
    }

    protected void WeaponUpdate()
    {
        PlayerMovement.fovModifier[0] = fovModifier;
        ammoCountText.text = PlayerScript.ammoCounts[weaponIndex].ToString() + "/" + maxAmmo;
        SwayBobRotate();
    }

    void SwayBobRotate()
    {
        Vector2 walkInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 lookInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        Sway(lookInput);
        SwayRotation(lookInput);
        BobOffset(walkInput);
        BobRotation(walkInput);

        CompositePositionRotation();
    }

    void Sway(Vector2 lookInput)
    {
        Vector3 invertLook = lookInput * -swayConfig.step;
        invertLook.x = Mathf.Clamp(invertLook.x, -swayConfig.maxStepDistance, swayConfig.maxStepDistance);
        invertLook.y = Mathf.Clamp(invertLook.y, -swayConfig.maxStepDistance, -swayConfig.maxStepDistance);

        swayConfig.swayPos = invertLook;
    }

    void SwayRotation(Vector2 lookInput)
    {
        Vector2 invertLook = lookInput * -swayConfig.rotationStep;
        invertLook.x = Mathf.Clamp(invertLook.x, -swayConfig.maxRotationStep, swayConfig.maxRotationStep);
        invertLook.y = Mathf.Clamp(invertLook.y, -swayConfig.maxRotationStep, swayConfig.maxRotationStep);

        swayConfig.swayEulerRot = new Vector3(-invertLook.y, invertLook.x, invertLook.x);
    }

    void BobOffset(Vector2 walkInput)
    {
        swayConfig.speedCurve += Time.deltaTime * (PlayerMovement.grounded ? rb.velocity.magnitude : 1f) + 0.01f;

        //bob input offset
        swayConfig.bobPosition.x = (swayConfig.curveCos * swayConfig.bobLimit.x * (PlayerMovement.grounded ? 1 : 0)) - (walkInput.x * swayConfig.travelLimit.x);

        //bob y velocity offset
        swayConfig.bobPosition.y = (swayConfig.curveSin * swayConfig.bobLimit.y) - (rb.velocity.y * swayConfig.travelLimit.y);

        //input offset
        swayConfig.bobPosition.z = -(walkInput.y * swayConfig.travelLimit.z);
    }

    void BobRotation(Vector2 walkInput)
    {
        //Pitch
        swayConfig.bobEulerRotation.x = (walkInput != Vector2.zero ? swayConfig.multiplier.x * (Mathf.Sin(2 * swayConfig.speedCurve)) : swayConfig.multiplier.x * (Mathf.Sin(2 * swayConfig.speedCurve) / 2));

        //Yaw
        swayConfig.bobEulerRotation.y = (walkInput != Vector2.zero ? swayConfig.multiplier.y * swayConfig.curveCos : 0);
        swayConfig.bobEulerRotation.z = (walkInput != Vector2.zero ? swayConfig.multiplier.z * swayConfig.curveCos * walkInput.x : 0);
    }

    void CompositePositionRotation()
    {
        float smooth = 10f;
        float smoothRot = 12f;

        //Position
        transform.localPosition = Vector3.Lerp(transform.localPosition, relativePos + swayConfig.swayPos + swayConfig.bobPosition, Time.deltaTime * smooth);
        //Rotation
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(swayConfig.swayEulerRot) * Quaternion.Euler(swayConfig.bobEulerRotation), Time.deltaTime * smoothRot);
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