using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_CoinToss : UI_Popup
{
    enum CoinGroup
    {
        StrengthGroup,
        VitalityGroup,
        DexterityGroup,
        IntelligenceGroup,
    }

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(CoinGroup));
    }

    /// <summary>
    /// Stat에 해당하는 이미지 출력
    /// </summary>
    public void SetStatType(Define.Stat stat, int count)
    {
        Clear();

        CoinGroup type = stat switch
        {
            Define.Stat.Strength => CoinGroup.StrengthGroup,
            Define.Stat.Vitality => CoinGroup.VitalityGroup,
            Define.Stat.Dexterity => CoinGroup.DexterityGroup,
            Define.Stat.Intelligence => CoinGroup.IntelligenceGroup,
        };

        GameObject groupObj = GetGameObject(type);
        groupObj.SetActive(true);
        int i = 0;
        foreach (Transform item in groupObj.transform)
            item.gameObject.SetActive(i++ < count);
    }

    private void OnEnable()
    {
        Clear();
    }

    private void Clear()
    {
        foreach (CoinGroup group in Enum.GetValues(typeof(CoinGroup)))
            GetGameObject(group).SetActive(false);
    }
}
