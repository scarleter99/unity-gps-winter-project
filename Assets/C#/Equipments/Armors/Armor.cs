public abstract class Armor: Equipment
{
    protected Define.ArmorType _armorType;
    protected int _armorIndex;

    public Define.ArmorType ArmorType { get => _armorType; }

    public Armor(PlayerController equipper): base(equipper) { }

    ~Armor()
    {
        UnEquip();
    }

    protected override void LoadDataFromJson(string className)
    {
        var data = Managers.DataMng.ArmorDataDict[className];
        if (data.Hp != 0) StatData.TryAdd(Define.Stat.Hp, data.Hp);
        if (data.Attack != 0) StatData.TryAdd(Define.Stat.Attack, data.Attack);
        if (data.Defense != 0) StatData.TryAdd(Define.Stat.Defense, data.Defense);
        if (data.Speed != 0) StatData.TryAdd(Define.Stat.Speed, data.Speed);
        if (data.Dexterity != 0) StatData.TryAdd(Define.Stat.Dexterity, data.Dexterity);
        if (data.Strength != 0) StatData.TryAdd(Define.Stat.Strength, data.Strength);
        if (data.Vitality != 0) StatData.TryAdd(Define.Stat.Vitality, data.Vitality);
        if (data.Intelligence != 0) StatData.TryAdd(Define.Stat.Intelligence, data.Intelligence);
        if (data.Index != 0) _armorIndex = data.Index;
    }
    
    public override void Equip()
    {
        _equipper.Stat.AttachEquipment(StatData);
        
        if (_armorIndex != 0)
            _equipper.ChangeArmorVisibility(ArmorType, _armorIndex, true);
    }

    public override void UnEquip()
    {
        _equipper.Stat.DetachEquipment(StatData);
        
        if (_armorIndex != 0)
            _equipper.ChangeArmorVisibility(ArmorType, _armorIndex, false);
    }
}
