using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Weapons
{
    public class ShotgunBulletController : MonoBehaviour
    {
        public float bulletSpeed;
        public Rigidbody rb;
        public DamageInfo damageInfo;
        public int piercing;
        public GameObject bulletHitPrefab;

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
            if(other.gameObject.tag == "Enemy" && targetsHit < piercing)
            {
                other.gameObject.GetComponent<HitCollider>().Hit(damageInfo, transform.position);
                targetsHit++;
            }
            else if ((other.gameObject.tag != "Bullet" && targetsHit >= piercing) || ((other.gameObject.tag != "Player" && targetsHit >= piercing)))
            {
                Die();
            }
        }

        void Die()
        {
            Destroy(gameObject);
        }
    }
}