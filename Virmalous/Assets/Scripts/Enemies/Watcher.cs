using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Watcher : BaseEnemy
{
    bool inCombat = false;
    bool shootDone = false;

    [Header("Watcher specific")]
    public float lookSpeed = 1f;
    public float chargeTime = 1f;
    public float laserTime = 1f;

    private Transform body;

    private void Start()
    {
        body = transform.GetChild(0);
    }

    private void Update()
    {
        //Move to player if in sight
        if (playerInSight) { nav.SetDestination(player.transform.position); }
        if(nav.remainingDistance - nav.stoppingDistance < 1 && !inCombat) { inCombat = true; StartCoroutine(Combat()); }

        EnemyUpdate();
    }

    private void FixedUpdate()
    {
        //Slowly rotate towards player
        body.rotation = Quaternion.Lerp(body.rotation, Quaternion.LookRotation(Camera.main.transform.position - body.position) * Quaternion.Euler(-90f, 90f, 0) , lookSpeed);
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
        nav.speed = 0f;
        shootDone = false;
        StartCoroutine(ChargeLaser());
        yield return new WaitUntil(() => shootDone);
        nav.speed = speed;
        inCombat = false;
    }

    IEnumerator ChargeLaser()
    {
        VFXParents[4].SetActive(true);
        yield return new WaitForSeconds(chargeTime);
        VFXParents[4].SetActive(false);
        VFXParents[5].SetActive(true);
        yield return new WaitForSeconds(laserTime);
        VFXParents[5].SetActive(false);
        shootDone = true;
    }
}
