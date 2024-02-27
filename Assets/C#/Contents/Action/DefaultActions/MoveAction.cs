using UnityEngine;

public class MoveAction : BaseAction
{
    public void SetInfo(Creature owner)
    {
        Owner = owner;
            
        ActionAttribute = Define.ActionAttribute.Move;
        ActionTargetType = Define.ActionTargetType.Single;
    }

    public override void HandleAction(BattleGridCell targetCell)
    {
        if (targetCell.CellCreature != null)
            return;
        
        Managers.BattleMng.PlaceCreature(Owner, targetCell);
        Owner.OnMove(targetCell);
    }
}