using UnityEngine;

public class SampleSwordAndShield: Weapon
{
    public SampleSwordAndShield() : base()
    {
        WeaponType = Define.WeaponType.SwordAndShield;
    }
    
    protected override void SetInfo(int templateId)
    {
        base.SetInfo(templateId);

        Skill1 = new Strike();
        Skill2 = new Strike();
        Skill3 = new Strike();
    }
}