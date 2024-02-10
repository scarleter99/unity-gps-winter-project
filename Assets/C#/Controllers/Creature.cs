using System.Linq.Expressions;
using System.Text;
using DG.Tweening;
using UnityEngine;
using Random = System.Random;

public abstract class Creature : MonoBehaviour
{
    public Animator Animator { get; protected set; }
    
    public ulong Id { get; set; }
    public int DataId { get; protected set; }
    public Define.CreatureType CreatureType { get; protected set; }
    public Data.CreatureData CreatureData { get; protected set; }
    public IStat CreatureStat { get; protected set; }
    
    public Define.CreatureBattleState CreatureBattleState { get; set; }
    public int Row { get; set; }
    public int Col { get; set; }

    public BaseAction CurrentAction { get; set; }
    public Creature TargetCreature { get; protected set; }

    private void Start()
    {
        Init();
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
        
        CreatureBattleState = Define.CreatureBattleState.Wait;
        AnimState = Define.AnimState.Idle;
    }

    public virtual void ChangeStat(IStat statStruct)
    {
        CreatureStat = statStruct;
    }
    
    // 턴제에서 본인 턴일 때 사용할 함수
    public virtual void DoAction(ulong targetId)
    {
        if (Managers.ObjectMng.Heroes.TryGetValue(targetId, out Hero hero))
            TargetCreature = hero;
        if (Managers.ObjectMng.Monsters.TryGetValue(targetId, out Monster monster))
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
    
    #region AnimationControl
    
    // 공격하기 전 접근 단계에서 사용
    protected Define.ApproachType _approachType;
    protected Vector3 _comebackPos;
    protected bool _moveDOTriggered;
    [SerializeField] protected Vector3 _approachOffset;

    // animator controller bool hash
    protected static readonly int _hashbFAttack = UnityEngine.Animator.StringToHash("AttackFinished");
    protected static readonly int _hashbFMove = UnityEngine.Animator.StringToHash("MoveFinished");
    protected static readonly int _hashbNJump = UnityEngine.Animator.StringToHash("NeedsJump");
    protected static readonly int _hashbNMove = UnityEngine.Animator.StringToHash("NeedsMove");
    
    // state hash
    protected static readonly int _hashAttack = UnityEngine.Animator.StringToHash("Attack1");
    protected static readonly int _hashDefend = UnityEngine.Animator.StringToHash("Defend");
    protected static readonly int _hashDefendHit = UnityEngine.Animator.StringToHash("DefendHit");
    protected static readonly int _hashDizzy = UnityEngine.Animator.StringToHash("Dizzy");
    protected static readonly int _hashIdle = UnityEngine.Animator.StringToHash("Idle");
    protected static readonly int _hashJump = UnityEngine.Animator.StringToHash("Jump");
    protected static readonly int _hashMove = UnityEngine.Animator.StringToHash("Move");
    protected static readonly int _hashMoveAttack = UnityEngine.Animator.StringToHash("MoveBeforeAttack");
    protected static readonly int _hashVictory = UnityEngine.Animator.StringToHash("Victory");
    
    // randomly selected states
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
                    Animator.Play(_hashDefend);
                    break;
                case Define.AnimState.DefendHit:
                    Animator.Play(_hashDefendHit);
                    break;
                case Define.AnimState.Die:
                    PlayRandomAnimation(value);
                    break;
                case Define.AnimState.Dizzy:
                    Animator.Play(_hashDizzy);
                    break;
                case Define.AnimState.Hit:
                    PlayRandomAnimation(value);
                    break;
                case Define.AnimState.Idle:
                    Animator.Play(_hashIdle);
                    break;
                case Define.AnimState.Move:
                    Animator.Play(_hashMove);
                    break;
                //case Define.AnimState.Skill:
                //    break;
                case Define.AnimState.Victory:
                    Animator.Play(_hashVictory);
                    break;
            }

            _animState = value;
        }
    }

    protected void ApproachBeforeAttack()
    {
        Animator.SetBool(_hashbFAttack, false);
        _comebackPos = transform.position;
        _moveDOTriggered = false;
        
        switch (_approachType)
        {
            case Define.ApproachType.Jump:
                Animator.SetBool(_hashbNJump, true);
                Animator.SetBool(_hashbNMove, false);
                Animator.Play(_hashJump);
                break;
            case Define.ApproachType.InPlace:
                Animator.SetBool(_hashbNJump, false);
                Animator.SetBool(_hashbNMove, false);
                Animator.Play(_hashAttack);
                break;
            case Define.ApproachType.Move:
                Animator.SetBool(_hashbNJump, false);
                Animator.SetBool(_hashbNMove, true);
                Animator.Play(_hashMoveAttack);
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
                _stringDie.Remove(_stringDie.Length - 2, 1);
                break;
            case Define.AnimState.Hit:
                _stringHit.Append(index.ToString());
                Animator.Play(_stringHit.ToString());
                _stringHit.Remove(_stringHit.Length - 2, 1);
                break;
        }
    }
    
    #endregion

    #region Event
    
    public virtual void OnHandleAction()
    {
        CurrentAction.HandleAction(TargetCreature.Id);
        CreatureBattleState = Define.CreatureBattleState.Wait;
        
        TargetCreature = null;
        
        Managers.BattleMng.NextTurn();
    }

    public void OnApproachStart()
    {
        if (!_moveDOTriggered)
        {
            _moveDOTriggered = true;
            float duration = _approachType == Define.ApproachType.Jump ? 0.433f : 0.8f;
            if (Animator.GetBool(_hashbFAttack))
            {
                transform.DOMove(_comebackPos, duration)
                    .OnComplete(() => { Animator.SetBool(_hashbFMove, true); });
            }
            else
            {
                transform.DOMove(TargetCreature.transform.position + _approachOffset, duration)
                    .OnComplete(() => { Animator.SetBool(_hashbFMove, true); });
            }
        }
    }

    public void OnAttackEnd()
    {
        _moveDOTriggered = false;
        Animator.SetBool(_hashbFAttack, true);
    }

    // TODO - 코인 앞면 수에 비례한 데미지 계산 
    public virtual void OnDamage(Creature attacker, int amount = 1)
    {
        CreatureStat.OnDamage(attacker.CreatureStat.Attack, amount);
        if (CreatureStat.Hp <= 0)
        {
            OnDead();
            return;
        }

        AnimState = Define.AnimState.Hit;
    }
    
    public virtual void OnDead()
    {
        CreatureBattleState = Define.CreatureBattleState.Dead;
        AnimState = Define.AnimState.Die;
    }
    

    #endregion
}
