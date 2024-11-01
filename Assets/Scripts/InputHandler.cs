using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [HideInInspector] public Vector2 moveInput;
    [HideInInspector] public bool sprintInput;
    [HideInInspector] public bool jumpInput;
    [HideInInspector] public bool crouchInput;

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
        sprintInput = value > 0.1f;
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
}
