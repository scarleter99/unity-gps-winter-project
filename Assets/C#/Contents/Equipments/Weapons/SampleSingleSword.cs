using UnityEngine;

public class SampleSingleSword : Weapon
{
    public SampleSingleSword() : base()
    {
        WeaponType = Define.WeaponType.SingleSword;
    }
    
    protected override void SetInfo(int templateId)
    {
        base.SetInfo(templateId);

        Skill1 = new Strike();
        Skill2 = new Strike();
        Skill3 = new Strike();
    }
}
