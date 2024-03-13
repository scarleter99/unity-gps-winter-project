using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : Creature
{
    #region Field
    
    public Data.MonsterData MonsterData => CreatureData as Data.MonsterData;
    public MonsterStat MonsterStat => (MonsterStat)CreatureStat;
    
    public UI_CoinToss CoinTossUI { get; protected set; }
    
    #endregion

    public override void SetInfo(int templateId)
    {
        CreatureType = Define.CreatureType.Monster;
        
        base.SetInfo(templateId);
    }

    #region Battle

    public override void DoPrepareAction()
    {
        // TODO - Action 선택 알고리즘 구현
        BaseAction action = new Bite();
        action.SetInfo(Define.ACTION_BITE_ID);
        action.Equip(this);
        CurrentAction = action;

        TargetCell = GetRandomHeroCell(); // TODO - Target Hero 선택 알고리즘 구현
        CreatureBattleState = Define.CreatureBattleState.ActionProceed;
    }

    public override void DoAction()
    {
        CoinHeadNum = 0;
        CoinHeadNum = CurrentAction.CoinToss();
        ((UI_BattleScene)Managers.UIMng.SceneUI).CoinTossUI.ShowCoinToss(CurrentAction, CoinHeadNum);
        
        CurrentAction.DoAction();
    }

    public override void DoEndTurn()
    {
        ((UI_BattleScene)Managers.UIMng.SceneUI).CoinTossUI.EndTurn();
        CreatureBattleState = Define.CreatureBattleState.Wait;
        TargetCell = null;
        Managers.BattleMng.NextTurn();
    }
    
    #endregion
    
    BattleGridCell GetRandomHeroCell()
    {
        List<ulong> keysList = new List<ulong>(Managers.ObjectMng.Heroes.Keys);
        ulong randomKey = keysList[Random.Range(0, keysList.Count)];

        return Managers.ObjectMng.Heroes[randomKey].Cell;
    }
}
