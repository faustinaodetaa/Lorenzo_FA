using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : PlayerController
{
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    Animator animator;
    public float rotationSpeed = 3f;
    RaycastWeapon weapon;

    protected override void Start()
    {
        controller = GetComponent<CharacterController>();
        velocity.y = 0;
        animator = GetComponent<Animator>();
        weapon = GetComponentInChildren < RaycastWeapon>();
    }

    // Update is called once per frame
    protected override void Update()
    {

        var horizontal = Input.GetAxisRaw("Horizontal");   
        var vertical = Input.GetAxisRaw("Vertical");       
        //var direction = Vector3.forward * vertical + Vector3.right * horizontal;
        var direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
            animator.SetBool("IsWalking", true);

        }
        else
        {
            animator.SetBool("IsWalking", false);
        }

        base.Update();

    }
    void LateUpdate()
    {
        cam.transform.position = transform.position + Vector3.back * 5 + Vector3.up;


        if (Input.GetButtonDown("Fire1"))
        {
            weapon.StartFiring();
        }
        if (weapon.isFiring)
        {
            weapon.UpdateFiring(Time.deltaTime);
        }
        weapon.UpdateBullets(Time.deltaTime);
        if (Input.GetButtonUp("Fire1"))
        {
            weapon.StopFiring();
        }
    
    }


}
