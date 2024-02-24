using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_BattleOrder : UI_Base
{
    public event Action<BaseAction> SelectedActionChange;

    private BaseAction _selectedAction;
    private static readonly string _stringDamage = "Damage";
    private static readonly string _stringBlank = "";
    private static readonly string _stringPercent = "Percentage\nPer Slot";

    enum GameObjects
    {
        ActionIcons
    }
    
    enum ActionGroup
    {
        Skill1,
        Skill2,
        Skill3,
        Item,
        Move,
        Flee
    }

    enum Text
    {
        Text_ActionName,
        Text_ActionDescription,
        Text_DamageNumber,
        Text_DamageWord,
        Text_SlotPercentage,
        Text_SlotPercentWord
        //Text_Target
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        Bind<TextMeshProUGUI>(typeof(Text));
    }

    private void OnEnable()
    {
        Managers.BattleMng.TurnHeroUIChange += EnableBattleOrderUI;
        foreach (var pair in Managers.ObjectMng.Heroes)
        {
            var hero = pair.Value;
            hero.WeaponChange += SetSkills;
        }
    }

    private void OnDisable()
    {
        Managers.BattleMng.TurnHeroUIChange -= EnableBattleOrderUI;
        foreach (var pair in Managers.ObjectMng.Heroes)
        {
            var hero = pair.Value;
            hero.WeaponChange -= SetSkills;
        }
    }

    public void EnableBattleOrderUI(Hero hero)
    {
        BindDefaultActions(hero);
        SetSkills(hero);
    }

    private void BindDefaultActions(Hero hero)
    {
        //ClearDefaultActionIcon();
        
        foreach (ActionGroup singleAction in Enum.GetValues(typeof(ActionGroup)))
        {
            if (singleAction == ActionGroup.Skill1 || singleAction == ActionGroup.Skill2 || singleAction == ActionGroup.Skill3)
                continue;

            var test = GetGameObject(GameObjects.ActionIcons);
            var iconObj = test.transform.GetChild((int)singleAction).gameObject;
            BaseAction action;
            switch (singleAction)
            {
                case ActionGroup.Flee:
                    action = hero.Flee;
                    break;
                case ActionGroup.Move:
                    action = hero.Move;
                    break;
                default: // case ActionGroup.Item:
                    action = hero.SelectBag;
                    break;
            };
            
            // TODO - Test Code, json으로 데이터 관리
            void DisplayAction(PointerEventData eventData)
            {
                GetText(Text.Text_ActionName).text = action.ToString();
                GetText(Text.Text_ActionDescription).text = "Test Description check";
                GetText(Text.Text_DamageNumber).text = IsDefaultAction(singleAction)? 
                    _stringBlank : Mathf.Max(hero.HeroStat.Attack - hero.TargetCell.CellCreature.CreatureStat.Defense,1f).ToString();
                GetText(Text.Text_DamageWord).text = IsDefaultAction(singleAction)? _stringBlank : _stringDamage;
                GetText(Text.Text_SlotPercentage).text = IsDefaultAction(singleAction)? 
                    _stringBlank: hero.WeaponType switch
                    {
                        Define.WeaponType.NoWeapon => hero.HeroStat.Strength.ToString(),
                        Define.WeaponType.Bow => hero.HeroStat.Dexterity.ToString(),
                        Define.WeaponType.Spear => hero.HeroStat.Dexterity.ToString(),
                        Define.WeaponType.Wand => hero.HeroStat.Intelligence.ToString(),
                        Define.WeaponType.SingleSword => hero.HeroStat.Strength.ToString(),
                        Define.WeaponType.DoubleSword => hero.HeroStat.Strength.ToString(),
                        Define.WeaponType.SwordAndShield => hero.HeroStat.Strength.ToString(),
                        Define.WeaponType.TwoHandedSword => hero.HeroStat.Strength.ToString(),
                    } + '%';
                GetText(Text.Text_SlotPercentWord).text = IsDefaultAction(singleAction)? 
                    _stringBlank : _stringPercent;

                _selectedAction = action;
                SelectedActionChange?.Invoke(_selectedAction);
            }

            void UseAction(PointerEventData eventData)
            {
                // TODO - BattleState 넘기고 Select Target으로 이어주기
                this.gameObject.SetActive(false);
            }
            
            iconObj.SetActive(true);
            iconObj.BindEvent(DisplayAction, Define.UIEvent.Enter);
            iconObj.BindEvent(UseAction, Define.UIEvent.Click);
        }
    }

    private bool IsDefaultAction(ActionGroup action)
    {
        return action == ActionGroup.Flee || action == ActionGroup.Item || action == ActionGroup.Move;
    }

    public void SetSkills(Hero hero)
    {
        //ClearSkillIcon();
        var heroStat = hero.HeroStat;

        for (int i = 0; i < 3; i++)
        {
            var iconObj = GetGameObject(GameObjects.ActionIcons).transform.GetChild((int)ActionGroup.Skill1 + i).gameObject;
            var skill = i switch
            {
                0 => hero.Weapon?.Skill1,
                1 => hero.Weapon?.Skill2,
                2 => hero.Weapon?.Skill3
            };

            if (skill == null)
            {
                iconObj.SetActive(false);
                continue;
            }

            void DisplaySkill(PointerEventData eventData)
            {
                //iconObj.GetOrAddComponent<Image>().color = new Colo
                var skillData = skill.SkillData;
                GetText(Text.Text_ActionName).text = skillData.Name;
                GetText(Text.Text_ActionDescription).text = skillData.Description;
                GetText(Text.Text_DamageNumber).text = Mathf.Max(skill.Owner.CreatureStat.Attack, 1f).ToString();
                GetText(Text.Text_DamageWord).text = _stringDamage;
                GetText(Text.Text_SlotPercentWord).text = _stringPercent;
                GetText(Text.Text_SlotPercentage).text = skill.ActionAttribute == Define.ActionAttribute.TauntSkill?
                    heroStat.Vitality.ToString() : 
                    (skill.Owner as Hero)?.WeaponType switch
                {
                    Define.WeaponType.NoWeapon => heroStat.Strength.ToString(),
                    Define.WeaponType.Bow => heroStat.Dexterity.ToString(),
                    Define.WeaponType.Spear => heroStat.Dexterity.ToString(),
                    Define.WeaponType.Wand => heroStat.Intelligence.ToString(),
                    Define.WeaponType.SingleSword => heroStat.Strength.ToString(),
                    Define.WeaponType.DoubleSword => heroStat.Strength.ToString(),
                    Define.WeaponType.SwordAndShield => heroStat.Strength.ToString(),
                    Define.WeaponType.TwoHandedSword => heroStat.Strength.ToString(),
                } + '%';

                _selectedAction = skill;
                SelectedActionChange?.Invoke(_selectedAction);
                this.gameObject.SetActive(true);
            }

            void UseSkill(PointerEventData eventData)
            {
                // TODO - BattleState 넘기고 Select Target으로 이어주기
                this.gameObject.SetActive(false);
            }
            
            iconObj.SetActive(true);
            iconObj.BindEvent(DisplaySkill, Define.UIEvent.Enter);
            iconObj.BindEvent(UseSkill, Define.UIEvent.Click);
        }
    }

    private void ClearSkillIcon()
    {
        for (int i = 0; i < 2; i++)
        {
            var icon = GetGameObject(GameObjects.ActionIcons).transform.GetChild((int)ActionGroup.Skill1 + i).gameObject;
            icon.gameObject.SetActive(false);
            icon.gameObject.ClearEvent();
        }
    }

    private void ClearDefaultActionIcon()
    {
        foreach (ActionGroup action in Enum.GetValues(typeof(ActionGroup)))
        {
            if (action == ActionGroup.Skill1 || action == ActionGroup.Skill2)
                continue;
            
            var icon = GetGameObject(GameObjects.ActionIcons).transform.GetChild((int)action).gameObject;
            icon.gameObject.SetActive(false);
            icon.gameObject.ClearEvent();
        }
    }
}
