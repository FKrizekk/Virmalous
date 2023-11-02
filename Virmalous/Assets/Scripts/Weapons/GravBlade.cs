using System.Collections;
using UnityEngine;

public class GravBlade : MonoBehaviour
{
    public float speed = 1.0f;
    public DamageInfo damageInfo;
    bool moving = true;
    Rigidbody rb;
    BoxCollider col;

    // Update is called once per frame
    void Update()
    {
        rb.velocity = moving ? transform.TransformDirection(Vector3.forward) * speed : Vector3.zero;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = rb.GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag != "Player") { moving = false; col.enabled = false; transform.parent = other.transform; transform.localPosition -= transform.TransformDirection(Vector3.forward * 2); }

        if(other.gameObject.tag == "Enemy") { other.gameObject.GetComponent<HitCollider>().Hit(damageInfo, other.ClosestPoint(transform.position)); }
    }
}