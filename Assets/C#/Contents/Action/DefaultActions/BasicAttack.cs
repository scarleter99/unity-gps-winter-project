using UnityEngine;

public class BasicAttack : BaseAction
{
    public int CoinNum { get; set; }
    
    public void SetInfo(Creature owner)
    {
        Owner = owner;
            
        ActionAttribute = Define.ActionAttribute.BasicAttack;
        ActionTargetType = Define.ActionTargetType.Single;
    }

    public override void HandleAction(BattleGridCell cell)
    {
        if (cell.CellCreature == null)
            return;
        
        SetApproachTypeIfHero();
        Creature targetCreature = cell.CellCreature;
        targetCreature.OnDamage(Owner.CreatureStat.Attack - Owner.TargetCell.CellCreature.CreatureStat.Defense, 1);
    }

    private void SetApproachTypeIfHero()
    {
        if (Owner.CreatureType != Define.CreatureType.Hero)
            return;

        Hero? hero = Owner as Hero;
        switch (hero.WeaponType)
        {
            case Define.WeaponType.Wand:
            case Define.WeaponType.Bow:
                hero.ApproachType = Define.ApproachType.InPlace;
                break;
            default:
                hero.ApproachType = Define.ApproachType.Jump;
                break;
        }
    }
}