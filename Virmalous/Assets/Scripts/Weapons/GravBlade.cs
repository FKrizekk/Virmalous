using System.Collections;
using UnityEngine;

public class GravBlade : MonoBehaviour
{
    public float speed = 1.0f;
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
        if(collision.gameObject.tag != "Player") { moving = false; col.enabled = false; }
    }
}