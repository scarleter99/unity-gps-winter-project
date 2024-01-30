using UnityEngine;

public class MonsterController : CreatureController
{
    protected override void Init()
    {
        base.Init();
        
        CreatureType = Define.CreatureType.Monster;

        // TODO - TEST CODE
        Managers.InputMng.KeyAction -= OnKeyboard;
        Managers.InputMng.KeyAction += OnKeyboard;
    }
    
    public MonsterStat GetMonsterStat()
    {
        MonsterStat monsterStat = (MonsterStat)Stat;
        return monsterStat;
    }
    
    #region Event
    
    public override void OnDamage(CreatureController attacker, int amount = 1)
    {
        var nextState = (AnimState == Define.AnimState.Defend) ? Define.AnimState.DefendHit : Define.AnimState.Hit;
        var playerAttacker = attacker as HeroController;
        Stat.OnDamage(playerAttacker.Stat.Attack, amount);
        nextState = (Stat.Hp > 0) ? nextState : Define.AnimState.Die;
        AnimState = nextState;
    }
    
    // 적절한 Animation Timing에서 호출
    protected override void OnAttackEvent()
    {
        if (LockTarget != null)
            LockTarget.GetComponent<CreatureController>().OnDamage(this);
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
        var currentState = Animator.GetCurrentAnimatorStateInfo(0);
        if (currentState.normalizedTime >= 0.8f && currentState.shortNameHash == _stateHash)
            AnimState = Define.AnimState.Idle;
    }

    protected override void UpdateHit()
    {
        var currentState = Animator.GetCurrentAnimatorStateInfo(0);
        if (currentState.normalizedTime >= 0.8f && currentState.shortNameHash == _stateHash)
            AnimState = Define.AnimState.Idle;
    }
    
    #endregion
    
    /*----------------------
        TODO - TEST CODE
    ----------------------*/
    protected void OnKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.A))
            AnimState = Define.AnimState.Attack;
    }
}
