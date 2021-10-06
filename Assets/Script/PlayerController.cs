using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform cam;
    public float speed = 6f;
    public Vector3 velocity;
    protected CharacterController controller;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
        }
        velocity.y -= 9.8f * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
