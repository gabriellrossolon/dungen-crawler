using UnityEngine;
using UnityEngine.AI;

public class EnemyAttackState : IState
{
    private readonly StateMachine _fsm;
    private readonly Animator _animator;
    private readonly EnemyWeaponBehavior _enemyWeaponBehavior;
    public EnemyAttackState(StateMachine fsm, Animator animator, EnemyWeaponBehavior enemyWeaponBehavior)
    {
        _fsm = fsm;
        _animator = animator;
        _enemyWeaponBehavior = enemyWeaponBehavior;
    }

    public void OnEnter()
    {
        _animator.SetTrigger("isAttack");
    }
    public void OnExit()
    {
        _enemyWeaponBehavior.isAttacking = false;
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
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Attack"))
        {
            if (stateInfo.normalizedTime >= 0.3f && stateInfo.normalizedTime < 0.35f)
            {
                _enemyWeaponBehavior.isAttacking = true;
            }
            if (stateInfo.normalizedTime >= 0.7f)
            {
                _fsm.SetState("Idle");
            }
        }
    }
}
