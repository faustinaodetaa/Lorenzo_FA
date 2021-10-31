using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static AudioClip gunshot;
    static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        gunshot = Resources.Load<AudioClip>("gunshot");

        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "gunshot":
                audioSrc.PlayOneShot(gunshot);
                break;
        }
    }
}
