using System.Collections.Generic;

public abstract class Equipment
{
    public Dictionary<Define.Stat, int> EquipmentStat { get; protected set; }
    public HeroController Owner { get; protected set; }
    
    public Equipment(HeroController owner)
    {
        EquipmentStat = new Dictionary<Define.Stat, int>();
        Owner = owner;
    }
    
    public abstract void Equip();
    public abstract void UnEquip();
    protected abstract void LoadDataFromJson(string className);
}
