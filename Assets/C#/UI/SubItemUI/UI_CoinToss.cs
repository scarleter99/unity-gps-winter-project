using OpenCover.Framework.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_CoinToss : UI_Base
{
    enum CoinGroup
    {
        StrengthGroup,
        VitalityGroup,
        DexterityGroup,
        IntelligenceGroup,
    }

    private CoinGroup _currentGroup;
    private int _count;

    public override void Init()
    {
        Bind<GameObject>(typeof(CoinGroup));
    }

    /// <summary>
    /// Stat에 해당하는 이미지 출력
    /// </summary>
    public void SetStatType(BaseSkill skill)
    {
        Clear();

        CoinGroup type = skill.ActionAttribute == Define.ActionAttribute.Tempt?
            CoinGroup.VitalityGroup : (skill.Owner as Hero)?.WeaponType switch
        {
            Define.WeaponType.NoWeapon => CoinGroup.StrengthGroup,
            Define.WeaponType.Bow => CoinGroup.DexterityGroup,
            Define.WeaponType.Spear => CoinGroup.DexterityGroup,
            Define.WeaponType.Wand => CoinGroup.IntelligenceGroup,
            Define.WeaponType.SingleSword => CoinGroup.StrengthGroup,
            Define.WeaponType.DoubleSword => CoinGroup.StrengthGroup,
            Define.WeaponType.SwordAndShield => CoinGroup.StrengthGroup,
            Define.WeaponType.TwoHandedSword => CoinGroup.StrengthGroup,
        };

        GameObject groupObj = GetGameObject(type);
        groupObj.SetActive(true);
        int i = 0;
        foreach (Transform item in groupObj.transform)
            item.gameObject.SetActive(i++ < skill.CoinNum);

        _currentGroup = type;
        _count = skill.CoinNum;
    }

    public void DisplayCoinToss(int successCount)
    {
        GameObject groupObj = GetGameObject(_currentGroup);

        for (int i = 0; i < _count; i++)
            groupObj.transform.GetChild(i).GetOrAddComponent<UI_Coin>().DisplayIcon(i < successCount);
    }

    private void Clear()
    {
        foreach (CoinGroup group in Enum.GetValues(typeof(CoinGroup)))
            GetGameObject(group).SetActive(false);
    }
}
