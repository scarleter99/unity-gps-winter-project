using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_BattleOrder : UI_Base
{
    enum Buttons
    {
        Skill1,
        Skill2,
        Skill3,
        Item,
        Move,
        Flee
    }

    enum Texts
    {
        Text_ActionName,
        Text_ActionDescription,
        Text_DamageNumber,
        Text_DamageWord,
        Text_SlotPercentage,
        Text_SlotPercentageWord
    }
    
    public Hero CurrentTurnHero { get; protected set; }
    
    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));
        
        GetButton(Buttons.Skill1).GetOrAddComponent<UI_ActionButton>().UIBattleOrder = this;
        GetButton(Buttons.Skill2).GetOrAddComponent<UI_ActionButton>().UIBattleOrder = this;
        GetButton(Buttons.Skill3).GetOrAddComponent<UI_ActionButton>().UIBattleOrder = this;
        GetButton(Buttons.Move).GetOrAddComponent<UI_ActionButton>().UIBattleOrder = this;
        GetButton(Buttons.Flee).GetOrAddComponent<UI_ActionButton>().UIBattleOrder = this;
    }

    public void InitTurn()
    {
        gameObject.SetActive(true);
        CurrentTurnHero = Managers.BattleMng.CurrentTurnCreature as Hero;
        
        SetButtons();
    }

    public void EndTurn()
    {
        gameObject.SetActive(true);
        CurrentTurnHero = null;
        SetButtons();
    }
    
    protected void SetButtons()
    {
        GetButton(Buttons.Skill1).GetOrAddComponent<UI_ActionButton>().Action = CurrentTurnHero.Weapon.Skill1;
        GetButton(Buttons.Skill2).GetOrAddComponent<UI_ActionButton>().Action = CurrentTurnHero.Weapon.Skill2;
        GetButton(Buttons.Skill3).GetOrAddComponent<UI_ActionButton>().Action = CurrentTurnHero.Weapon.Skill3;
        GetButton(Buttons.Move).GetOrAddComponent<UI_ActionButton>().Action = CurrentTurnHero.MoveAction;
        GetButton(Buttons.Flee).GetOrAddComponent<UI_ActionButton>().Action = CurrentTurnHero.FleeAction;
    }

    public void ShowActionInfo(BaseAction action, bool isNoDamage, bool isNoPercentage)
    {
        ClearActionInfo();
        GetText(Texts.Text_ActionName).text = action.ToString();
        GetText(Texts.Text_ActionDescription).text = "Test Description check";

        if (!isNoDamage)
        {
            GetText(Texts.Text_DamageWord).text = "DAMAGE";
            GetText(Texts.Text_DamageNumber).text = CurrentTurnHero.HeroStat.Attack.ToString();
        }

        if (!isNoPercentage)
        {
            GetText(Texts.Text_SlotPercentageWord).text = "Percentage\nPer Slot";
            GetText(Texts.Text_SlotPercentage).text = CurrentTurnHero.HeroStat.GetStatByDefine(action.UsingStat).ToString();
        }
        
        ((UI_BattleScene)Managers.UIMng.SceneUI).CoinTossUI.ShowCoinNum(action);
    }

    protected void ClearActionInfo()
    {
        GetText(Texts.Text_ActionName).text = null;
        GetText(Texts.Text_ActionDescription).text = null;
        GetText(Texts.Text_DamageWord).text = null;
        GetText(Texts.Text_DamageNumber).text = null;
        GetText(Texts.Text_SlotPercentageWord).text = null;
        GetText(Texts.Text_SlotPercentage).text = null;
    }
}
