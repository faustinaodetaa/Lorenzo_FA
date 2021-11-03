using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public GameObject character;
    private Animator animator;
    private List<Animator> childAnimatorList = new List<Animator>();
    public Player player;
    private bool doorOpen;

    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
        GameObject child = transform.GetChild(0).gameObject;
        for(int i = 0; i < child.transform.childCount; i++)
        {
            childAnimatorList.Add(child.transform.GetChild(i).GetComponent<Animator>());
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(player.coreItems < 9)
        {
            doorOpen = false;
        }else if(player.coreItems >= 9)
        {
            doorOpen = true;
        }

        
            if (Vector3.Distance(character.transform.position, transform.position) <= 1f)
            {

                if (doorOpen)
                {
                    Debug.Log("Core item > 9");
                    SoundManager.PlaySound("basement");

                    SoundManager.PlaySound("door");
                    
                    Debug.Log("ganti lagu n door");

                animator.SetBool("character_nearby", true);
                foreach(Animator a in childAnimatorList)
                {
                    a.SetBool("isOpening", true);
                }
                }
                else
                {
                    player.message = "Core item is not enough...";
                    player.messageDisplay.text = player.message;
                    Debug.Log("Core item < 9");
                    //Invoke(nameof(player.displayMessage), 5);
                }
        }
            else
            {
                animator.SetBool("character_nearby", false);
                foreach (Animator a in childAnimatorList)
                {
                    a.SetBool("isOpening", false);
                }
            }

       
        
    }
}
