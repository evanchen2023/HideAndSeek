using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PM2 : MonoBehaviour
{
    [SerializeField]
    public float maximumSpeed;

    [SerializeField]
    public float rotationSpeed;

    [SerializeField]
    public float jumpSpeed;

    [SerializeField]
    public float jumpButtonGracePeriod;

    [SerializeField]
    private Transform cameraTransform;

    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource footstepSound;
    [SerializeField] private AudioSource runSound;
    private bool isRunning = false;
    private bool isJumping = false;

    private Animator animator;
    private CharacterController characterController;
    private float ySpeed;
    private float originalStepOffset;
    private float? lastGroundedTime;
    private float? jumpButtonPressedTime;
    
    //Game Manager
    private GameObject gameManager;
    private WinCondition winCondition;

    // Start is called before the first frame update
    void Start()
    {
        //UI Interaction
        Cursor.lockState = CursorLockMode.Locked;
        gameManager = GameObject.FindWithTag("Manager");
        winCondition = gameManager.GetComponent<WinCondition>(); //Script with Timer Check Function

        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        originalStepOffset = characterController.stepOffset;
    }

    // Update is called once per frame
    void Update()
    {
        if (winCondition.GetControlsActive()) //Can Only Move While Timer is Running
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            bool isMoving = Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f;

            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                isRunning = true;
            }
            else
            {
                isRunning = false;
            }
            if (characterController.isGrounded && Input.GetButtonDown("Jump"))
            {
                isJumping = true;
            }
            else if (characterController.isGrounded)
            {
                isJumping = false;
            }
            if (isMoving && !isRunning && !isJumping && !footstepSound.isPlaying)
            {
                footstepSound.Play();
            }
            else if ((!isMoving || isRunning || isJumping) && footstepSound.isPlaying)
            {
                footstepSound.Stop();
            }
            if (isRunning && !isJumping && !runSound.isPlaying)
            {
                runSound.Play();
            }
            else if ((!isRunning || isJumping) && runSound.isPlaying)
            {
                runSound.Stop();
            }


            Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
            float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);
            inputMagnitude /= 2;

            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);
            }
            
            animator.SetFloat("Input Magnitude", inputMagnitude, 0.05f, Time.deltaTime);

            float speed = inputMagnitude * maximumSpeed;
            movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) *
                                movementDirection;
            movementDirection.Normalize();

            ySpeed += Physics.gravity.y * Time.deltaTime;

            if (characterController.isGrounded)
            {
                lastGroundedTime = Time.time;
            }

            if (Input.GetButtonDown("Jump"))
            {
                jumpButtonPressedTime = Time.time;
                animator.SetBool("IsJump", true);
                jumpSound.Play();
            }
            else
            {
                animator.SetBool("IsJump", false);
            }

            if (Time.time - lastGroundedTime <= jumpButtonGracePeriod)
            {
                characterController.stepOffset = originalStepOffset;
                ySpeed = -0.5f;

                if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod)
                {
                    ySpeed = jumpSpeed;
                    jumpButtonPressedTime = null;
                    lastGroundedTime = null;
                }
            }
            else
            {
                characterController.stepOffset = 0;
            }

            Vector3 velocity = movementDirection * speed;
            velocity.y = ySpeed;

            characterController.Move(velocity * Time.deltaTime);

            if (movementDirection != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

                transform.rotation =
                    Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }
        }
        else
        {
            animator.SetFloat("Input Magnitude", 0, 0.05f, Time.deltaTime); //Reset Animations
        }
    }


    //this one will hide the cursor when we move the mouse:
    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}