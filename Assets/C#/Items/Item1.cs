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
                var monsterStat = (control as MonsterController)?.Stat;
                monsterStat?.RecoverHp(40);
                break;
            case Define.WorldObject.Player:
                var playerStat = (control as PlayerController)?.Stat;
                playerStat?.RecoverHp(40);
                break;
        }
    }
}
