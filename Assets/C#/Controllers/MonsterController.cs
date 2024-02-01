using UnityEngine;

public class MonsterController : BaseController
{
    [ReadOnly(false), SerializeField]
    protected MonsterStat _stat;
    public ref MonsterStat Stat { get => ref _stat; }
    
    public override void Init()
    {
        base.Init();
        WorldObjectType = Define.WorldObject.Monster;
        _stat = new MonsterStat(gameObject.name);
        
    //////////////////////////////////////////
    // TODO - TEST CODE
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
    
    #region Event
    
    public override void OnDamage(BaseController attacker, int amount = 1)
    {
        var nextState = (AnimState == Define.AnimState.Defend) ? Define.AnimState.DefendHit : Define.AnimState.Hit;
        var playerAttacker = attacker as PlayerController;
        Stat.OnDamage(playerAttacker.Stat.Attack, amount);
        nextState = (_stat.Hp > 0) ? nextState : Define.AnimState.Die;
        AnimState = nextState;
    }
    
    // 적절한 Animation Timing에서 호출
    protected override void OnAttackEvent()
    {
        if (_lockTarget != null)
            _lockTarget.GetComponent<BaseController>().OnDamage(this);
    }
    
    // 적절한 Animation Timing에서 호출
    protected override void OnJumpStart()
    {
        // TODO
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
