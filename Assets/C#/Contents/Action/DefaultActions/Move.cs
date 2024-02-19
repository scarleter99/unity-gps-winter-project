using UnityEngine;

public class Move : BaseAction
{
    public void SetInfo(Creature owner)
    {
        Owner = owner;
            
        ActionAttribute = Define.ActionAttribute.Move;
        ActionTargetType = Define.ActionTargetType.Single;
    }

    public override void HandleAction(BattleGridCell cell)
    {
        if (cell.CellCreature != null)
            return;
        
        Owner.OnMove(cell);
    }
}