using UnityEngine;

public class FleeAction : BaseAction
{
    public int CoinNum { get; set; }
    
    public void SetInfo(Creature owner)
    {
        Owner = owner;
            
        ActionAttribute = Define.ActionAttribute.Flee;
        ActionTargetType = Define.ActionTargetType.Single;
        CoinNum = 3;
    }

    public override void HandleAction(BattleGridCell cell, int coinHeadNum)
    {
        // TODO - 구현
        Debug.Log("Flee Debug");
    }
}