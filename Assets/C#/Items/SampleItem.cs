using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SampleItem : BaseItem
{
    protected override void Init()
    {
        ItemType = Define.ItemType.Buff;
        Name = name;
    }
    
    public override void Use()
    {
        Debug.Log($"Item {Name} description : {Description}");
    }
}
