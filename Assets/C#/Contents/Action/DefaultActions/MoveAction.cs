using UnityEngine;

public class MoveAction : BaseAction
{
    public void SetInfo(Creature owner)
    {
        Owner = owner;
            
        ActionAttribute = Define.ActionAttribute.Move;
        ActionTargetType = Define.ActionTargetType.Single;
    }
    
    public override void HandleAction(BattleGridCell targetCell, int coinHeadNum)
    {
        if (targetCell.CellCreature != null)
            return;
        
        Managers.BattleMng.MoveCreature(Owner, targetCell);
    }
}