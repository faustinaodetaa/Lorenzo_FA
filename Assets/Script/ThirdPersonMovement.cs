using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations.Rigging;

public class ThirdPersonMovement : PlayerController
{
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    public Animator animator, planeAnimator;
    RaycastWeapon weapon;
    public float rotationSpeed = 3f;
    public float aimDuration = 0.3f;
    public Rig aimLayer;
    public Transform cameraLookAt;
    public Cinemachine.AxisState xAxis;
    public Cinemachine.AxisState yAxis;
    int isAimingParam = Animator.StringToHash("isAiming");
    public static bool ShootingMode = false;
    public Text stopwatchTxt;

    public GameObject playerHUD;
    //public GameObject dialogueUI;

    public GameObject dialogueCamera;
    public GameObject mainCamera;
    public GameObject shoulderCamera;
    public GameObject planeCamera;
    public GameObject player;

    public static bool shoulderMode = false;

    public GameObject victoryMenu;

    DialogueTrigger dt;

    protected override void Start()
    {
        controller = GetComponent<CharacterController>();
        velocity.y = 0;
        animator = GetComponent<Animator>();
        weapon = GetComponentInChildren < RaycastWeapon>();
        //dt.TriggerDialogue();

        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
        playerHUD.SetActive(false);
        //dialogueCamera.SetActive(true);
        //mainCamera.SetActive(false);
    }

    // Update is called once per frame
    protected override void Update()
    {

        var horizontal = Input.GetAxisRaw("Horizontal");   
        var vertical = Input.GetAxisRaw("Vertical");       
        //var direction = Vector3.forward * vertical + Vector3.right * horizontal;
        var direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (!PauseMenu.GameIsPaused)
        {
            xAxis.Update(Time.fixedDeltaTime);
            yAxis.Update(Time.fixedDeltaTime);

            cameraLookAt.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0);

        }

        bool isAiming = Input.GetMouseButton(1);
        animator.SetBool(isAimingParam, isAiming);

        //if (ShootingMode)
        //{
        //    var mouseX = Input.GetAxis("Mouse X");
        //    transform.Rotate(new Vector3(0, mouseX, 0));
        //}

            if (direction.magnitude >= 0.1f)
        {

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            if (!ShootingMode)
            {
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                //var mouseX = Input.GetAxis("Mouse X");
                //transform.Rotate(new Vector3(0, mouseX, 0));
            }


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
            if (ShootingMode)
            {
                //mainCam.SetActive(true);
                //shootingCam.SetActive(false);
                ShootingMode = false;
                StartCoroutine(Idle());
            }
            else
            {
                //mainCam.SetActive(false);
                //shootingCam.SetActive(true);
                ShootingMode = true;
                //var mouseX = Input.GetAxis("Mouse X");
                //transform.Rotate(new Vector3(0, mouseX, 0));
                StartCoroutine(Shoot());
            }
        }

        IEnumerator Shoot()
        {
            while (aimLayer.weight < 1)
            {
                yield return null;
                aimLayer.weight += Time.deltaTime / aimDuration;
            }
            yield return null;
        }

        IEnumerator Idle()
        {
            while (aimLayer.weight > 0)
            {
                yield return null;
                aimLayer.weight -= Time.deltaTime / aimDuration;
            }
            yield return null;

        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            //Debug.Log("tab");
            if (shoulderMode)
            {
                mainCamera.SetActive(true);
                shoulderCamera.SetActive(false);
                shoulderMode = false;
                //Debug.Log("Out shoulder mode");
            }
            else
            {
                mainCamera.SetActive(false);
                shoulderCamera.SetActive(true);
                shoulderMode = true;
                //Debug.Log("Shoulder mode");
                
            }
            
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            SoundManager.PlaySound("victory");
            Debug.Log("terbang");
            mainCamera.SetActive(false);
            player.SetActive(false);
            stopwatchTxt.text = Stopwatch.lastTime;
            playerHUD.SetActive(false);
            planeCamera.SetActive(true);
            victoryMenu.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            planeAnimator.SetBool("isFlying", true);
        }

    }


    void LateUpdate()
    {
        cam.transform.position = transform.position + Vector3.back * 5 + Vector3.up;

        if (ShootingMode)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                weapon.StartFiring();
                SoundManager.PlaySound("gunshot");
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


}
