using System.Collections;
using UnityEngine;

public class Strike : BaseSkill
{
    public Creature Target { get; protected set; }

    public void SetInfo(int templateId)
    {
        ActionAttribute = Define.ActionAttribute.MeleeAttack;
        ActionTargetType = Define.ActionTargetType.Single;
        
        base.SetInfo(templateId);
    }
    
    public override void HandleAction(ulong targetId)
    {
        Target = Managers.ObjectMng.GetCreatureWithId(targetId);
        
        Target.OnDamage(Owner.CreatureStat.Attack, 1);
    }
}