public abstract class Weapon: Equipment
{
    protected Define.WeaponType _weaponType;
    protected int _leftWeaponIndex;
    protected int _rightWeaponIndex;
    
    public Define.WeaponType WeaponType { get => _weaponType; }
    
    public abstract void EffectAfterAttack();
    public abstract void Skill1();
    public abstract void Skill2();

    public Weapon(HeroController owner): base(owner) { }

    protected override void LoadDataFromJson(string className)
    {
        var data = Managers.DataMng.WeaponDataDict[className];
        if (data.Hp != 0) EquipmentStat.TryAdd(Define.Stat.Hp, data.Hp);
        if (data.Attack != 0) 
            EquipmentStat.TryAdd(Define.Stat.Attack, data.Attack);
        if (data.Defense != 0) EquipmentStat.TryAdd(Define.Stat.Defense, data.Defense);
        if (data.Speed != 0) EquipmentStat.TryAdd(Define.Stat.Speed, data.Speed);
        if (data.Dexterity != 0) EquipmentStat.TryAdd(Define.Stat.Dexterity, data.Dexterity);
        if (data.Strength != 0) EquipmentStat.TryAdd(Define.Stat.Strength, data.Strength);
        if (data.Vitality != 0) EquipmentStat.TryAdd(Define.Stat.Vitality, data.Vitality);
        if (data.Intelligence != 0) EquipmentStat.TryAdd(Define.Stat.Intelligence, data.Intelligence);
        if (data.Left != 0) _leftWeaponIndex = data.Left;
        if (data.Right != 0) _rightWeaponIndex = data.Right;
    }
    
    public override void Equip()
    {
        Owner.Stat.AttachEquipment(EquipmentStat);
        
        if (_leftWeaponIndex != 0)
            Owner.ChangeWeaponVisibility(Define.WeaponSide.Left, _leftWeaponIndex, true);
        if (_rightWeaponIndex != 0)
            Owner.ChangeWeaponVisibility(Define.WeaponSide.Right, _rightWeaponIndex, true);
        
        Owner.ChangeAnimator();
    }

    public override void UnEquip()
    {
        Owner.Stat.DetachEquipment(EquipmentStat);
        
        if (_leftWeaponIndex != 0)
            Owner.ChangeWeaponVisibility(Define.WeaponSide.Left, _leftWeaponIndex, false);
        if (_rightWeaponIndex != 0)
            Owner.ChangeWeaponVisibility(Define.WeaponSide.Right, _rightWeaponIndex, false);
    }
}