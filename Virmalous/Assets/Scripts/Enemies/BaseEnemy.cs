using Assets.Scripts;
using JetBrains.Annotations;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

public abstract class BaseEnemy : Entity
{
    [Header("Stats")]
    public int maxHealth;
    protected int health;
    [Tooltip("Damage per attack for melee or Damage per projectile for ranged")]
    public DamageInfo damage;
    [Tooltip("Attacks per second")]
    public float attackRate;

    [Header("NavConfig")]
    public float speed;
    public float angularSpeed;
    public float acceleration;
    public float stoppingDistance;
    public bool autoBraking = true;

    [Header("References")]
    [Tooltip("AudioSource for playing sounds from the clips array")]
    public AudioSource source;
    [Tooltip("The Enemy's Animator")]
    public GameObject blood;
    [Tooltip("Array for any sound to be played by the source")]
    public SoundClip[] clips;
    [Tooltip("Array for hit sounds, played when this Entity is hit.")]
    public SoundClip[] HitClips;
    [Tooltip("Ragdoll GameObject to be spawned when enemy dies")]
    public GameObject ragdoll;
    [Tooltip("Blood ParticleSystem GameObject")]
    public Animator anim;
    

    protected float lastAttackTime = 0;
    protected float lastStatusUpdateTime = 0;
    protected float statusUpdateTick = 1f; //How many times takes damage per second based on status effects
    protected Vector3 killPoint;
    protected NavMeshAgent nav;
    protected GameObject player;
    protected int layerMask;
    protected Rigidbody rb;

    protected bool playerInSight = false;

    void Awake()
    {
        health = maxHealth;

        //Get references
        nav = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");

        //Set layermask for raycasting
        layerMask = 1 << 6 | 1 << 2;
        layerMask = ~layerMask;

        //Set nav config
        nav.speed = speed;
        nav.angularSpeed = angularSpeed;
        nav.acceleration = acceleration;
        nav.stoppingDistance = stoppingDistance;
        nav.autoBraking = autoBraking;
    }

    protected void EnemyUpdate()
    {
        EntityUpdate();

        if (Time.time - lastStatusUpdateTime >= statusUpdateTick && (entityState.stunned != 0 || entityState.onFire != 0 || entityState.frozen != 0 || entityState.electrified != 0))
        {
            lastStatusUpdateTime = Time.time;

            //EntityState check
            hitInfo stateUpdateInfo = new hitInfo();

            stateUpdateInfo.point = GetComponentInChildren<Renderer>().bounds.center;

            stateUpdateInfo.damageInfo.damage = 0f;
            stateUpdateInfo.damageInfo.stunDamage = entityState.stunned * entityState.stunDamageMultiplier / maxHealth * 2000;
            stateUpdateInfo.damageInfo.fireDamage = entityState.onFire * entityState.fireDamageMultiplier / maxHealth * 2000;
            stateUpdateInfo.damageInfo.freezeDamage = entityState.frozen * entityState.freezeDamageMultiplier / maxHealth * 2000;
            stateUpdateInfo.damageInfo.electricityDamage = entityState.electrified * entityState.electricityDamageMultiplier / maxHealth * 2000;

            GotHit(stateUpdateInfo, true);
        }

        //Check if dead
        if (health <= 0) { Death(); }

        //Check if player is in sight
        RaycastHit hit;
        if (Physics.Raycast(GetComponentInChildren<Renderer>().bounds.center, player.transform.position - GetComponentInChildren<Renderer>().bounds.center, out hit, Mathf.Infinity, layerMask))
        {
            playerInSight = hit.collider.gameObject.tag == "Player";
        }

        //Update nav speed based on how stunned the enemy is
        nav.speed = speed * (1 - entityState.stunned);
    }

    public virtual void GotHit(hitInfo hit)
    {
        //idk if this is the right fix but i am like 99% sure
        GotHit(hit, false);
    }

    public virtual void GotHit(hitInfo hit, bool statusUpdate)
    {
        killPoint = hit.point;
        float amount = hit.damageInfo.damage +
            (hit.damageInfo.fireDamage * entityState.fireDamageMultiplier) +
            (hit.damageInfo.freezeDamage * entityState.freezeDamageMultiplier) +
            (hit.damageInfo.electricityDamage * entityState.electricityDamageMultiplier);
        health -= (int)amount;

        if (!statusUpdate)
        {
            entityState._stunned += hit.damageInfo.stunDamage * entityState.stunDamageMultiplier;
            entityState._onFire += hit.damageInfo.fireDamage * entityState.fireDamageMultiplier / 100f;
            entityState._frozen += hit.damageInfo.freezeDamage * entityState.freezeDamageMultiplier / 100f;
            entityState._electrified += hit.damageInfo.electricityDamage * entityState.electricityDamageMultiplier / 100f;
        }

        Bleed();
    }

    private int currentHitIndex = 0;
    void Bleed()
    {
        //Play hit sound from HitClips array
        source.PlayOneShot(HitClips[currentHitIndex].clip, HitClips[currentHitIndex].volumeMultiplier); currentHitIndex++; if(currentHitIndex >= HitClips.Length) { currentHitIndex = 0; }
        //Spawn bleed effect
        Instantiate(blood, killPoint, killPoint == transform.GetComponentInChildren<Renderer>().bounds.center ? Quaternion.identity : Quaternion.LookRotation(killPoint - transform.GetComponentInChildren<Renderer>().bounds.center));
    }

    protected virtual void Death()
    {
        //Instantiates ragdoll, adds a force away from player to it and destroys the original enemy object
        GameObject ragdollObj = Instantiate(ragdoll, transform.position, Quaternion.identity);
        ragdollObj.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Rigidbody>().AddForce((transform.position - player.transform.position).normalized * 100, ForceMode.VelocityChange);
        Destroy(gameObject);
    }

    protected void PlaySound(int index)
    {
        //Function for playing any AudioClip in the clips array
        source.PlayOneShot(clips[index].clip);
    }
}