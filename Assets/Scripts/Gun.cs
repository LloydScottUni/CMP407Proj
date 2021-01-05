using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bullet;
    public AK.Wwise.Event gunSound = new AK.Wwise.Event();

    // Update is called once per frame
    void Update()
    {
        //Fire bullet from gun, trigger gun sound
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Instantiate(bullet, transform.position, transform.rotation);
            gunSound.Post(gameObject);
        }
    }
}
