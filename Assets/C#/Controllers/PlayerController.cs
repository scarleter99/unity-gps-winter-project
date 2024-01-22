using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerController : BaseController
{
    private int _layerMask = (1 << (int)Define.Layer.Monster);
    
    [ReadOnly(false), SerializeField]
    protected PlayerStat _stat;
    protected Define.WeaponType _weaponType;
    protected Bag _bag;
    public PlayerStat Stat { get => _stat; }

    public Define.WeaponType WeaponType { get => _weaponType; set => _weaponType = value; }
    public Bag Bag { get => _bag; }
    
    public override void Init()
    {
        WorldObjectType = Define.WorldObject.Player;
        _bag = new Bag(transform);
        _stat = new PlayerStat(name); // TODO : 플레이어 닉네임 설정하면 여기에 할당
        
        Managers.InputMng.MouseAction -= OnMouseEvent;
        Managers.InputMng.MouseAction += OnMouseEvent;
        Managers.InputMng.KeyAction -= OnKeyboard;
        Managers.InputMng.KeyAction += OnKeyboard;
    }
    
    public void PlayerStatChange(PlayerStatStruct statStruct)
    {
        // TODO
    }

    #region Event

    // 적절한 Animation Timing에서 호출
    protected override void OnAttackEvent()
    {
        if (_lockTarget != null)
        {
            switch (WeaponType)
            {
                case Define.WeaponType.DoubleSword:
                    _lockTarget.GetComponent<BaseController>().OnDamage(Stat, 2);
                    break;
                default:
                    _lockTarget.GetComponent<BaseController>().OnDamage(Stat);
                    break;
            }
        }
    }

    protected void OnKeyboard()
    {
        // temp - for test
        if (Input.GetKeyDown(KeyCode.Alpha1))
            Bag.UseItem(this, 0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            Bag.UseItem(this, 1);
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
                break;
        }
    }
    
    public override void OnDamage(Stat attackerStat, int amount = 1) 
    {
        var nextState = (AnimState == Define.AnimState.Defend) ? Define.AnimState.Defend : Define.AnimState.Hit;
        Stat.OnDamage(attackerStat, amount);
        nextState = (_stat.Hp > 0) ? nextState : Define.AnimState.Die;
        AnimState = nextState;
    }

    #endregion
    
    #region Update
    
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

    #endregion

}
