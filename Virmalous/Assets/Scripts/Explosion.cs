using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float radius = 1f;
    public DamageInfo damageInfo;

    private void Start()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        var players = GameObject.FindGameObjectsWithTag("Player");

        foreach (var enemy in enemies)
        {
            if(Vector3.Distance(transform.position, enemy.transform.position) < radius)
            {
                hitInfo hitInfo = new hitInfo();
                hitInfo.damageInfo = damageInfo;
                try
                {
                    hitInfo.point = enemy.GetComponent<Renderer>().bounds.ClosestPoint(transform.position);
                    enemy.GetComponent<BaseEnemy>().GotHit(hitInfo);
                }
                catch { }
            }
        }
        foreach (var player in players)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < radius)
            {
                try
                {
                    player.GetComponent<PlayerScript>().GotHit(damageInfo);
                }
                catch { }
            }
        }
    }
}