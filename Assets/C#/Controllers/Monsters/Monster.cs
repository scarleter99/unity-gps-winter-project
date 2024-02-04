using UnityEngine;

public class Monster : Creature
{
    public Data.MonsterData MonsterData => CreatureData as Data.MonsterData;
    public MonsterStat MonsterStat => (MonsterStat)Stat;
    
    protected override void Init()
    {
        base.Init();
        
        CreatureType = Define.CreatureType.Monster;

        // TODO - TEST CODE
        Managers.InputMng.KeyAction -= OnKeyboardClick;
        Managers.InputMng.KeyAction += OnKeyboardClick;
    }
    
    public override void SetInfo(int templateId)
    {
        base.SetInfo(templateId);
        
        Stat = new MonsterStat(MonsterData);
    }
    
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
    protected void OnKeyboardClick()
    {
        if (Input.GetKeyDown(KeyCode.A))
            AnimState = Define.AnimState.Attack;
    }
}
