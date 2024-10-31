using UnityEngine;

public class PlayerFSM : MonoBehaviour
{
    private StateMachine _fsm;
    public StateMachine FSM => _fsm;

    private InputHandler _inputHandler; // Referência ao InputHandler
    private CharacterController _characterController;
    private Transform _cameraPos;
    private Transform _playerTransform;
    private Animator _animator;

    public float _playerSpeed = 5f;
    public float _playerSprintSpeed = 8f;
    [SerializeField] private float _playerActualSpeed;


    private void Awake()
    {
        // Inicializa a statemachine e adiciona os estados
        _fsm = new();
        _inputHandler = GetComponent<InputHandler>();
        _characterController = GetComponent<CharacterController>();
        _cameraPos = Camera.main.transform;
        _playerTransform = transform;
        _animator = GetComponent<Animator>();

        _fsm.AddState("Idle", new PlayerIdleState(_fsm, _inputHandler, _characterController, _playerActualSpeed));
        _fsm.AddState("Walk", new PlayerWalkState(_fsm, _inputHandler, _characterController, _cameraPos, () => _playerActualSpeed, _playerTransform));

        _fsm.SetInitialState("Idle"); // Define o estado inicial
    }

    private void Update()
    {
        // Chama o Tick dela no Update
        _fsm.Tick();

        SpeedController();
    }

    private void FixedUpdate()
    {
        // Chama o FixedTIck dela no FixedUpdate
        _fsm.FixedTick();
    }

    private void SpeedController()
    {
        float targetSpeed;

        if (_inputHandler.moveInput == Vector2.zero)
        {
            targetSpeed = 0;
        }
        else
        {
            targetSpeed = _playerSpeed;
        }

        _playerActualSpeed = Mathf.Lerp(_playerActualSpeed, targetSpeed, 0.1f);

        if (Mathf.Abs(_playerActualSpeed - targetSpeed) < 0.01f)
        {
            _playerActualSpeed = targetSpeed;
        }

        _animator.SetFloat("speed", _playerActualSpeed);
    }
}
