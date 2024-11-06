using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [HideInInspector] public Vector2 moveInput;
    [HideInInspector] public bool sprintInput;
    [HideInInspector] public bool jumpInput;
    [HideInInspector] public bool crouchInput;
    [HideInInspector] public bool rightHandInput;
    [HideInInspector] public bool leftHandInput;


    [HideInInspector] public bool canRun = true;

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
        if(value > 0.1f && canRun)
        {
            sprintInput = true;
        }
        else
        {
            sprintInput = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
        jumpInput = value > 0.1f;
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
        crouchInput = value > 0.1f;
    }

    public void OnRightHand(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
        rightHandInput = value > 0.9f;
    }

    public void OnLeftHand(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
        leftHandInput = value > 0.9f;
    }
}
