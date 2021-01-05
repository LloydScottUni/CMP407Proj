using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10;

    private void Start()
    {
        Destroy(gameObject, 6);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (speed * Time.deltaTime * transform.forward);
    }
    
    //Kill NPC when bullet hits it.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            Destroy(other.gameObject.gameObject);
        }
        
        Destroy(gameObject, 2);
    }
}
