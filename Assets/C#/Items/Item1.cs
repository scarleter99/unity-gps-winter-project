using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item1 : BaseItem
{
    protected override void Init()
    {
        base.Init();
        ItemType = Define.ItemType.Recover;
    }

    public override void Use(BaseController control)
    {
        switch (control.WorldObjectType)
        {
            case Define.WorldObject.Monster:
                (control as MonsterController)?.Stat.RecoverHp(40);
                break;
            case Define.WorldObject.Player:
                (control as PlayerController)?.Stat.RecoverHp(40);
                break;
        }
    }
}
