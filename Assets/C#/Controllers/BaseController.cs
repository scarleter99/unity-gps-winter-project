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
    protected Vector3 _destPos;
    [SerializeField]
    protected GameObject _lockTarget;

    protected Animator anim;

    public Define.AnimState AnimState
    {
        get => _animState;
        set
        {
            _animState = value;
            Random random = new Random();

            bool isPlayer = WorldObjectType == Define.WorldObject.Player;
            int minIndex = 1;
            int maxAttackIndex = isPlayer ? 5 : 3;
            int maxHitIndex = isPlayer ? 3 : 2;
            int maxDieIndex = isPlayer ? 3 : 2;
            int index;
            
            switch (_animState)
            {
                case Define.AnimState.Attack:
                    index = random.Next(minIndex, maxAttackIndex);
                    anim.CrossFade($"Attack{index}", 0.1f);
                    break;
                case Define.AnimState.Defend:
                    anim.CrossFade("Defend", 0.1f);
                    break;
                case Define.AnimState.DefendHit:
                    anim.CrossFade("DefendHit", 0.1f);
                    break;
                case Define.AnimState.Die:
                    index = random.Next(minIndex, maxDieIndex);
                    anim.CrossFade($"Die{index}", 0.1f);
                    break;
                case Define.AnimState.Dizzy:
                    anim.CrossFade("Dizzy", 0.1f);
                    break;
                case Define.AnimState.Hit:
                    index = random.Next(minIndex, maxHitIndex);
                    anim.CrossFade($"Hit{index}", 0.1f);
                    break;
                case Define.AnimState.Idle:
                    anim.CrossFade("Idle", 0.1f);
                    break;
                //case Define.AnimState.Skill:
                //    break;
                case Define.AnimState.Victory:
                    anim.CrossFade("Victory", 0.1f);
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
        anim = GetComponent<Animator>();
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

    public virtual void OnDamage(Stat attackerStat)
    {
        var nextState = (AnimState == Define.AnimState.Idle) ? Define.AnimState.Hit : Define.AnimState.DefendHit;
        AnimState = nextState;
    }
    
    protected virtual void UpdateAttack() { }
    protected virtual void UpdateDefend() { }
    protected virtual void UpdateDefendHit() { }
    protected virtual void UpdateDie() { }
    protected virtual void UpdateDizzy() { }
    protected virtual void UpdateHit() { }
    protected virtual void UpdateIdle() { }
    protected virtual void UpdateVictory() { }
    
    
}
