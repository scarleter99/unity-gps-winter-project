using UnityEngine;

public abstract class BaseAction
{
    public Define.ActionAttribute ActionAttribute { get; protected set; }
    public Define.ActionTargetType ActionTargetType { get; protected set; }
    public Define.Stat UsingStat { get; protected set; } = Define.Stat.Strength;

    public int CoinNum { get; protected set; } = 3;

    public Creature Owner { get; set; }

    public int CoinToss()
    {
        Hero hero = Owner as Hero;
        if (hero == null)
            return -1;
        
        int successCount = 0;
        for (int i = 0; i < CoinNum; i++)
        {
            float val = Random.value;
            if (val < hero.HeroStat.GetStatByDefine(UsingStat) / 100f)
                successCount++;
        }
        
        return successCount;
    }
    
    public abstract void HandleAction(BattleGridCell targetCell, int coinHeadNum);
}