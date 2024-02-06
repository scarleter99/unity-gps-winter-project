public abstract class BaseItem : BaseAction
{
    public Data.ItemData ItemData { get; protected set; }
    public Define.ItemType ItemType { get; protected set; }
    
    public Bag Bag { get; set; }
    public int Idx { get; set; }
    public int Count { get; set; }

    public override void SetInfo(int templateId, Creature owner, Bag bag, int idx, int addNum)
    {
        DataId = templateId;

        ItemData = Managers.DataMng.ItemDataDict[templateId];
        
        Owner = owner;
        Bag = bag;
        Idx = idx;
        Count += addNum;
        
        base.SetInfo(templateId, owner, bag, idx, addNum);
    }
    
    public override void HandleAction(ulong targetId)
    {
        Count--;
        if (Count <= 0)
            Bag.Items[Idx] = null;
    }
}