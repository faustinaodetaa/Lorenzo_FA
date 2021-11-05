using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject cannonBullet;
    public Transform cannonShootSpot;
    private float startTime, timeElapsed;

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        timeElapsed = Time.time - startTime;
        if (timeElapsed >= 3)
        {
            shootCannon();
            startTime = Time.time;
        }
    }

    void shootCannon()
    {
        GameObject o = Instantiate(cannonBullet, cannonShootSpot.position, Quaternion.identity);
        o.transform.rotation = cannonShootSpot.rotation;
        Rigidbody rb = o.GetComponent<Rigidbody>();
        rb.AddForce(o.transform.forward * 1000);
    }

}
