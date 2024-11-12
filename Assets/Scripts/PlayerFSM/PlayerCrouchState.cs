using System;
using System.Linq;
using UnityEngine;

public class PlayerCrouchState : IState
{
    private readonly StateMachine _fsm;
    private readonly InputHandler _inputHandler;
    private readonly CharacterController _characterController;
    private readonly Func<float> _getCurrentSpeed;
    private readonly Animator _animator;
    private readonly Transform _playerTransform;
    private readonly float _playerCrouchSpeed;

    private Vector3 movement;
    private float _playerActualSpeed;

    private float normalCenterY = 1.03f;
    private float crouchCenterY = 0.70f;
    private float normalHeight = 2f;
    private float crouchHeight = 1.35f;
    
    public PlayerCrouchState(
        StateMachine fsm,
        InputHandler inputHandler,
        CharacterController characterController,
        Func<float> getCurrentSpeed,
        Animator animator,
        Transform playerTransform,
        float playerCrouchSpeed
        )
    {
        _fsm = fsm;
        _inputHandler = inputHandler;
        _characterController = characterController;
        _getCurrentSpeed = getCurrentSpeed;
        _animator = animator;
        _playerTransform = playerTransform;
        _playerCrouchSpeed = playerCrouchSpeed;
    }

    public void OnEnter()
    {
        _animator.SetBool("isCrouch", true);
        Crouch();
        _inputHandler.canRun = false;
    }
    public void OnExit()
    {
        _animator.SetBool("isCrouch", false);

        _characterController.center = new Vector3(_characterController.center.x, normalCenterY, _characterController.center.z);
        _characterController.height = normalHeight;
    }
    public void OnFixedTick()
    {
    }
    public void OnTick()
    {
        _playerActualSpeed = (_playerActualSpeed >= _playerCrouchSpeed) ? _playerCrouchSpeed : _getCurrentSpeed();

        Move();
        StateChange();
    }

    private void Move()
    {
        movement = new Vector3(_inputHandler.moveInput.x, 0, _inputHandler.moveInput.y);
        movement = Camera.main.transform.TransformDirection(movement);
        movement.y = 0;

        _characterController.Move(_playerActualSpeed * Time.deltaTime * movement);
        _characterController.Move(new Vector3(0, -9.81f, 0) * Time.deltaTime);

        if (movement != Vector3.zero)
        {
            _playerTransform.rotation = Quaternion.Slerp(_playerTransform.rotation, Quaternion.LookRotation(movement), Time.deltaTime * 10);
        }
    }

    private void Crouch()
    {
        _characterController.center = new Vector3(_characterController.center.x, crouchCenterY, _characterController.center.z);
        _characterController.height = crouchHeight;
    }

    private void StateChange()
    {
        float raycastDistance = 1.6f;
        RaycastHit hit;
        if (!_inputHandler.crouchInput && !Physics.Raycast(_playerTransform.position, Vector3.up, out hit, raycastDistance, ~LayerMask.GetMask("Player")))
        {
            _fsm.SetState("Idle");
        }
        Debug.DrawRay(_playerTransform.position, Vector3.up * raycastDistance, Color.red);
    }
}
