using UnityEngine;

public class PlayerIdleState : IState
{
    private readonly StateMachine _fsm;
    private readonly InputHandler _inputHandler;
    private readonly CharacterController _characterController;
    private readonly float _playerActualSpeed;

    private Vector3 movement;

    public PlayerIdleState(StateMachine fsm, InputHandler inputHandler, CharacterController characterController, float playerActualSpeed)
    {
        _fsm = fsm;
        _inputHandler = inputHandler;
        _characterController = characterController;
        _playerActualSpeed = playerActualSpeed;
    } 

    public void OnEnter()
    {
        _inputHandler.canRun = true;
    }
    public void OnExit()
    {
    }
    public void OnFixedTick()
    {
    }
    public void OnTick()
    {
        StateChange();

        movement = new Vector3(_inputHandler.moveInput.x, 0, _inputHandler.moveInput.y);

        _characterController.Move(movement * Time.deltaTime * _playerActualSpeed);
        _characterController.Move(new Vector3(0, -9.81f, 0) * Time.deltaTime);
    }

    private void StateChange()
    {
        if(movement != Vector3.zero) { _fsm.SetState("Walk"); }
        if (_inputHandler.jumpInput && _characterController.isGrounded) { _fsm.SetState("Jump"); }
        if (_inputHandler.crouchInput && _characterController.isGrounded) { _fsm.SetState("Crouch"); }
        if (_inputHandler.attackInput || Input.GetKeyDown(KeyCode.R)) { _fsm.SetState("Combat"); }
    }
}
