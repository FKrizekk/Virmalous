using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Watcher : BaseEnemy
{
    bool inCombat = false;

    public float lookSpeed = 1f;
    private Transform body;

    private void Start()
    {
        body = transform.GetChild(0);
    }

    private void Update()
    {
        //Move to player if in sight
        if (playerInSight) { nav.SetDestination(player.transform.position); if ((!inCombat)){ /*StartCoroutine(Combat());*/ } inCombat = true; } else { inCombat = false; }

        EnemyUpdate();
    }

    private void FixedUpdate()
    {
        //Slowly rotate towards player
        body.rotation = Quaternion.Lerp(body.rotation, Quaternion.LookRotation(player.GetComponentInChildren<Renderer>().bounds.center - body.position) * Quaternion.Euler(-90f, 90f, 0) , lookSpeed);
    }

    protected override void Death()
    {
        //Instantiates ragdoll, adds a force away from player to it and destroys the original enemy object
        GameObject ragdollObj = Instantiate(ragdoll, transform.position, Quaternion.identity);
        ragdollObj.transform.GetComponent<Rigidbody>().AddForce((transform.position - player.transform.position).normalized * 30, ForceMode.VelocityChange);
        Destroy(gameObject);
    }

    IEnumerator Combat()
    {
        return null;
    }
}
