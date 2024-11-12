using System;
using UnityEditor;
using UnityEngine;

public class PlayerCombatState : IState
{
    private readonly StateMachine _fsm;
    private readonly InputHandler _inputHandler;
    private readonly Animator _animator;
    private readonly CharacterController _characterController;
    private readonly Transform _playerTransform;
    private readonly Func<float> _getCurrentSpeed;
    private readonly GameObject _weaponHandSlot;
    private readonly GameObject _weaponBackSlot;
    private readonly Func<bool> _usingTwoHand;
    private readonly PlayerStats _playerStats;

    private bool canAttack = false;
    private bool canMove = true;
    private Vector3 movement;
    private float _playerActualSpeed;
    private bool drawingWeapon;
    private bool canRemoveStamina;

    public PlayerCombatState(
         StateMachine fsm,
         InputHandler inputHandler,
         Animator animator,
         CharacterController characterController,
         Transform playerTransform,
         Func<float> getCurrentSpeed,
         GameObject weaponHandSlot,
         GameObject weaponBackSlot,
         Func<bool> usingTwoHand,
         PlayerStats playerStats
         )
    {
        _fsm = fsm;
        _inputHandler = inputHandler;
        _animator = animator;
        _characterController = characterController;
        _playerTransform = playerTransform;
        _getCurrentSpeed = getCurrentSpeed;
        _weaponHandSlot = weaponHandSlot;
        _weaponBackSlot = weaponBackSlot;
        _usingTwoHand = usingTwoHand;
        _playerStats = playerStats;
    }

    public void OnEnter()
    {
        _animator.SetBool("1HEquip", !_usingTwoHand());
        _animator.SetBool("2HEquip", _usingTwoHand());

        _animator.SetBool("combatState", true);
        _inputHandler.canRun = false;
        _animator.SetTrigger("drawWeapon");
    }
    public void OnExit()
    {
        _animator.SetBool("combatState", false);
        drawingWeapon = false;
        canAttack = false;
        _animator.SetBool("1HEquip", false);
        _animator.SetBool("2HEquip", false);
    }
    public void OnFixedTick()
    {
    }

    public void OnTick()
    {
        _playerActualSpeed = _getCurrentSpeed();
        if (_inputHandler.rightHandInput) { ExecutaAttackOne(); }
        if (_inputHandler.leftHandInput) { ExecuteAttackTwo(); }
        AttackCooldown();
        Move();
        AnimationControll();
        ChangeState();
    }

    private void ExecutaAttackOne()
    {
        if (canAttack)
        {
            _animator.SetTrigger("isAttack");
            canAttack = false;
            RemoveStamina(10f);
        }
    }

    private void ExecuteAttackTwo()
    {
        if (canAttack)
        {
            _animator.SetTrigger("isAttackTwo");
            canAttack = false;
            RemoveStamina(15f);
        }
    }

    private void RemoveStamina(float value)
    {
        if (canRemoveStamina)
        {
            canRemoveStamina = false;
            _playerStats.currentStamina -= value;
        }
        
    }

    private void AttackCooldown()
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(3);
        if (stateInfo.IsName("Attack1H") || stateInfo.IsName("Attack1H2") || stateInfo.IsName("Attack2H") || stateInfo.IsName("Attack2H2"))
        {
            canMove = false;
            if (stateInfo.normalizedTime >= 0.5f)
            {
                canAttack = true;

                if (stateInfo.normalizedTime >= 0.6f)
                {
                    canMove = true;
                } 
            }
        }
        else
        {
            canRemoveStamina = true;
        }
    }

    private void ChangeState()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            _animator.SetTrigger("undrawWeapon");
            drawingWeapon = true;
        }
    }

    private void Move()
    {
        if (canMove)
        {
            movement = new Vector3(_inputHandler.moveInput.x, 0, _inputHandler.moveInput.y);
            movement = Camera.main.transform.TransformDirection(movement);
            movement.y = 0;

            _characterController.Move(_playerActualSpeed * Time.deltaTime * movement);
            _characterController.Move(new Vector3(0, -9.81f, 0) * Time.deltaTime);
        }

        if (movement != Vector3.zero)
        {
            _playerTransform.rotation = Quaternion.Slerp(_playerTransform.rotation, Quaternion.LookRotation(movement), Time.deltaTime * 10);
        }
    }

    private void AnimationControll()
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(2);
        if (stateInfo.IsName("DrawWeapon"))
        {
            if (stateInfo.normalizedTime >= 0.4f)
            {
                _weaponBackSlot.SetActive(false);
                _weaponHandSlot.SetActive(true);
                canAttack = true;
            }
        }
        
        if (drawingWeapon)
        {
            if (stateInfo.IsName("UndrawWeapon"))
            {
                if (stateInfo.normalizedTime >= 0.7f)
                {
                    _weaponBackSlot.SetActive(true);
                    _weaponHandSlot.SetActive(false);
                    _fsm.SetState("Idle");
                }
            }
        }
    }
}
