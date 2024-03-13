using Unity.VisualScripting;
using UnityEngine;

public class SampleSingleSword : Weapon
{
    public override void SetInfo(int templateId)
    {
        WeaponType = Define.WeaponType.SingleSword;
        
        base.SetInfo(templateId);
        
        Skill1 = new Strike();
        Skill2 = new Strike();
        Skill3 = new Strike();
        
        Skill1.SetInfo(Define.ACTION_STRIKE_ID);
        Skill2.SetInfo(Define.ACTION_STRIKE_ID);
        Skill3.SetInfo(Define.ACTION_STRIKE_ID);
    }

    public override void Equip(Hero hero)
    {
        base.Equip(hero);
        
        Skill1.Equip(Owner);
        Skill2.Equip(Owner);
        Skill3.Equip(Owner);
    }
}
