using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerLocomotion : MonoBehaviour
{
    InputManager inputManager;
    Vector3 moveDirection;
    Transform cameraObject;
    Rigidbody playerRigidbody;

    [Header("Falling")]
    public float inAirTimer;
    public float leapingVelocity;
    public float fallingVelocity;
    public LayerMask groundLayer;
    public float rayCastHeightOffSet = 0.5f;

    [Header("Movement Flags")]
    public bool isGrounded;
    public bool isJumping = false;

    [Header("Movement Speeds")]
    public float movementSpeed = 5;
    public float rotationSpeed = 15;
    public Vector3 movementVelocity;

    [Header("Jump Speeds")]
    public float maxJumpHeight = 3f;
    float initialJumpVelocity = 50f;
    public float maxJumpTime = 0.5f;
    public float fallMultiplier = 2f;
    //public float gravityIntensity = -9.8f;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerRigidbody = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
        calculateJumpVariables();
    }

    void calculateJumpVariables()
    {
        float timeToApex = maxJumpTime / 2f;
        //gravityIntensity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }

    public void HandleAllMovement()
    {
        HandleFallingAndLanding();
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        moveDirection = cameraObject.forward * inputManager.verticalInput;
        moveDirection = moveDirection + cameraObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;

        moveDirection = moveDirection * movementSpeed;

        movementVelocity = moveDirection;
        movementVelocity.y = playerRigidbody.velocity.y;
        playerRigidbody.velocity = movementVelocity;
    }

    private void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;

        targetDirection = cameraObject.forward * inputManager.verticalInput;
        targetDirection = targetDirection + cameraObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if(targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        
        transform.rotation = playerRotation;
    }

    private void HandleFallingAndLanding()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        rayCastOrigin.y = rayCastOrigin.y + rayCastHeightOffSet;
        if(playerRigidbody.velocity.y < 0f)
        {
            isJumping = false;
        }

        if (!isGrounded)
        {
            float newFallingVelocity;
            if(!isJumping)
            {
                newFallingVelocity = fallingVelocity * fallMultiplier;
            }else
            {
                newFallingVelocity = fallingVelocity;
            }
            inAirTimer = inAirTimer + Time.deltaTime;
            playerRigidbody.AddForce(transform.forward * leapingVelocity);
            playerRigidbody.AddForce(Vector3.down * newFallingVelocity * inAirTimer);
        }

        if (Physics.SphereCast(rayCastOrigin, 0.1f, Vector3.down, out hit, groundLayer))
        {
            if(!isGrounded)
            {
                inAirTimer = 0;
                isGrounded = true;
                if(isJumping)
                {
                    isJumping = false;
                }
            }
        }else
        {
            isGrounded = false;
        }

    }

    public void HandleJumping(bool isJumpPressed)
    {
        if(isGrounded && !isJumping && isJumpPressed)
        {
            isJumping = true;
            Debug.Log("Jump");
            movementVelocity.y = initialJumpVelocity;
            playerRigidbody.linearVelocity = movementVelocity;
            /*float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * maxJumpHeight);
            Vector3 playerVelocity = moveDirection;
            playerVelocity.y = jumpingVelocity;
            playerRigidbody.linearVelocity = playerVelocity;*/
            
            
        }if(!isJumpPressed && isJumping)
        {
            isJumping = false;
        }

        
    }
}
