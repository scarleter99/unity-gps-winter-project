using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerController : BaseController
{
    private int _layerMask = (1 << (int)Define.Layer.Monster);
    private PlayerStat _stat;
    
    public override void Init()
    {
        WorldObjectType = Define.WorldObject.Player;
        _stat = GetComponent<PlayerStat>();
        
        Managers.InputMng.MouseAction -= OnMouseEvent;
        Managers.InputMng.MouseAction += OnMouseEvent;
    }

    public override void OnDamage(Stat attackerStat)
    {
        base.OnDamage(attackerStat);
        _stat.OnAttacked(attackerStat);
    }

    protected override void UpdateAttack()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            AnimState = Define.AnimState.Idle;
    }

    protected override void UpdateHit()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
            AnimState = Define.AnimState.Idle;
    }

    protected override void OnAttackEvent()
    {
        if (_lockTarget != null)
            _lockTarget.GetComponent<BaseController>().OnDamage(_stat);
    }

    private void OnMouseEvent(Define.MouseEvent evt)
    {
        if (TurnState != Define.TurnState.Action)
            return;

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

        //Debug.Assert(hit.collider.gameObject.layer == (int)Define.Layer.Monster);
        _lockTarget = hit.collider.gameObject;
        OnAttackEvent();
        
        AnimState = Define.AnimState.Attack;
        if (TurnState == Define.TurnState.Action)
            TurnState = Define.TurnState.End;
    }
}
