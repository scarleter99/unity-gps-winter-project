using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : Creature
{
    #region Field
    public Data.MonsterData MonsterData => CreatureData as Data.MonsterData;
    public MonsterStat MonsterStat => (MonsterStat)CreatureStat;
    
    public UI_CoinToss CoinTossUI { get; protected set; }
    #endregion
    
    protected override void Init()
    {
        base.Init();

        // TODO - TEST CODE
        Managers.InputMng.KeyAction -= OnKeyboardClick;
        Managers.InputMng.KeyAction += OnKeyboardClick;
    }
    
    public override void SetInfo(int templateId)
    {
        CreatureType = Define.CreatureType.Monster;
        
        base.SetInfo(templateId);
    }

    #region Battle
    public override void DoSelectAction()
    {
        CoinTossUI = Managers.UIMng.MakeSubItemUI<UI_CoinToss>();
        
        // TODO - Action 선택 알고리즘 구현
        BaseSkill skill = new Strike();
        skill.SetInfo(Define.SKILL_STRIKE_ID, this);
        CurrentAction = skill;

        Managers.BattleMng.BattleState = Define.BattleState.SelectTarget;
    }
    
    public override void DoSelectTarget()
    {
        TargetCell = GetRandomHeroCell(); // TODO - Target Hero 선택 알고리즘 구현
        Managers.BattleMng.BattleState = Define.BattleState.ActionProceed;
    }
    
    public override void DoAction()
    {
        CoinHeadNum = 0;
        CoinHeadNum = CurrentAction.CoinToss();
        CoinTossUI.ShowCoinToss(CurrentAction, CoinHeadNum);
        
        switch (CurrentAction.ActionAttribute)
        {
            case Define.ActionAttribute.AttackSkill:
                AnimState = Define.AnimState.Attack;
                break;
            case Define.ActionAttribute.Move:
                OnMove(TargetCell);
                break;
        }
    }

    public override void DoEndTurn()
    {
        Managers.ResourceMng.Destroy(CoinTossUI.gameObject);
    }
    #endregion
    
    BattleGridCell GetRandomHeroCell()
    {
        List<ulong> keysList = new List<ulong>(Managers.ObjectMng.Heroes.Keys);
        ulong randomKey = keysList[Random.Range(0, keysList.Count)];

        return Managers.ObjectMng.Heroes[randomKey].Cell;
    }

    /*----------------------
        TODO - TEST CODE
    ----------------------*/
    protected void OnKeyboardClick()
    {
        if (Input.GetKeyDown(KeyCode.A))
            AnimState = Define.AnimState.Attack;
    }
}
