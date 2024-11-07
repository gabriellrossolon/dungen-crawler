using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : IState
{
    private readonly StateMachine _fsm;
    private readonly NavMeshAgent _agent;
    private readonly Transform _playerPos;
    private readonly float _detectionRange;

    public EnemyChaseState(StateMachine fsm, NavMeshAgent agent, Transform playerPos, float detectionRange)
    {
        _fsm = fsm;
        _agent = agent;
        _playerPos = playerPos;
        _detectionRange = detectionRange;
    }

    public void OnEnter()
    {
        _agent.isStopped = false;
        _agent.SetDestination(_playerPos.position);
    }
    public void OnExit()
    {
    }
    public void OnFixedTick()
    {
    }
    public void OnTick()
    {
        _agent.SetDestination(_playerPos.position);
        Detection();
    }

    private void Detection()
    {
        float distanceToPlayer = Vector3.Distance(_agent.transform.position, _playerPos.position);

        if (distanceToPlayer > _detectionRange)
        {
            _fsm.SetState("Idle");
        }
    }
}
