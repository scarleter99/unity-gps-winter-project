using System.Collections;
using UnityEngine;

public abstract class BaseAction
{
    public Define.ActionType ActionType;
    public Define.ActionTargetType ActionTargetType = Define.ActionTargetType.Single;

    public Creature Owner { get; set; }

    public BaseAction()
    {
        
    }
    
    public abstract void HandleAction(ulong targetId);
}