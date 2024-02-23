public abstract class BaseAction
{
    public Define.ActionAttribute ActionAttribute;
    public Define.ActionTargetType ActionTargetType;

    public Creature Owner { get; set; }
    
    public abstract void HandleAction(BattleGridCell targetCell);
}