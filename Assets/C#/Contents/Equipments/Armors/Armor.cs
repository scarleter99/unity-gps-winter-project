public abstract class Armor: Equipment
{
    protected Define.ArmorType _armorType;
    protected int _armorIndex;

    public Define.ArmorType ArmorType { get => _armorType; }

    public Armor(HeroController equipper): base(equipper) { }

    ~Armor()
    {
        UnEquip();
    }

    protected override void LoadDataFromJson(string className)
    {
        var data = Managers.DataMng.ArmorDataDict[className];
        if (data.Hp != 0) EquipmentStat.TryAdd(Define.Stat.Hp, data.Hp);
        if (data.Attack != 0) EquipmentStat.TryAdd(Define.Stat.Attack, data.Attack);
        if (data.Defense != 0) EquipmentStat.TryAdd(Define.Stat.Defense, data.Defense);
        if (data.Speed != 0) EquipmentStat.TryAdd(Define.Stat.Speed, data.Speed);
        if (data.Dexterity != 0) EquipmentStat.TryAdd(Define.Stat.Dexterity, data.Dexterity);
        if (data.Strength != 0) EquipmentStat.TryAdd(Define.Stat.Strength, data.Strength);
        if (data.Vitality != 0) EquipmentStat.TryAdd(Define.Stat.Vitality, data.Vitality);
        if (data.Intelligence != 0) EquipmentStat.TryAdd(Define.Stat.Intelligence, data.Intelligence);
        if (data.Index != 0) _armorIndex = data.Index;
    }
    
    public override void Equip()
    {
        Owner.Stat.AttachEquipment(EquipmentStat);
        
        if (_armorIndex != 0)
            Owner.ChangeArmorVisibility(ArmorType, _armorIndex, true);
    }

    public override void UnEquip()
    {
        Owner.Stat.DetachEquipment(EquipmentStat);
        
        if (_armorIndex != 0)
            Owner.ChangeArmorVisibility(ArmorType, _armorIndex, false);
    }
}
