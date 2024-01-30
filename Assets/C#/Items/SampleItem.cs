using UnityEngine;

public class SampleItem : BaseItem
{
    protected override void Init()
    {
        base.Init();
        ItemType = Define.ItemType.Buff;
    }
    
    public override void Use(BaseController control)
    {
        // debug
        Debug.Log($"Item {Name} description : {Description}");
    }
}
