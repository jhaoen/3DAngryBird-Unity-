using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class glassBehavior : MonoBehaviour
{
    public GameObject BrokenEffect;
    private float BrokenThreshold = 50;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody != null)
        {
            float impactForce = collision.relativeVelocity.magnitude * collision.rigidbody.mass;

            if (impactForce >= BrokenThreshold)
            {
                Broken();
            }
        }
    }

    void Broken()
    {
        if(BrokenEffect != null)
        {
            Instantiate(BrokenEffect, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("Broken Effect at glass bar is null");
        }

        Destroy(gameObject);
    }
}
