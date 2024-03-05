using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

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

    public override void Init()
    {
        Bind<GameObject>(typeof(CoinGroup));
    }

    private void OnEnable()
    {
        Managers.BattleMng.ShowCoinToss += ShowTossResult;
    }

    private void OnDisable()
    {
        Managers.BattleMng.ShowCoinToss -= ShowTossResult;
    }

    public void ShowTossResult(BaseAction action, int successPercent)
    {
        if (action.ActionAttribute == Define.ActionAttribute.Move ||
            action.ActionAttribute == Define.ActionAttribute.SelectBag)
            return;
        
        StartCoroutine(DisplayCoinToss(action, successPercent));
    }
    
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
        foreach (Transform item in groupObj.transform)
            item.gameObject.SetActive(i++ < CoinCount(action));

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
                    null => CoinGroup.StrengthGroup,
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

    private int CoinCount(BaseAction action)
    {
        switch (action.ActionAttribute)
        {
            case Define.ActionAttribute.Flee:
                return (action as FleeAction).CoinNum;
            default:
                return (action as BaseSkill).CoinNum;
        }
    }

    private IEnumerator DisplayCoinToss(BaseAction action, int successPercent)
    {
        GameObject groupObj = GetGameObject(_currentGroup);
        int coinCount = CoinCount(action);
        int successCount = Managers.BattleMng.SuccessCount(coinCount, successPercent);

        for (int i = 0; i < coinCount; i++)
        {
            successCount--;
            if (successCount >= 0)
                groupObj.transform.GetChild(i).GetOrAddComponent<UI_Coin>().DisplayIcon(true);
            yield return new WaitForSeconds(0.3f);
        }

        yield return new WaitForSeconds(0.3f);
        
        // TODO - Test Code
        if (action.ActionAttribute == Define.ActionAttribute.Flee)
        {
            if (successCount == coinCount)
                Debug.Log("Success");
            else
                Debug.Log("Failure");
        }
        else
        {
            //Managers.BattleMng.CurrentTurnCreature.DoAction();
        }
        
        groupObj.SetActive(false);
    }

    private void Clear()
    {
        foreach (CoinGroup group in Enum.GetValues(typeof(CoinGroup)))
            GetGameObject(group).SetActive(false);
    }
}
