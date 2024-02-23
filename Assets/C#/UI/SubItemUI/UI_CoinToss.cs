using OpenCover.Framework.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
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
    /// CoinToss UI 필요할 때만 보이고, 나머지는 숨김
    /// </summary>
    public void ChangeVisibility(BaseAction action)
    {
        switch (action.ActionAttribute)
        {
            case Define.ActionAttribute.Move:
            case Define.ActionAttribute.SelectBag:
                Clear();
                break;
            default:
                SetStatType(action);
                break;
        }
    }
    
    private void SetStatType(BaseAction action)
    {
        Clear();
        
        CoinGroup activatedGroup = ProperStat(action);
        GameObject groupObj = GetGameObject(activatedGroup);
        groupObj.SetActive(true);
        
        int i = 0;
        switch (action.ActionAttribute)
        {
            case Define.ActionAttribute.Flee:
                foreach (Transform item in groupObj.transform)
                    item.gameObject.SetActive(i++ < (action as Flee)?.CoinNum);
                break;
            default:
                foreach (Transform item in groupObj.transform)
                    item.gameObject.SetActive(i++ < (action as BaseSkill)?.CoinNum);
                break;
        }

        _currentGroup = activatedGroup;
    }

    private CoinGroup ProperStat(BaseAction action)
    {
        CoinGroup ret;
        switch (action.ActionAttribute)
        {
            case Define.ActionAttribute.Flee:
                ret = CoinGroup.DexterityGroup;
                break;
            default:
                ret = (action.Owner as Hero)?.WeaponType switch
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
                if (action.ActionAttribute == Define.ActionAttribute.TauntSkill)
                    ret = CoinGroup.VitalityGroup;
                break;
        }

        return ret;
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
