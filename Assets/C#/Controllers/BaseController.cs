using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;
using Random = System.Random;

public abstract class BaseController : MonoBehaviour
{
    public Define.WorldObject WorldObjectType { get; protected set; } = Define.WorldObject.Unknown;
    protected ulong _id;
    protected int _stateHash;

    public ulong Id { get => _id; set => _id = value; }
    
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

            string stateName = "";
            switch (_animState)
            {
                case Define.AnimState.Attack:
                    _animator.Play(stateName = "Attack1");
                    break;
                case Define.AnimState.Defend:
                    _animator.Play(stateName = "Defend");
                    break;
                case Define.AnimState.DefendHit:
                    _animator.Play(stateName = "DefendHit");
                    break;
                case Define.AnimState.Die:
                    index = random.Next(minIndex, maxDieIndex);
                    _animator.Play(stateName = $"Die{index}");
                    break;
                case Define.AnimState.Dizzy:
                    _animator.Play(stateName = "Dizzy");
                    break;
                case Define.AnimState.Hit:
                    index = random.Next(minIndex, maxHitIndex);
                    _animator.Play(stateName = $"Hit{index}");
                    break;
                case Define.AnimState.Idle:
                    _animator.CrossFade(stateName = "Idle", 0.2f);
                    break;
                //case Define.AnimState.Skill:
                //    break;
                case Define.AnimState.Victory:
                    _animator.Play(stateName = "Victory");
                    break;
            }

            _stateHash = Animator.StringToHash(stateName);
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
        ChangeAnim();
    }

    public void ChangeAnim()
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
    
    public virtual void StatChange(IStat statStruct)
    {
        // TODO
    }

    #region Event

    // Animation의 적절한 타이밍에서 호출
    protected abstract void OnAttackEvent();

    public abstract void OnDamage(BaseController attacker, int amount = 1);

    #endregion
    
    #region Update

    protected virtual void UpdateAttack() { }
    protected virtual void UpdateDefend() { }
    protected virtual void UpdateDefendHit() { }
    
    protected virtual void UpdateDie()
    {
        var currentState = _animator.GetCurrentAnimatorStateInfo(0);
        // if (currentState.normalizedTime >= 0.98f && currentState.shortNameHash == _stateHash)
        //     Managers.GameMng.Despawn(this.gameObject);
    }
    
    protected virtual void UpdateDizzy() { }
    protected virtual void UpdateHit() { }
    protected virtual void UpdateIdle() { }
    protected virtual void UpdateVictory() { }

    #endregion
    
    public void LockAndAttack(GameObject target)
    {
        _lockTarget = target;
        AnimState = Define.AnimState.Attack;
    }
}
