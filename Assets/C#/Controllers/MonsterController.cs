using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterController : BaseController
{
    [ReadOnly(false), SerializeField]
    protected Stat _stat;
    public Stat Stat { get => _stat; }
    
    public override void Init()
    {
        WorldObjectType = Define.WorldObject.Monster;
        _stat = new Stat(gameObject.name);
        
    //////////////////////////////////////////
    // TEST CODE
    //////////////////////////////////////////
        Managers.InputMng.KeyAction -= OnKeyboard;
        Managers.InputMng.KeyAction += OnKeyboard;
    }

    protected void OnKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.A))
            AnimState = Define.AnimState.Attack;
    }
    //////////////////////////////////////////
    //}
    
    public override void OnDamage(Stat attackerStat, int amount = 1)
    {
        var nextState = (AnimState == Define.AnimState.Defend) ? Define.AnimState.DefendHit : Define.AnimState.Hit;
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
            _lockTarget.GetComponent<BaseController>().OnDamage(Stat);
    }
    
}
