using UnityEngine;

public class SampleItem : BaseItem
{
    protected override void Init()
    {
        base.Init();
        
        ItemType = Define.ItemType.Buff;
    }
    
    public override void Use(Creature owner)
    {
        // debug
        Debug.Log($"ItemData {ItemData.name} description : {Description}");
    }
}
