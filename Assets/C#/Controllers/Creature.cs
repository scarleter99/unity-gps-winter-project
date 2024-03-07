using System.Collections;
using System.Text;
using DG.Tweening;
using UnityEngine;
using Random = System.Random;

public abstract class Creature : MonoBehaviour
{
    #region Field
    public Animator Animator { get; protected set; }
    
    public ulong Id { get; set; }
    public int DataId { get; protected set; }
    public Define.CreatureType CreatureType { get; protected set; }
    public Data.CreatureData CreatureData { get; protected set; }
    public CreatureStat CreatureStat { get; protected set; }

    private Define.CreatureBattleState _creatureBattleState;
    public Define.CreatureBattleState CreatureBattleState
    {
        get => _creatureBattleState;
        set
        {
            switch (value)
            {
                case Define.CreatureBattleState.Wait:
                    break;
                case Define.CreatureBattleState.SelectAction:
                    DoSelectAction();
                    break;
                case Define.CreatureBattleState.SelectTarget:
                    DoSelectTarget();
                    break;
                case Define.CreatureBattleState.ActionProceed:
                    DoAction();
                    break;
                case Define.CreatureBattleState.Dead:
                    break;
            }
        }
    }
    
    public BattleGridCell Cell { get; set; }

    public BaseAction CurrentAction { get; set; }
    public BattleGridCell TargetCell { get; protected set; }
    public int CoinHeadNum { get; protected set; }
    
    public MoveAction MoveAction { get; protected set; }
    public FleeAction FleeAction { get; protected set; }
    #endregion
    
    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        Animator = GetComponent<Animator>();
        CreatureStat = GetComponent<CreatureStat>();
        MoveAction = new MoveAction();
        MoveAction.SetInfo(this);
        FleeAction = new FleeAction();
        FleeAction.SetInfo(this);
    }
    
    // 수동 실행
    public virtual void SetInfo(int templateId)
    {
        DataId = templateId;

        if (CreatureType == Define.CreatureType.Hero)
            CreatureData = Managers.DataMng.HeroDataDict[templateId];
        else
            CreatureData = Managers.DataMng.MonsterDataDict[templateId];

        gameObject.name = $"{CreatureData.DataId}_{CreatureData.Name}";
        
        CreatureStat.SetStat(CreatureData);
        CreatureBattleState = Define.CreatureBattleState.Wait;
        AnimState = Define.AnimState.Idle;
        
        CoinHeadNum = 0;
    }
    
    #region Animation
    
    // 공격하기 전 접근 단계에서 사용
    public Define.ApproachType ApproachType { get; set; }
    protected Vector3 _comebackPos;
    protected bool _moveDOTriggered;
    [SerializeField] protected Vector3 _approachOffset;
    
    // randomly selected states
    protected StringBuilder _stringAttack = new ("Attack");
    protected StringBuilder _stringDie = new ("Die");
    protected StringBuilder _stringHit = new ("Hit");
    
    protected Define.AnimState _animState;
    public virtual Define.AnimState AnimState
    {
        get => _animState;
        protected set
        {
            if (!Animator)
                return;
            
            switch (value)
            {
                case Define.AnimState.Attack:
                    ApproachBeforeAttack();
                    break;
                case Define.AnimState.Defend:
                    Animator.Play(Define.ANIMATION_DEFEND);
                    break;
                case Define.AnimState.DefendHit:
                    Animator.Play(Define.ANIMATION_DEFEND_HIT);
                    break;
                case Define.AnimState.Die:
                    PlayRandomAnimation(value);
                    break;
                case Define.AnimState.Dizzy:
                    Animator.Play(Define.ANIMATION_DIZZY);
                    break;
                case Define.AnimState.Hit:
                    PlayRandomAnimation(value);
                    break;
                case Define.AnimState.Idle:
                    Animator.Play(Define.ANIMATION_IDLE);
                    break;
                case Define.AnimState.Move:
                    Animator.Play(Define.ANIMATION_MOVE);
                    break;
                //case Define.AnimState.Skill:
                //    break;
                case Define.AnimState.Victory:
                    Animator.Play(Define.ANIMATION_VICTORY);
                    break;
            }

            _animState = value;
        }
    }

    protected void ApproachBeforeAttack()
    {
        // TODO - TEST CODE
        _stringAttack.Append("1");
        
        Animator.SetBool(Define.PARAMETER_ATTACK_FINISHED, false);
        Animator.SetBool(Define.PARAMETER_APPROACH_FINISHED, false);
        _comebackPos = transform.position;
        _moveDOTriggered = false;
        
        switch (ApproachType)
        {
            case Define.ApproachType.Jump:
                Animator.SetBool(Define.PARAMETER_NEEDS_JUMP, true);
                Animator.SetBool(Define.PARAMETER_NEEDS_MOVE, false);
                Animator.Play(Define.ANIMATION_JUMP);
                break;
            case Define.ApproachType.InPlace:
                Animator.SetBool(Define.PARAMETER_NEEDS_JUMP, false);
                Animator.SetBool(Define.PARAMETER_NEEDS_MOVE, false);
                Animator.Play(_stringAttack.ToString());
                break;
            case Define.ApproachType.Move:
                Animator.SetBool(Define.PARAMETER_NEEDS_JUMP, false);
                Animator.SetBool(Define.PARAMETER_NEEDS_MOVE, true);
                Animator.Play(Define.ANIMATION_MOVEAPPROACH);
                break;
        }
    }

    protected void PlayRandomAnimation(Define.AnimState state)
    {
        Random random = new Random();

        bool isPlayer = CreatureType == Define.CreatureType.Hero;
        int maxIndex = isPlayer ? 3 : 2;
        int index = random.Next(1, maxIndex);
        
        switch (state)
        {
            case Define.AnimState.Die:
                _stringDie.Append(index.ToString());
                Animator.Play(_stringDie.ToString());
                _stringDie.Remove(_stringDie.Length - 1, 1);
                break;
            case Define.AnimState.Hit:
                _stringHit.Append(index.ToString());
                Animator.Play(_stringHit.ToString());
                _stringHit.Remove(_stringHit.Length - 1, 1);
                break;
        }
    }
    
    private bool _isReturn;
    public virtual void OnApproachStart()
    {
        if (!_moveDOTriggered)
        {
            _moveDOTriggered = true;
            float duration = ApproachType == Define.ApproachType.Jump ? 0.433f : 0.8f;
            if (Animator.GetBool(Define.PARAMETER_ATTACK_FINISHED))
            {
                transform.DOMove(_comebackPos, duration)
                    .OnComplete(() => { _stringAttack.Remove(6, 1); Animator.SetBool(Define.PARAMETER_APPROACH_FINISHED, true); });
            }
            else
            {
                transform.DOMove(TargetCell.CellCreature.transform.position + _approachOffset, duration)
                    .OnComplete(() => { Animator.SetTrigger(_stringAttack.ToString()); });
            }
        }
    }

    public virtual void OnHandleAction()
    {
        CurrentAction.HandleAction(TargetCell, CoinHeadNum);
        CoinHeadNum = 0;
        _isReturn = true;
    }
    
    public virtual void OnAttackEnd()
    {
        _moveDOTriggered = false;
        Animator.SetBool(Define.PARAMETER_ATTACK_FINISHED, true);
        Animator.SetBool(Define.PARAMETER_APPROACH_FINISHED, false);
    }

    public virtual void OnReturnEnd()
    {
        if (!_isReturn)
            return;

        _isReturn = false;
        CreatureBattleState = Define.CreatureBattleState.Wait;
        TargetCell = null;
        Managers.BattleMng.NextTurn();
    }
    #endregion

    #region Battle

    public abstract void DoSelectAction();

    public abstract void DoSelectTarget();

    public abstract void DoAction();

    public abstract void DoEndTurn();
    #endregion
    
    #region Event
    public virtual void OnChangeStat(CreatureStat creatureStatStruct)
    {
        CreatureStat = creatureStatStruct;
    }
    
    // TODO - 코인 앞면 수에 비례한 데미지 계산 
    public void OnDamage(int damage, int attackCount = 1)
    {
        CreatureStat.OnDamage(damage, attackCount);
        
        if (CreatureStat.Hp <= 0)
        {
            OnDead();
            return;
        }

        AnimState = Define.AnimState.Hit;
    }
    
    public void OnDead()
    {
        CreatureBattleState = Define.CreatureBattleState.Dead;
        AnimState = Define.AnimState.Die;
    }

    public void OnHeal(int heal)
    {
        CreatureStat.OnHeal(heal);

        // TODO - 회복 애니메이션 실행
    }

    public void OnMove(BattleGridCell cell)
    {
        Managers.BattleMng.ReplaceCreature(this, cell);
        Cell = cell;
        
        StartCoroutine("CoLerpToCell", cell);
    }
    #endregion
    
    IEnumerator CoLerpToCell(BattleGridCell cell)
    {
        while ((transform.position - cell.transform.position).magnitude > 0.001)
        {
            transform.position = Vector3.MoveTowards(transform.position, cell.transform.position, Define.MOVE_SPEED * Time.deltaTime);

            yield return new WaitForSeconds(0.01f);
        }

        transform.position = cell.transform.position;
        CurrentAction.HandleAction(TargetCell, CoinHeadNum);
        yield return null;
    }
}
