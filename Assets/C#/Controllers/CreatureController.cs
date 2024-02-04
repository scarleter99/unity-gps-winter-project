using System;
using UnityEngine;
using Random = System.Random;

public abstract class CreatureController : MonoBehaviour
{
    public Animator Animator { get; protected set; }
    
    public ulong Id { get; set; }
    public int DataId { get; protected set; }
    public Define.CreatureType CreatureType { get; protected set; } = Define.CreatureType.None;
    public Data.CreatureData CreatureData { get; protected set; }
    public IStat Stat { get; protected set; }
    public Define.HeroTurnState HeroTurnState { get; protected set; } = Define.HeroTurnState.Wait;
    private Define.AnimState _animState = Define.AnimState.Idle;
    public Define.AnimState AnimState
    {
        get => _animState;
        protected set
        {
            _animState = value;
            Random random = new Random();

            bool isPlayer = CreatureType == Define.CreatureType.Hero;
            int minIndex = 1;
            int maxHitIndex = isPlayer ? 3 : 2;
            int maxDieIndex = isPlayer ? 3 : 2;
            int index;

            string stateName = "";
            switch (_animState)
            {
                case Define.AnimState.Attack:
                    Animator.Play(stateName = "Attack1");
                    break;
                case Define.AnimState.Defend:
                    Animator.Play(stateName = "Defend");
                    break;
                case Define.AnimState.DefendHit:
                    Animator.Play(stateName = "DefendHit");
                    break;
                case Define.AnimState.Die:
                    index = random.Next(minIndex, maxDieIndex);
                    Animator.Play(stateName = $"Die{index}");
                    break;
                case Define.AnimState.Dizzy:
                    Animator.Play(stateName = "Dizzy");
                    break;
                case Define.AnimState.Hit:
                    index = random.Next(minIndex, maxHitIndex);
                    Animator.Play(stateName = $"Hit{index}");
                    break;
                case Define.AnimState.Idle:
                    Animator.CrossFade(stateName = "Idle", 0.2f);
                    break;
                case Define.AnimState.JumpBack:
                    Animator.Play(stateName = "Jump");
                    break;
                case Define.AnimState.JumpFront:
                    Animator.Play(stateName = "Jump");
                    _comebackPos = transform.position;
                    break;
                case Define.AnimState.Move:
                    Animator.Play(stateName = "Move");
                    break;
                //case Define.AnimState.Skill:
                //    break;
                case Define.AnimState.Victory:
                    Animator.Play(stateName = "Victory");
                    break;
            }

            _stateHash = Animator.StringToHash(stateName);
        }
    }
    public int Row { get; set; }
    public int Col { get; set; }

    public BaseAction CurrentAction { get; set; }
    public CreatureController TargetCreature { get; protected set; }
    
    protected int _stateHash;
    protected Vector3 _comebackPos; // jump에서 사용
    
    private void Start()
    {
        Init();
    }
    
    private void Update()
    {
        ChangeAnim();
    }

    protected virtual void Init()
    {
        Animator = GetComponent<Animator>();
    }
    
    // 수동 실행
    public virtual void SetInfo(int templateId)
    {
        DataId = templateId;

        if (CreatureType == Define.CreatureType.Hero)
            CreatureData = Managers.DataMng.HeroDataDict[templateId];
        else
            CreatureData = Managers.DataMng.MonsterDataDict[templateId];

        gameObject.name = $"{CreatureData.dataId}_{CreatureData.name}";
        
        HeroTurnState = Define.HeroTurnState.Wait;
        AnimState = Define.AnimState.Idle;
    }

    protected void ChangeAnim()
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
            case Define.AnimState.JumpBack:
                UpdateJumpBack();
                break;
            case Define.AnimState.JumpFront:
                UpdateJumpFront();
                break;
            case Define.AnimState.Move:
                UpdateMove();
                break;
            //case Define.AnimState.Skill:
            //    break;
            case Define.AnimState.Victory:
                UpdateVictory();
                break;
        }
    }

    public virtual void ChangeStat(IStat statStruct)
    {
        Stat = statStruct;
    }
    
    public virtual void DoAction(ulong targetId)
    {
        if (Managers.ObjectMng.Heroes.TryGetValue(targetId, out HeroController hero))
            TargetCreature = hero;
        if (Managers.ObjectMng.Monsters.TryGetValue(targetId, out MonsterController monster))
            TargetCreature = monster;
        if (TargetCreature == null)
        {
            Debug.Log("Failed to DoAction");
            return;
        }

        switch (CurrentAction.ActionType)
        {
            case Define.ActionType.MeleeAttack:
                // TODO - 애니메이션 실행
                Debug.Log("MeleeAttack");
                break;
        }
    }

    // Animation의 적절한 타이밍에서 호출
    public virtual void HandleAction()
    {
        CurrentAction.HandleAction(TargetCreature.Id);
        TargetCreature = null;
    }

    // TODO - 코인 앞면 수에 비례한 데미지 계산 
    public virtual void OnDamage(CreatureController attacker, int amount = 1)
    {
        Stat.OnDamage(attacker.Stat.Attack, amount);
        // TODO - 피격 애니메이션 실행
    }
    
    #region Update

    protected virtual void UpdateAttack() { }
    protected virtual void UpdateDefend() { }
    protected virtual void UpdateDefendHit() { }
    
    protected virtual void UpdateDie()
    {
        var currentState = Animator.GetCurrentAnimatorStateInfo(0);
        // if (currentState.normalizedTime >= 0.98f && currentState.shortNameHash == _stateHash)
        //     Managers.ObjectMng.Despawn(this.gameObject);
    }
    
    protected virtual void UpdateDizzy() { }
    protected virtual void UpdateHit() { }
    protected virtual void UpdateIdle() { }

    protected virtual void UpdateJumpBack()
    {
        var currentState = Animator.GetCurrentAnimatorStateInfo(0);
        if (currentState.normalizedTime >= 0.8f && currentState.shortNameHash == _stateHash)
            AnimState = Define.AnimState.Idle;
    }
    
    protected virtual void UpdateJumpFront()
    {
        var currentState = Animator.GetCurrentAnimatorStateInfo(0);
        if (currentState.normalizedTime >= 0.98f && currentState.shortNameHash == _stateHash)
        {
            /* TODO - 현재 오류 발생
            var nextAct = (Managers.SceneMng.CurrentScene as BattleScene)?.BattleManager.ActionType;
            switch (nextAct)
            {
                case Define.ActionType.Attack:
                    AnimState = Define.AnimState.Attack;
                    break;
                case Define.ActionType.SkillUse:
                    AnimState = Define.AnimState.Skill;
                    break;
            }
            */
        }
    }
    
    protected virtual void UpdateMove() { }
    protected virtual void UpdateVictory() { }

    #endregion
}
