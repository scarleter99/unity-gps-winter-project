public class HealPotion : BaseItem
{
    public Creature Target { get; protected set; }
    
    public override void SetInfo(int templateId, Creature owner, Bag bag, int idx, int addNum)
    {
        ActionAttribute = Define.ActionAttribute.Heal;
        ActionTargetType = Define.ActionTargetType.Single;
        
        base.SetInfo(templateId, owner, bag, idx, addNum);
    }

    public override void HandleAction(ulong targetId)
    {
        if (Managers.ObjectMng.Heroes.TryGetValue(targetId, out Hero hero))
            Target = hero;
        if (Managers.ObjectMng.Monsters.TryGetValue(targetId, out Monster monster))
            Target = monster;
        
        Target.OnHeal(ItemData.Heal);
        
        base.HandleAction(targetId);
    }
}
