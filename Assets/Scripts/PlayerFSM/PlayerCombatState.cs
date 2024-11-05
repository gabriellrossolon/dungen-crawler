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

    private bool canAttack = true;
    private Vector3 movement;
    private float _playerActualSpeed;


    public PlayerCombatState(
         StateMachine fsm,
         InputHandler inputHandler,
         Animator animator,
         CharacterController characterController,
         Transform playerTransform,
         Func<float> getCurrentSpeed,
         GameObject weaponHandSlot,
         GameObject weaponBackSlot
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
    }

    public void OnEnter()
    {
        _animator.SetBool("combatState", true);
        _inputHandler.canRun = false;
        _animator.SetTrigger("drawWeapon");
    }
    public void OnExit()
    {
        _animator.SetBool("combatState", false);
        _animator.SetTrigger("undrawWeapon");
    }
    public void OnFixedTick()
    {
    }

    public void OnTick()
    {
        _playerActualSpeed = _getCurrentSpeed();
        if (_inputHandler.attackInput) { ExecutaAttack(); }
        AttackCooldown();
        Move();
        AnimationControll();
        ChangeState();
    }

    private void ExecutaAttack()
    {
        if (!canAttack)
        {
            return;
        }
        _animator.SetTrigger("isAttack");
        canAttack = false;
    }

    private void AttackCooldown()
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(2);
        if (stateInfo.IsName("Attack1H") || stateInfo.IsName("Attack2H"))
        {
            if (stateInfo.normalizedTime >= 0.5f)
            {
                canAttack = true;
            }
        }
    }

    private void ChangeState()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            _fsm.SetState("Idle"); 
        }
    }

    private void Move()
    {
        movement = new Vector3(_inputHandler.moveInput.x, 0, _inputHandler.moveInput.y);
        movement = Camera.main.transform.TransformDirection(movement);
        movement.y = 0;

        _characterController.Move(_playerActualSpeed * Time.deltaTime * movement);
        _characterController.Move(new Vector3(0, -9.81f, 0) * Time.deltaTime);

        if (movement != Vector3.zero)
        {
            _playerTransform.rotation = Quaternion.Slerp(_playerTransform.rotation, Quaternion.LookRotation(movement), Time.deltaTime * 10);
        }
    }

    private void AnimationControll()
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(3);
        Debug.Log("Animação Atual: " + stateInfo.shortNameHash + ", Normalized Time: " + stateInfo.normalizedTime);

        if (stateInfo.IsName("DrawWeapon"))
        {
            if (stateInfo.normalizedTime >= 0.4f)
            {
                _weaponBackSlot.SetActive(false);
                _weaponHandSlot.SetActive(true);
            }
        } 
        else if (stateInfo.IsName("UndrawWeapon"))
        {
            if (stateInfo.normalizedTime >= 0.4f)
            {
                _weaponBackSlot.SetActive(true);
                _weaponHandSlot.SetActive(false);
            }
        }
    }
}
