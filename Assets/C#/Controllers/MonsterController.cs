using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterController : BaseController
{
    private Stat _stat;
    
    public override void Init()
    {
        WorldObjectType = Define.WorldObject.Monster;
        _stat = GetComponent<Stat>();
        
    //////////////////////////////////////////
    // TEST CODE
    //////////////////////////////////////////
        Managers.InputMng.KeyAction -= OnKeyboard;
        Managers.InputMng.KeyAction += OnKeyboard;
    }

    protected void OnKeyboard()
    {
        attacked = false;
        if (Input.GetKeyDown(KeyCode.A))
            AnimState = Define.AnimState.Attack;
    }
    //////////////////////////////////////////
    //}
    
    public override void OnDamage(Stat attackerStat)
    {
        var nextState = (AnimState == Define.AnimState.Defend) ? Define.AnimState.DefendHit : Define.AnimState.Hit;
        _stat.OnAttacked(attackerStat);
        nextState = (_stat.Hp > 0) ? nextState : Define.AnimState.Die;
        AnimState = nextState;
    }

    private bool attacked = false; // temp - FOR TEST
    protected override void UpdateAttack()
    {
        // temp - FOR TEST
        if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.33f && !attacked)
            OnAttackEvent();

        if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
            AnimState = Define.AnimState.Idle;
    }

    protected override void UpdateHit()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
            AnimState = Define.AnimState.Idle;
    }
    
    // 적절한 Animation Timing에서 호출
    protected override void OnAttackEvent()
    {
        if (_lockTarget != null)
            _lockTarget.GetComponent<BaseController>().OnDamage(_stat);

        attacked = true;
    }
    
}
