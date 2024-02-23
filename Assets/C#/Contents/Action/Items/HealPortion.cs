public class HealPortion : BaseItem
{
    public Creature Target { get; protected set; }
    
    public override void SetInfo(int templateId, Creature owner, Bag bag, int idx, int addNum)
    {
        ActionAttribute = Define.ActionAttribute.Heal;
        ActionTargetType = Define.ActionTargetType.Single;
        
        base.SetInfo(templateId, owner, bag, idx, addNum);
    }

    public override void HandleAction(BattleGridCell targetCell)
    {
        if (targetCell.CellCreature == null)
            return;
        
        Creature targetCreature = targetCell.CellCreature;
        targetCreature.OnHeal(ItemData.Heal);
        
        base.HandleAction(targetCell);
    }
}
