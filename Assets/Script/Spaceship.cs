using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spaceship : MonoBehaviour
{

    public GameObject player;
    public Text messageDisplay;
    public string message;
    public Animator animator, planeAnimator;

    public GameObject dialogueCamera;
    public GameObject mainCamera;
    public GameObject shoulderCamera;
    public GameObject planeCamera;
    public Text stopwatchTxt;
    public GameObject playerHUD;

    public static bool shoulderMode = false;

    public GameObject victoryMenu;

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Vector3.Distance(player.transform.position, transform.position));
        if (Vector3.Distance(player.transform.position, transform.position) <= 7f)
        {
            Debug.Log("masuk spaceship");
            animator.SetBool("isOpen", true);
            message = "Press F";
            if (Input.GetKeyDown(KeyCode.H))
            {
                Debug.Log("terbang");
                mainCamera.SetActive(false);
                player.SetActive(false);
                stopwatchTxt.text = Stopwatch.lastTime;
                playerHUD.SetActive(false);
                planeCamera.SetActive(true);
                victoryMenu.SetActive(true);
                
                SoundManager.PlaySound("victory");
                //SoundManager.Destroy("main");
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                //Time.timeScale = 0f;

                planeAnimator.SetBool("isFlying", true);
            }
            messageDisplay.text = message;
        }
        else
        {
            
            //overrideText.gameObject.SetActive(false);
        }
    }
}
