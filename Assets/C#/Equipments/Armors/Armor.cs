using System.Collections.Generic;

public abstract class Armor
{
    protected Define.ArmorType _armorType;
    protected PlayerController _equipper;
    protected int _armorIndex;

    public Dictionary<Define.Stat, int> StatData { get; protected set; }
    public Define.ArmorType ArmorType { get => _armorType; }

    public Armor(PlayerController equipper)
    {
        StatData = new Dictionary<Define.Stat, int>();
        _equipper = equipper;
    }
    
    public void Equip()
    {
        _equipper.Stat.AttachEquipment(StatData);
        
        if (_armorIndex != 0)
            _equipper.ChangeArmorVisibility(ArmorType, _armorIndex, true);
    }

    public void UnEquip()
    {
        _equipper.Stat.DetachEquipment(StatData);
        
        if (_armorIndex != 0)
            _equipper.ChangeArmorVisibility(ArmorType, _armorIndex, false);
    }
}
