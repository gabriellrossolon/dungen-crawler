using UnityEngine;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{
    private StateMachine _fsm;
    private NavMeshAgent _agent;
    private Transform _playerPos;
    private Animator _animator;
    private EnemyWeaponBehavior _enemyWeaponBehavior;
    [SerializeField] float _detectionRange;
    private EnemyStats _enemyStats;
    
    [Header("Debug")]
    public string _currentState;

    private void Awake()
    {
        _fsm = new();
        _agent = GetComponent<NavMeshAgent>();
        _playerPos = GameObject.FindWithTag("Player")?.transform;
        _animator = GetComponentInChildren<Animator>();
        _enemyWeaponBehavior = GetComponentInChildren<EnemyWeaponBehavior>();
        _enemyStats = GetComponent<EnemyStats>();

        _fsm.AddState("Idle", new EnemyIdleState(_fsm, _agent, _playerPos, _detectionRange));
        _fsm.AddState("Chase", new EnemyChaseState(_fsm, _agent, _playerPos, _detectionRange, _animator));
        _fsm.AddState("Attack", new EnemyAttackState(_fsm, _animator, _enemyWeaponBehavior));

        _fsm.SetInitialState("Idle"); 
    }

    private void Update()
    {
        _fsm.Tick();
    }

    private void FixedUpdate()
    {
        _fsm.FixedTick();
        UpdateCurrentState();
    }


    private void UpdateCurrentState()
    {
        if (_fsm.CurrentState is EnemyIdleState)
        {
            _currentState = "Idle";
        }
        else if (_fsm.CurrentState is EnemyChaseState)
        {
            _currentState = "Chase";
        }
        else if (_fsm.CurrentState is EnemyAttackState)
        {
            _currentState = "Attack";
        }
    }
}
