using System.Collections.Generic;

public abstract class Weapon
{
    protected Define.WeaponType _weaponType;
    protected PlayerController _equipper;
    
    public Dictionary<string, int> StatData { get; } // TODO : Network 쪽에서 enum Stat 만들어지면 교체
    public Define.WeaponType WeaponType { get => _weaponType; }
    
    public abstract void EffectAfterAttack(PlayerController controller);
    public abstract void Skill1(PlayerController controller);
    public abstract void Skill2(PlayerController controller);

    public Weapon(PlayerController equipper)
    {
        StatData = new Dictionary<string, int>();
        _equipper = equipper;
    }
    
    public virtual void Equip()
    {
        _equipper.Stat.AttachEquipment(StatData);
    }

    public virtual void UnEquip()
    {
        _equipper.Stat.DetachEquipment(StatData);
    }
}