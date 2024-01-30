using UnityEngine;

public class SampleSingleSword : Weapon
{
    public SampleSingleSword(HeroController owner): base(owner)
    {
        LoadDataFromJson(this.GetType().Name);
        
        _weaponType = Define.WeaponType.SingleSword;
        owner.WeaponType = _weaponType;
    }
    
    #region Event
    public override void EffectAfterAttack()
    {
        // 무언가 특별한 로직이 있다면 여기에 구현 (ex: 특정 확률로 상태이상)
        // debug
        Debug.Log("Effect After Attack Debug");
    }
    #endregion

    #region Skill
    public override void Skill1()
    {
        // debug
        Debug.Log("Skill1 Debug");
    }

    public override void Skill2()
    {
        // debug
        Debug.Log("Skill2 Debug");
    }
    #endregion
}
