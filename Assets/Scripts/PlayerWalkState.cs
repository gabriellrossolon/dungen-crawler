using System;
using UnityEngine;

public class PlayerWalkState : IState
{
    private StateMachine _fsm;
    private InputHandler _inputHandler; // Referência ao InputHandler
    private CharacterController _characterController;
    private Transform _cameraPos;
    private float _playerActualSpeed;
    private Transform _playerTransform;

    private Vector3 movement;

    private Func<float> _getCurrentSpeed;

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
        float _playerActualSpeed = _getCurrentSpeed();

        movement = new Vector3(_inputHandler.moveInput.x, 0, _inputHandler.moveInput.y);
        movement = _cameraPos.TransformDirection(movement);
        movement.y = 0;

        _characterController.Move(movement * Time.deltaTime * _playerActualSpeed);
        _characterController.Move(new Vector3(0, -9.81f, 0) * Time.deltaTime);

        if(movement != Vector3.zero)
        {
            _playerTransform.rotation = Quaternion.Slerp(_playerTransform.rotation, Quaternion.LookRotation(movement), Time.deltaTime * 10);
        }

        if (movement == Vector3.zero)
            _fsm.SetState("Idle");
    }
}
