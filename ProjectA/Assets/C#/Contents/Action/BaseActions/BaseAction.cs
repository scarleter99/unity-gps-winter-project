using UnityEngine;

public abstract class BaseAction
{
    #region Field

    public int DataId { get; protected set; }
    public Data.ActionData ActionData { get; protected set; }
    
    public Creature Owner { get; set; }
    public Animator Animator => Owner.Animator;
    public BattleGridCell TargetCell => Owner.TargetCell;
    
    public Define.ActionTargetType ActionTargetType { get; protected set; }
    public Define.Stat UsingStat { get; protected set; } = Define.Stat.Strength;

    public int CoinNum { get; protected set; }
    public int CoinHeadNum { get; protected set; }

    #endregion
    
    public virtual void SetInfo(int templateId)
    {
        DataId = templateId;
        ActionData = Managers.DataMng.ActionDataDict[templateId];
        
        CoinNum = ActionData.CoinNum;
    }

    public virtual void Equip(Creature owner)
    {
        Owner = owner;
    }

    public int CoinToss()
    {
        if (ActionData.UsingStat == Define.Stat.None)
            return -1;

        int coinHeadNum = 0;
        if (Owner.CreatureType == Define.CreatureType.Hero)
        {
            for (int i = 0; i < CoinNum; i++)
            {
                float value = Random.value;
                if (value < ((Hero)Owner).HeroStat.GetStatByDefine(UsingStat) / 100f)
                    coinHeadNum++;
            }
        }
        else
        {
            for (int i = 0; i < CoinNum; i++)
            {
                float value = Random.value;
                if (value < ((Monster)Owner).MonsterData.Stat / 100f)
                    coinHeadNum++;
            }
        }

        return coinHeadNum;
    }
    
    #region Action

    public abstract void DoAction();
    
    public abstract void OnHandleAction();

    public virtual void OnJumpFWDStart()
    {
    }

    public virtual void OnJumpFWDEnd()
    {
    }
    
    public virtual void OnJumpBWDStart()
    {
    }
    
    public virtual void OnMoveFWDStart()
    {
    }
    
    public virtual void OnMoveFWDEnd()
    {
    }
    
    public virtual void OnMoveBWDStart()
    {
    }
    
    public virtual void OnAttackEnd()
    {
    }
    
    public void OnActionEnd()
    {
        Animator.Play("Idle");
        Owner.DoEndTurn();
    }

    #endregion
    
}