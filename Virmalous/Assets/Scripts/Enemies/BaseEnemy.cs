﻿using Assets.Scripts;
using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseEnemy : Entity
{
    [Header("Stats")]
    public int maxHealth;
    protected int health;
    [Tooltip("Damage per attack for melee or Damage per projectile for ranged")]
    public int damage;
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
    [Tooltip("Ragdoll GameObject to be spawned when enemy dies")]
    public GameObject ragdoll;
    [Tooltip("Blood ParticleSystem GameObject")]
    public Animator anim;

    protected float lastAttackTime = 0;
    protected Vector3 killPoint;
    protected NavMeshAgent nav;
    protected GameObject player;
    protected int layerMask;

    protected bool playerInSight = false;

    void Awake()
    {
        health = maxHealth;

        //Get references
        nav = GetComponent<NavMeshAgent>();
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
        //Check if dead
        if(health <= 0) { Death(); }

        //Check if player is in sight
        RaycastHit hit;
        if (Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, Mathf.Infinity, layerMask))
        {
            playerInSight = hit.collider.gameObject.tag == "Player";
        }

        //Update nav speed based on how stunned the enemy is
        nav.speed = speed * (1 - entityState.stunned);
    }

    public virtual void GotHit(hitInfo hit)
    {
        killPoint = hit.point;
        health -= hit.amount;

        //If receives more than half of max health adds 0.5 stun
        if(hit.amount >= maxHealth / 2) { entityState._stunned += 0.5f; }

        Bleed();
    }

    void Bleed()
    {
        Instantiate(blood, killPoint, Quaternion.identity);
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