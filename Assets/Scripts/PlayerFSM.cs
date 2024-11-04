using Unity.VisualScripting;
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
    public float _playerCrouchSpeed = 2.5f;
    [SerializeField] private float _playerActualSpeed;
    public float _jumpForce;

    [Header("Debug")]
    public string _currentState; // Nova variável para armazenar o estado atual


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
        _fsm.AddState("Jump", new PlayerJumpState(_fsm, _inputHandler, _characterController, _animator, () => _playerActualSpeed, () => _jumpForce));
        _fsm.AddState("Crouch", new PlayerCrouchState(_fsm, _inputHandler, _characterController, () =>  _playerActualSpeed, _animator, _playerTransform, _playerCrouchSpeed));
        _fsm.AddState("Combat", new PlayerCombatState(_fsm, _inputHandler, _animator));

        _fsm.SetInitialState("Idle"); // Define o estado inicial
    }

    private void Update()
    {
        // Chama o Tick dela no Update
        _fsm.Tick();

        SpeedController();

        UpdateCurrentState();
    }

    private void FixedUpdate()
    {
        // Chama o FixedTIck dela no FixedUpdate
        _fsm.FixedTick();
    }

    private void SpeedController()
    {
        float targetSpeed = 0f;

        if (_inputHandler.moveInput == Vector2.zero)
        {
            targetSpeed = 0;
        }
        else if(_inputHandler.moveInput != Vector2.zero)
        {
            if (_inputHandler.sprintInput)
            {
                targetSpeed = _playerSprintSpeed;
            }
            else if (_inputHandler.crouchInput)
            {
                targetSpeed = _playerCrouchSpeed;
            }
            else
            {
                targetSpeed = _playerSpeed;
            }
        }

        _playerActualSpeed = Mathf.Lerp(_playerActualSpeed, targetSpeed, 0.1f);

        if (Mathf.Abs(_playerActualSpeed - targetSpeed) < 0.01f)
        {
            _playerActualSpeed = targetSpeed;
        }

        _animator.SetFloat("speed", _playerActualSpeed);
    }

    private void UpdateCurrentState()
    {
        // Atualiza o estado atual baseado no estado da máquina de estados
        // Aqui você pode implementar uma lógica simples que atualiza o _currentState baseado no que a sua máquina de estados está fazendo
        if (_fsm.CurrentState is PlayerIdleState)
        {
            _currentState = "Idle";
        }
        else if (_fsm.CurrentState is PlayerWalkState)
        {
            _currentState = "Walk";
        }
        else if (_fsm.CurrentState is PlayerJumpState)
        {
            _currentState = "Jump";
        }
        else if (_fsm.CurrentState is PlayerCrouchState)
        {
            _currentState = "Crouch";
        }
        else if (_fsm.CurrentState is PlayerCombatState)
        {
            _currentState = "Combat";
        }
    }
}
