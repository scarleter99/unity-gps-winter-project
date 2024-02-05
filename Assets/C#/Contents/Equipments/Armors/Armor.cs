public abstract class Armor: Equipment
{
    public Data.ArmorData ArmorData => EquipmentData as Data.ArmorData;
    public Define.ArmorType ArmorType { get; protected set; }
    public int ArmorIndex { get; protected set; }
    
    public override void SetInfo(int templateId)
    {
        EquipmentType = Define.EquipmentType.Armor;
        
        base.SetInfo(templateId);

        ArmorIndex = ArmorData.ArmorIndex;
    }
    
    public override void Equip(Hero hero)
    {
        base.Equip(hero);
        
        if (ArmorIndex != 0)
            Owner.ChangeArmorVisibility(ArmorType, ArmorIndex, true);
    }

    public override void UnEquip()
    {
        Owner.HeroStat.DetachEquipment(EquipmentData);
        
        if (ArmorIndex != 0)
            Owner.ChangeArmorVisibility(ArmorType, ArmorIndex, false);

        Owner = null;
    }
}
