using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{

    public Text nameText;
    public Text dialogueText;

    public Animator animator;

    private Queue<string> sentences;

    public GameObject playerHUD;
    //public GameObject dialogueUI;

    public GameObject dialogueCamera;
    public GameObject mainCamera;
    public GameObject shoulderCamera;
    public Player player;



    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        Debug.Log("player restart :" + player.isRestart);
        if (!player.isRestart)
        {
            Debug.Log("no restart");
            //Debug.Log("Starting convo with " + dialogue.name);
            animator.SetBool("IsOpen", true);

            //dialogueUI.SetActive(true);
            Debug.Log("dialogue");
            playerHUD.SetActive(false);
            mainCamera.SetActive(false);
            shoulderCamera.SetActive(false);
            dialogueCamera.SetActive(true);
        

            nameText.text = dialogue.name;

            sentences.Clear();

            foreach(string sentence in dialogue.sentences)
            {
                sentences.Enqueue(sentence);
            }

            DisplayNextSentence();
        }
        else
        {
            Debug.Log("restart");
            dialogueCamera.SetActive(false);
            mainCamera.SetActive(true);
            playerHUD.SetActive(true);
            shoulderCamera.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
        //Debug.Log(sentence);
        //dialogueText.text = sentence;
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        //Debug.Log("End of convo");
        animator.SetBool("IsOpen", false);
        dialogueCamera.SetActive(false);
        mainCamera.SetActive(true);
        playerHUD.SetActive(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

   
}
