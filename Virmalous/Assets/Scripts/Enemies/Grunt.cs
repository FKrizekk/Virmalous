using System.Collections;
using UnityEngine;

public class Grunt : BaseEnemy
{
    void Start()
    {
        StartCoroutine(Screaming());
    }

    void Update()
    {
        //Move to player if in sight
        if (playerInSight) { nav.SetDestination(player.transform.position); }

        anim.SetBool("Walking", nav.hasPath);

        EnemyUpdate();
    }

    void OnCollisionStay(Collision col)
    {
        foreach (ContactPoint contact in col.contacts)
        {
            //Check if can attack and attack
            if (contact.otherCollider.gameObject.tag == "Player" && (Time.time - lastAttackTime) >= 1/attackRate)
            {
                PlayerScript.ChangeHealth(-damage);
                lastAttackTime = Time.time;
            }
        }
    }

    //Scream at random intervals
    IEnumerator Screaming()
    {
        yield return new WaitForSeconds(Random.Range(2.5f, 9f));
        PlaySound(0);
        StartCoroutine(Screaming());
    }
}