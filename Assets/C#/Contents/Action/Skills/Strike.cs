using System.Collections;
using UnityEngine;

public class Strike : BaseSkill
{
    public void SetInfo(int templateId, Creature owner)
    {
        ActionAttribute = Define.ActionAttribute.JumpAttack;
        ActionTargetType = Define.ActionTargetType.Single;
        
        base.SetInfo(templateId, owner);
    }
    
    public override void HandleAction(BattleGridCell targetCell)
    {
        if (targetCell.CellCreature == null)
            return;
        
        Creature targetCreature = targetCell.CellCreature;
        targetCreature.OnDamage(Owner.CreatureStat.Attack, 1);
    }
}