using UnityEngine;

public class Flee : BaseAction
{
    public int CoinNum { get; set; }
    
    public void SetInfo(Creature owner)
    {
        Owner = owner;
            
        ActionAttribute = Define.ActionAttribute.Flee;
        ActionTargetType = Define.ActionTargetType.Single;
    }

    public override void HandleAction(BattleGridCell cell)
    {
        // TODO - 구현
        Debug.Log("Flee Debug");
    }
}