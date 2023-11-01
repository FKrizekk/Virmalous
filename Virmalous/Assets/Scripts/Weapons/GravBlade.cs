using System.Collections;
using UnityEngine;

public class GravBlade : MonoBehaviour
{
    public float speed = 1.0f;

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.TransformDirection(new Vector3(0,0,1)) * Time.deltaTime * speed;
    }
}