using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSplashPS : MonoBehaviour
{
    ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;

    [SerializeField] private GameObject bloodDecal;

    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

        int i = 0;

        while (i < numCollisionEvents)
        {
            Vector3 pos = collisionEvents[i].intersection;

            Vector3 spawnPos = pos + collisionEvents[i].normal * 0.01f;
            Quaternion spawnRot = Quaternion.LookRotation(collisionEvents[i].normal);

            Instantiate(bloodDecal, spawnPos, spawnRot, null);
            
            i++;
        }
    }
}
