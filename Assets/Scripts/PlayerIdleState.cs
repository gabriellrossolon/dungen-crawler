using UnityEngine;

public class PlayerIdleState : IState
{
    private StateMachine _fsm;
    private InputHandler _inputHandler;
    private CharacterController _characterController;
    private Vector3 movement;
    private float _playerActualSpeed;

    public PlayerIdleState(StateMachine fsm, InputHandler inputHandler, CharacterController characterController, float playerActualSpeed)
    {
        _fsm = fsm;
        _inputHandler = inputHandler;
        _characterController = characterController;
        _playerActualSpeed = playerActualSpeed;
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
        movement = new Vector3(_inputHandler.moveInput.x, 0, _inputHandler.moveInput.y);

        _characterController.Move(movement * Time.deltaTime * _playerActualSpeed);
        _characterController.Move(new Vector3(0, -9.81f, 0) * Time.deltaTime);

        if (movement != Vector3.zero)
            _fsm.SetState("Walk");
    }
}
