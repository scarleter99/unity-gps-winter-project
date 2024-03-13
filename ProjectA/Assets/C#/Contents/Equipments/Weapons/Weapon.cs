public abstract class Weapon: Equipment
{
    public Data.WeaponData WeaponData => EquipmentData as Data.WeaponData;
    public Define.WeaponType WeaponType { get; protected set; }
    public int LeftIndex { get; protected set; }
    public int RightIndex { get; protected set; }

    public BaseAction Skill1;
    public BaseAction Skill2;
    public BaseAction Skill3;
    
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
        
        Skill1.Owner = Owner;
        Skill2.Owner = Owner;
        Skill3.Owner = Owner;
    }
    
    public override void UnEquip()
    {
        Skill1.Owner = null;
        Skill2.Owner = null;
        Skill3.Owner = null;
    }
}
