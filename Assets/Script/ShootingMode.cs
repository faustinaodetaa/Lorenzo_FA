using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingMode : MonoBehaviour
{

    public static bool Shooting = false;

    public GameObject ShootingCam, MainCam;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (Shooting)
            {
                MainCam.SetActive(true);
                ShootingCam.SetActive(false);
                Shooting = false;
            }
            else
            {
                MainCam.SetActive(false);
                ShootingCam.SetActive(true);
                Shooting = true;
            }
        }
    }
}
