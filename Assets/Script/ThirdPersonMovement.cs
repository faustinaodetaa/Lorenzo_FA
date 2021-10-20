using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ThirdPersonMovement : PlayerController
{
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    Animator animator;
    RaycastWeapon weapon;
    public float rotationSpeed = 3f;
    public float aimDuration = 0.3f;
    public Rig aimLayer;
    public Transform cameraLookAt;
    public Cinemachine.AxisState xAxis;
    public Cinemachine.AxisState yAxis;
    int isAimingParam = Animator.StringToHash("isAiming");

    protected override void Start()
    {
        controller = GetComponent<CharacterController>();
        velocity.y = 0;
        animator = GetComponent<Animator>();
        weapon = GetComponentInChildren < RaycastWeapon>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    protected override void Update()
    {

        var horizontal = Input.GetAxisRaw("Horizontal");   
        var vertical = Input.GetAxisRaw("Vertical");       
        //var direction = Vector3.forward * vertical + Vector3.right * horizontal;
        var direction = new Vector3(horizontal, 0f, vertical).normalized;

        xAxis.Update(Time.fixedDeltaTime);
        yAxis.Update(Time.fixedDeltaTime);

        cameraLookAt.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0);

        bool isAiming = Input.GetMouseButton(1);
        animator.SetBool(isAimingParam, isAiming);

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

        if (Input.GetKeyDown(KeyCode.C))
        {
            aimLayer.weight += Time.deltaTime / aimDuration;
        }
        else
        {
            aimLayer.weight -= Time.deltaTime / aimDuration;
        }

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
