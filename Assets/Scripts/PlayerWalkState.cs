using System;
using UnityEngine;

public class PlayerWalkState : IState
{
    private readonly StateMachine _fsm;
    private readonly InputHandler _inputHandler;
    private readonly CharacterController _characterController;
    private readonly Transform _cameraPos;
    private readonly Transform _playerTransform;
    private readonly Func<float> _getCurrentSpeed;

    private float _playerActualSpeed;
    private Vector3 movement;

    public PlayerWalkState(
        StateMachine fsm,
        InputHandler inputHandler,
        CharacterController characterController,
        Transform cameraPos,
        Func<float> getCurrentSpeed,
        Transform playerTransform
        )
    {
        _fsm = fsm;
        _inputHandler = inputHandler;
        _characterController = characterController;
        _cameraPos = cameraPos;
        _getCurrentSpeed = getCurrentSpeed;
        _playerTransform = playerTransform;
    }

    public void OnEnter()
    {
    }
    public void OnExit()
    {
    }
    public void OnFixedTick()
    {
    }

    public void OnTick()
    {
        _playerActualSpeed = _getCurrentSpeed();

        Move();
        StateChange();
    }

    private void Move()
    {
        movement = new Vector3(_inputHandler.moveInput.x, 0, _inputHandler.moveInput.y);
        movement = _cameraPos.TransformDirection(movement);
        movement.y = 0;

        _characterController.Move(_playerActualSpeed * Time.deltaTime * movement);
        _characterController.Move(new Vector3(0, -9.81f, 0) * Time.deltaTime);

        if (movement != Vector3.zero)
        {
            _playerTransform.rotation = Quaternion.Slerp(_playerTransform.rotation, Quaternion.LookRotation(movement), Time.deltaTime * 10);
        }
        //OBS, Sprint ou Walk e controlado pelo Player FSM, no SpeedController();
    }

    private void StateChange()
    {
        if (movement == Vector3.zero) { _fsm.SetState("Idle"); }
        if (_inputHandler.jumpInput && _characterController.isGrounded) { _fsm.SetState("Jump"); }
        if (_inputHandler.crouchInput && _characterController.isGrounded) { _fsm.SetState("Crouch"); }
        if (_inputHandler.attackInput) { _fsm.SetState("Combat"); }
    }
}
