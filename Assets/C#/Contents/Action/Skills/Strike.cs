public class Strike : BaseSkill
{
    public override void SetInfo(int templateId, Creature owner)
    {
        ActionAttribute = Define.ActionAttribute.AttackSkill;
        ActionTargetType = Define.ActionTargetType.Single;
        
        base.SetInfo(templateId, owner);
    }
    
    public override void HandleAction(BattleGridCell targetCell, int coinHeadNum)
    {
        if (targetCell.CellCreature == null)
            return;
        
        Creature targetCreature = targetCell.CellCreature;
        targetCreature.OnDamage(Owner.CreatureStat.Attack * (coinHeadNum / CoinNum), 1);
    }
}
