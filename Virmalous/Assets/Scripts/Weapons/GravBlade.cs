using System.Collections;
using UnityEngine;

public class GravBlade : MonoBehaviour
{
    public DamageInfo damageInfo;
    Rigidbody rb;
    BoxCollider col;
    Vector3 finalPos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if (rb.velocity.magnitude > 1)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 2f))
            {
                finalPos = hit.point;
            }
            Debug.DrawRay(transform.position, hit.point - transform.position, Color.white, 5f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag != "Player") { col.enabled = false; transform.parent = other.transform; rb.constraints = RigidbodyConstraints.FreezeAll; transform.position = finalPos; }

        if(other.gameObject.tag == "Enemy") { other.gameObject.GetComponent<HitCollider>().Hit(damageInfo, other.ClosestPoint(transform.position)); }
    }
}