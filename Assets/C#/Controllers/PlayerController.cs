using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerController : BaseController
{
    private int _layerMask = (1 << (int)Define.Layer.Monster);
    protected PlayerStat _stat;
    protected Define.WeaponType _weaponType;
    public PlayerStat Stat { get => _stat; }

    public Define.WeaponType WeaponType { get => _weaponType; set => _weaponType = value; }

    public override void Init()
    {
        WorldObjectType = Define.WorldObject.Player;
        _stat = GetComponent<PlayerStat>();
        
        Managers.InputMng.MouseAction -= OnMouseEvent;
        Managers.InputMng.MouseAction += OnMouseEvent;
        Managers.InputMng.KeyAction -= OnKeyboard;
        Managers.InputMng.KeyAction += OnKeyboard;
        
        // temp - for test
        Managers.GameMng.Player = gameObject;
    }

    public override void OnDamage(Stat attackerStat, int amount = 1) 
    {
        var nextState = (AnimState == Define.AnimState.Defend) ? Define.AnimState.Defend : Define.AnimState.Hit;
        _stat.OnDamage(attackerStat, amount);
        nextState = (_stat.Hp > 0) ? nextState : Define.AnimState.Die;
        AnimState = nextState;
    }

    protected override void UpdateAttack()
    {
        var currentState = _animator.GetCurrentAnimatorStateInfo(0);
        if (currentState.normalizedTime >= 0.8f && currentState.shortNameHash == _stateHash)
            AnimState = Define.AnimState.Idle;
    }

    protected override void UpdateHit()
    {
        var currentState = _animator.GetCurrentAnimatorStateInfo(0);
        if (currentState.normalizedTime >= 0.8f && currentState.shortNameHash == _stateHash)
            AnimState = Define.AnimState.Idle;
    }
    
    // 적절한 Animation Timing에서 호출
    protected override void OnAttackEvent()
    {
        if (_lockTarget != null)
        {
            switch (WeaponType)
            {
                case Define.WeaponType.DoubleSword:
                    _lockTarget.GetComponent<BaseController>().OnDamage(_stat, 2);
                    break;
                default:
                    _lockTarget.GetComponent<BaseController>().OnDamage(_stat);
                    break;
            }
        }
    }

    protected void OnKeyboard()
    {
        // temp - for test
        if (Input.GetKeyDown(KeyCode.Alpha1))
            Managers.GameMng.Bag.UseItem(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            Managers.GameMng.Bag.UseItem(1);
    }

    private void OnMouseEvent(Define.MouseEvent evt)
    {
        // if (TurnState != Define.TurnState.Action)
        //     return;

        if (AnimState != Define.AnimState.Idle)
            return;
        
        switch (evt)
        {
            case Define.MouseEvent.PointerDown:
                OnMouseEvent_Attack();
                break;
        }
    }

    private void OnMouseEvent_Attack()
    {
        if (AnimState != Define.AnimState.Idle)
            return;
        
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool raycastHit = Physics.Raycast(ray, out hit, 100.0f, _layerMask);
        if (!raycastHit)
            return;

        _lockTarget = hit.collider.gameObject;
        
        AnimState = Define.AnimState.Attack;
        if (TurnState == Define.TurnState.Action)
            TurnState = Define.TurnState.End;
    }
}
