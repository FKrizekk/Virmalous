﻿using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Weapons
{
    public class ShotgunBulletController : MonoBehaviour
    {
        public float bulletSpeed;
        public Rigidbody rb;
        public int damage;
        public int piercing;

        int targetsHit = 0;

        private void Start()
        {
            Invoke("Die", 20);
        }

        private void FixedUpdate()
        {
            rb.AddRelativeForce(Vector3.forward * bulletSpeed, ForceMode.VelocityChange);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Enemy")
            {
                other.gameObject.GetComponent<HitCollider>().Hit(damage, transform.position);
                targetsHit++;
            }

            if(other.gameObject.tag != "Bullet" && targetsHit >= piercing) { Die(); }
        }

        void Die()
        {
            Destroy(gameObject);
        }
    }
}