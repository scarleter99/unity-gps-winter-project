using System.Collections;
using UnityEngine;

public abstract class BaseAction
{
    public Define.ActionType ActionType;
    public Define.ActionTargetType ActionTargetType = Define.ActionTargetType.Single;

    protected Coroutine DoActionCo;

    public Creature Owner { get; set; }
    
    public abstract void HandleAction(ulong targetId);
}