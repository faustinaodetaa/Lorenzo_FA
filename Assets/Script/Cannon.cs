using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{

    //private Animator animator;
    //private bool cannonIsLoaded = false;
    //private Transform cannonBallSpawn = null;
    //public GameObject CannonBall;
    ////private ParticleSystem PS_Smoke;
    //public float Power = 12.0f;


    //// Start is called before the first frame update
    //void Start()
    //{
    //    animator = GetComponent<Animator>();
    //    cannonBallSpawn = transform.Find("CannonBallSpawn");
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    //ShootCannonBall();
    //}

    //private void ShootCannonBall()
    //{
    //    GameObject cannonBall = Instantiate(CannonBall, cannonBallSpawn.position, Quaternion.identity);
    //    Rigidbody rb = cannonBall.AddComponent<Rigidbody>();

    //    rb.velocity = Power * cannonBallSpawn.forward;

    //    StartCoroutine(RemoveCannonBall(rb, 3.0f));

    //    animator.SetTrigger("tr_shoot");
    //}

    //IEnumerator RemoveCannonBall(Rigidbody rb, float delay)
    //{
    //    yield return new WaitForSeconds(delay);

    //    Destroy(rb);
    //}


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
