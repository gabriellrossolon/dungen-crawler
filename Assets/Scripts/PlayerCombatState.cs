using UnityEngine;

public class PlayerCombatState : IState
{
    private readonly StateMachine _fsm;
    private readonly InputHandler _inputHandler;
    private readonly Animator _animator;
    public PlayerCombatState(
         StateMachine fsm,
         InputHandler inputHandler,
         Animator animator
         )
    {
        _fsm = fsm;
        _inputHandler = inputHandler;
        _animator = animator;
    }

    public void OnEnter()
    {
        _animator.SetTrigger("isAttack");
    }
    public void OnExit()
    {
    }
    public void OnFixedTick()
    {
    }

    public void OnTick()
    {
        ChangeState();
    }

    private void ChangeState()
    {
        if(!_inputHandler.attackInput)
        {
            _fsm.SetState("Idle");
        }
    }
}
