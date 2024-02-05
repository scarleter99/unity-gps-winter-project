public abstract class Weapon: Equipment
{
    public Data.WeaponData WeaponData => EquipmentData as Data.WeaponData;
    public Define.WeaponType WeaponType { get; protected set; }
    public int LeftIndex { get; protected set; }
    public int RightIndex { get; protected set; }

    public BaseSkill Skill1;
    public BaseSkill Skill2;
    public BaseSkill Skill3;
    
    public override void SetInfo(int templateId)
    {
        EquipmentType = Define.EquipmentType.Weapon;
        
        base.SetInfo(templateId);

        LeftIndex = WeaponData.LeftIndex;
        RightIndex = WeaponData.RightIndex;
    }
    
    public override void Equip(Hero hero)
    {
        base.Equip(hero);
        
        if (LeftIndex != 0)
            Owner.ChangeWeaponVisibility(Define.WeaponSide.Left, LeftIndex, true);
        if (RightIndex != 0)
            Owner.ChangeWeaponVisibility(Define.WeaponSide.Right, RightIndex, true);

        Skill1.Owner = Owner;
        Skill2.Owner = Owner;
        Skill3.Owner = Owner;
    }

    public override void UnEquip()
    {
        if (LeftIndex != 0)
            Owner.ChangeWeaponVisibility(Define.WeaponSide.Left, LeftIndex, false);
        if (RightIndex != 0)
            Owner.ChangeWeaponVisibility(Define.WeaponSide.Right, RightIndex, false);
        
        Owner = null;
        Skill1.Owner = null;
        Skill2.Owner = null;
        Skill3.Owner = null;
    }
}
