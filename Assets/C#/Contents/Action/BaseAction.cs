public abstract class BaseAction
{
    public int DataId { get; protected set; }
    
    public Define.ActionAttribute ActionAttribute;
    public Define.ActionTargetType ActionTargetType;

    public Creature Owner { get; set; }
    
    public virtual void SetInfo(int templateId, Creature owner, Bag bag, int idx, int addNum)
    {
        Owner = owner;
    }
    
    public abstract void HandleAction(ulong targetId);
}