public class Item1 : BaseItem
{
    protected override void Init()
    {
        base.Init();
        
        ItemType = Define.ItemType.Recover;
    }

    public override void Use(Creature owner)
    {
        owner.CreatureStat.RecoverHp(40);
    }
}
