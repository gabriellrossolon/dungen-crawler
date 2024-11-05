using System;
using UnityEngine;

public class PlayerJumpState : IState
{
    private readonly StateMachine _fsm;
    private readonly InputHandler _inputHandler;
    private readonly CharacterController _characterController;
    private readonly Animator _animator;
    private readonly Func<float> _getCurrentSpeed;
    private readonly Func<float> _getCurrentJumpForce;

    private float _playerActualSpeed;
    private Vector3 movement;
    private float _gravityForce;
    private bool normalJump = false;
    private bool sprintJump = false;
    private float entryXValue;
    private float entryYValue;

    public PlayerJumpState(
        StateMachine fsm,
        InputHandler inputHandler,
        CharacterController characterController,
        Animator animator,
        Func<float> getCurrentSpeed,
        Func<float> getCurrentJumpForce
    )
    {
        _fsm = fsm;
        _inputHandler = inputHandler;
        _characterController = characterController;
        _animator = animator;
        _getCurrentSpeed = getCurrentSpeed;
        _getCurrentJumpForce = getCurrentJumpForce;
    }

    public void OnEnter()
    {
        _gravityForce = _getCurrentJumpForce();
        _playerActualSpeed = _getCurrentSpeed();

        if (_inputHandler.moveInput.x == 0 && _inputHandler.moveInput.y == 0)
        {
            normalJump = true;
            _animator.SetTrigger("isJump");
        }
        else if (_inputHandler.moveInput.x != 0 || _inputHandler.moveInput.y != 0)
        {
            sprintJump = true;
            _animator.SetTrigger("isSprintJump");

            entryXValue = _inputHandler.moveInput.x;
            entryYValue = _inputHandler.moveInput.y;
        }

        _inputHandler.canRun = false;
    }
    public void OnExit()
    {
        normalJump = false;
        sprintJump = false;

        entryXValue = 0;
        entryXValue = 0;
    }
    public void OnFixedTick()
    { 
    }

    public void OnTick()
    {
        Jump();
        StateChange();
        GravityControll();
    }

    private void Jump()
    {
        movement = new Vector3(entryXValue, 0, entryYValue);
        movement = Camera.main.transform.TransformDirection(movement);
        movement.y = 0;

        if (normalJump)
        {
            _characterController.Move(new Vector3(0, _gravityForce, 0) * Time.deltaTime);
        }
        else if (sprintJump)
        {
            _characterController.Move(_playerActualSpeed * Time.deltaTime * movement);
            _characterController.Move(new Vector3(0, _gravityForce, 0) * Time.deltaTime);
        }
    }
    private void GravityControll()
    {
        if (_gravityForce > -9.81f)
        {
            _gravityForce += -9.81f * Time.deltaTime;
        }
    }

    private void StateChange()
    {
        if (_characterController.isGrounded)
        {
            _animator.SetTrigger("isGrounded");
            _fsm.SetState("Idle");
        }
    }
}
