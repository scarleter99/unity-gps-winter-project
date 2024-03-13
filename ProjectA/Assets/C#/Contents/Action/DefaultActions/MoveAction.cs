using UnityEngine;

public class MoveAction : BaseAction
{
    public override void SetInfo(int templateId)
    {
        base.SetInfo(templateId);
        
        ActionTargetType = Define.ActionTargetType.Single;
    }
    
    public override void DoAction()
    {
        // TODO
    }
    
    public override void OnHandleAction()
    {
        if (TargetCell.CellCreature != null)
            return;
        
        Managers.BattleMng.MoveCreature(Owner, TargetCell);
    }
}