using System.Collections.Generic;
using UnityEngine;

public class SampleSingleSword : Weapon
{
    public Define.WeaponType WeaponType { get => _weaponType; }
    
    public SampleSingleSword(PlayerController equipper): base(equipper)
    {
        // temp - for test
        StatData.TryAdd("Attack", 100);
        
        _weaponType = Define.WeaponType.SingleSword;
        equipper.WeaponType = _weaponType;
    }

    public override void EffectAfterAttack(PlayerController controller)
    {
        // 무언가 특별한 로직이 있다면 여기에 구현 (ex: 특정 확률로 상태이상)
        // debug
        Debug.Log("Effect After Attack Debug");
    }

    public override void Skill1(PlayerController controller)
    {
        // debug
        Debug.Log("Skill1 Debug");
    }

    public override void Skill2(PlayerController controller)
    {
        // debug
        Debug.Log("Skill2 Debug");
    }
}
