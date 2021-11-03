using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static AudioClip gunshot;
    public static AudioClip thunder;
    public static AudioClip death;
    public static AudioClip basement;
    public static AudioClip door;
    public static AudioClip reload;
    public static AudioClip main;
    static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        gunshot = Resources.Load<AudioClip>("gunshot");
        thunder = Resources.Load<AudioClip>("thunder");
        death = Resources.Load<AudioClip>("death");
        door = Resources.Load<AudioClip>("door");
        basement = Resources.Load<AudioClip>("basement");
        reload = Resources.Load<AudioClip>("reload");
        main = Resources.Load<AudioClip>("main");


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
            case "thunder":
                audioSrc.PlayOneShot(thunder);
                break;

            case "death":
                audioSrc.PlayOneShot(death);
                break;
            case "door":
                audioSrc.PlayOneShot(door);
                break;
            case "basement":
                //audioSrc.Stop();
                audioSrc.PlayOneShot(basement);
                break;
            case "reload":
                audioSrc.PlayOneShot(reload);
                break;
            case "main":
                audioSrc.PlayOneShot(main);
                break;
        }
    }
}
