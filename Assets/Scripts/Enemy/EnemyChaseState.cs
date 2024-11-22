using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : IState
{
    private readonly StateMachine _fsm;
    private readonly NavMeshAgent _agent;
    private readonly Transform _playerPos;
    private readonly float _detectionRange;
    private readonly Animator _animator;

    public EnemyChaseState(StateMachine fsm, NavMeshAgent agent, Transform playerPos, float detectionRange, Animator animator)
    {
        _fsm = fsm;
        _agent = agent;
        _playerPos = playerPos;
        _detectionRange = detectionRange;
        _animator = animator;
    }

    public void OnEnter()
    {
        _agent.isStopped = false;
        _agent.SetDestination(_playerPos.position);
        _animator.SetBool("isWalk", true);
    }
    public void OnExit()
    {
        _animator.SetBool("isWalk", false);
        _agent.isStopped = true;
    }
    public void OnFixedTick()
    {
    }
    public void OnTick()
    {
        Chase();
        Detection();
    }

    private void Detection()
    {
        float distanceToPlayer = Vector3.Distance(_agent.transform.position, _playerPos.position);

        if (distanceToPlayer > _detectionRange)
        {
            _fsm.SetState("Idle");
        }

        if(distanceToPlayer <= 3f)
        {
            _fsm.SetState("Attack");
        }
    }

    private void Chase()
    {
        _agent.SetDestination(_playerPos.position);
        Vector3 direction = (_playerPos.position - _agent.transform.position).normalized;
        if(direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            _agent.transform.rotation = Quaternion.Slerp(_agent.transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    
}
