using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBullet : MonoBehaviour
{
    public GameObject hitEffect;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("kena");
        if (other.tag == "Player")
        {
            Debug.Log("kena player");
            other.GetComponent<Player>().TakeDamage(250);
        }
        else
        {
            if (other.name.ToLower().Contains("floor") || other.name.ToLower().Contains("ground"))
            {
                GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
                effect.GetComponent<ParticleSystem>().Play();
                Destroy(effect, 6f);

            }
        }
    }
}
