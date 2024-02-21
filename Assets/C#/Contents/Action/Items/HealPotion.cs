public class HealPotion : BaseItem
{
    public Creature Target { get; protected set; }
    
    public override void SetInfo(int templateId, Creature owner, Bag bag, int idx, int addNum)
    {
        ActionAttribute = Define.ActionAttribute.HealItem;
        ActionTargetType = Define.ActionTargetType.Single;
        
        base.SetInfo(templateId, owner, bag, idx, addNum);
    }

    public override void HandleAction(BattleGridCell cell)
    {
        if (cell.CellCreature == null)
            return;
        
        Creature targetCreature = cell.CellCreature;
        targetCreature.OnHeal(ItemData.Heal);
        
        base.HandleAction(cell);
    }
}
