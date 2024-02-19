using System.Collections;
using UnityEngine;

public class Strike : BaseSkill
{
    public void SetInfo(int templateId, Creature owner)
    {
        ActionAttribute = Define.ActionAttribute.MeleeAttack;
        ActionTargetType = Define.ActionTargetType.Single;
        
        base.SetInfo(templateId, owner);
    }
    
    public override void HandleAction(BattleGridCell cell)
    {
        if (cell.CellCreature == null)
            return;
        
        Creature targetCreature = cell.CellCreature;
        targetCreature.OnDamage(Owner.CreatureStat.Attack, 1);
    }
}