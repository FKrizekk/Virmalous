using System.Collections;
using UnityEngine;

public class GravBlade : MonoBehaviour
{
    public float speed = 1.0f;
    public int damage = 50;
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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag != "Player") { moving = false; col.enabled = false; transform.parent = collision.transform; }

        if(collision.gameObject.tag == "Enemy") { collision.collider.gameObject.GetComponent<HitCollider>().Hit(damage, collision.contacts[0].point); }
    }
}