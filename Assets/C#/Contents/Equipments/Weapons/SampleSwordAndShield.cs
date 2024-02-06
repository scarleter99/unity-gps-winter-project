using UnityEngine;

public class SampleSwordAndShield: Weapon
{
    public override void SetInfo(int templateId)
    {
        WeaponType = Define.WeaponType.SwordAndShield;
        
        base.SetInfo(templateId);

        Skill1 = new Strike();
        Skill2 = new Strike();
        Skill3 = new Strike();
    }
}