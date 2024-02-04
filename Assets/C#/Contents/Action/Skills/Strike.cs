using System.Collections;
using UnityEngine;

public class Strike : BaseSkill
{
    Creature target;
    
    public void SetInfo()
    {
        ActionType = Define.ActionType.MeleeAttack;
        ActionTargetType = Define.ActionTargetType.Single;
        CoinNum = 3;
        ReducedStat = 0;
    }
    
    public override void HandleAction(ulong targetId)
    {
        if (Managers.ObjectMng.Heroes.TryGetValue(targetId, out Hero hero))
            target = hero;
        if (Managers.ObjectMng.Monsters.TryGetValue(targetId, out Monster monster))
            target = monster;
        
        target.OnDamage(Owner);
    }
}