using System.Collections.Generic;

public abstract class Equipment
{
    public int DataTemplateId { get; protected set; }
    public Define.EquipmentType EquipmentType { get; protected set; }
    public Data.EquipmentData EquipmentData { get; protected set; }
    public HeroController Owner { get; protected set; }
    
    // 수동 실행
    protected virtual void SetInfo(int templateId)
    {
        DataTemplateId = templateId;
        
        if (EquipmentType == Define.EquipmentType.Weapon)
            EquipmentData = Managers.DataMng.WeaponDataDict[templateId];
        else
            EquipmentData = Managers.DataMng.ArmorDataDict[templateId];
    }

    public virtual void Equip(HeroController heroController)
    {
        Owner = heroController;
        Owner.HeroStat.AttachEquipment(EquipmentData);
    }

    public virtual void UnEquip()
    {
        Owner.HeroStat.DetachEquipment(EquipmentData);
    }
}
