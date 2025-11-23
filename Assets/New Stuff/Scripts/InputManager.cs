using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    InputSystem_Actions playerControls;
    PlayerLocomotion playerLocomotion;

    public Vector2 movementInput;
    public Vector2 cameraInput;


    public float cameraInputX;
    public float cameraInputY;
    public float verticalInput;
    public float horizontalInput;

    public bool jump_Input = false;

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new InputSystem_Actions();
            playerLocomotion = GetComponent<PlayerLocomotion>();

            playerControls.Player.Move.performed += 
                i => movementInput = i.ReadValue<Vector2>();
            
            playerControls.Player.Look.performed += 
                i => cameraInput = i.ReadValue<Vector2>();
            
            //playerControls.Player.Jump.performed += i => jump_Input = true;

            playerControls.Player.Jump.started += onJump;
            playerControls.Player.Jump.canceled += onJump;

        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        //HandleJumpingInput();
    }

    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        cameraInputX = cameraInput.x;
        cameraInputY = cameraInput.y;


    }

    /*private void HandleJumpingInput()
    {
        playerLocomotion.HandleJumping(true);
        
    }*/

    void onJump (InputAction.CallbackContext context)
    {
        jump_Input = context.ReadValueAsButton();
        Debug.Log(jump_Input);
        playerLocomotion.HandleJumping(jump_Input);
    }
}

