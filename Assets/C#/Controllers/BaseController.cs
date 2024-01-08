using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public abstract class BaseController : MonoBehaviour
{
    public Define.WorldObject WorldObjectType { get; protected set; } = Define.WorldObject.Unknown;
    
    [SerializeField]
    protected Define.AnimState _animState = Define.AnimState.Idle;
    [SerializeField] 
    protected Define.TurnState _turnState = Define.TurnState.Wait;
    [SerializeField]
    protected GameObject _lockTarget;

    protected Animator _animator;

    public Define.AnimState AnimState
    {
        get => _animState;
        set
        {
            _animState = value;
            Random random = new Random();

            bool isPlayer = WorldObjectType == Define.WorldObject.Player;
            int minIndex = 1;
            int maxHitIndex = isPlayer ? 3 : 2;
            int maxDieIndex = isPlayer ? 3 : 2;
            int index;
            
            switch (_animState)
            {
                case Define.AnimState.Attack:
                    _animator.CrossFade("Attack1", 0.01f);
                    break;
                case Define.AnimState.Defend:
                    _animator.CrossFade("Defend", 0.01f);
                    break;
                case Define.AnimState.DefendHit:
                    _animator.CrossFade("DefendHit", 0.01f);
                    break;
                case Define.AnimState.Die:
                    index = random.Next(minIndex, maxDieIndex);
                    _animator.CrossFade($"Die{index}", 0.01f);
                    break;
                case Define.AnimState.Dizzy:
                    _animator.CrossFade("Dizzy", 0.01f);
                    break;
                case Define.AnimState.Hit:
                    index = random.Next(minIndex, maxHitIndex);
                    _animator.CrossFade($"Hit{index}", 0.01f);
                    break;
                case Define.AnimState.Idle:
                    _animator.CrossFade("Idle", 0.2f);
                    break;
                //case Define.AnimState.Skill:
                //    break;
                case Define.AnimState.Victory:
                    _animator.CrossFade("Victory", 0.01f);
                    break;
            }
        }
    }

    public Define.TurnState TurnState
    {
        get => _turnState;
        set => _turnState = value;
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        Init();
    }
    
    private void Update()
    {
        switch (AnimState)
        {
            case Define.AnimState.Attack:
                UpdateAttack();
                break;
            case Define.AnimState.Defend:
                UpdateDefend();
                break;
            case Define.AnimState.DefendHit:
                UpdateDefendHit();
                break;
            case Define.AnimState.Die:
                UpdateDie();
                break;
            case Define.AnimState.Dizzy:
                UpdateDizzy();
                break;
            case Define.AnimState.Hit:
                UpdateHit();
                break;
            case Define.AnimState.Idle:
                UpdateIdle();
                break;
            //case Define.AnimState.Skill:
            //    break;
            case Define.AnimState.Victory:
                UpdateVictory();
                break;
        }
    }
    
    public abstract void Init();
    
    // Animation의 적절한 타이밍에서 호출
    protected abstract void OnAttackEvent();

    public abstract void OnDamage(Stat attackerStat);
    
    protected virtual void UpdateAttack() { }
    protected virtual void UpdateDefend() { }
    protected virtual void UpdateDefendHit() { }
    
    protected virtual void UpdateDie()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.98f)
            Managers.GameMng.Despawn(this.gameObject);
    }
    
    protected virtual void UpdateDizzy() { }
    protected virtual void UpdateHit() { }
    protected virtual void UpdateIdle() { }
    protected virtual void UpdateVictory() { }
    
    
}
