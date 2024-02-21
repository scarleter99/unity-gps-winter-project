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
    public event Action<BaseAction> OnSelectedActionChanged;

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
        Attack,
        Skill1,
        Skill2,
        Skill3,
        Item,
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

    public void BindDefaultActions(Hero hero, IEnumerable<BaseAction> actions, bool isMine)
    {
        ClearDefaultActionIcon();
        
        foreach (ActionGroup singleAction in Enum.GetValues(typeof(ActionGroup)))
        {
            if (singleAction == ActionGroup.Skill1 || singleAction == ActionGroup.Skill2)
                continue;

            var iconObj = GetGameObject(GameObjects.ActionIcons).transform.GetChild((int)singleAction).gameObject;
            var action = actions.ElementAt((int)singleAction);
            
            // TODO - Test Code, json으로 데이터 관리
            void DisplayAction(PointerEventData eventData)
            {
                GetText(Text.Text_ActionName).text = action.ToString();
                GetText(Text.Text_ActionDescription).text = "Test Description check";
                GetText(Text.Text_DamageNumber).text = (singleAction == ActionGroup.Flee || singleAction == ActionGroup.Item)? 
                    _stringBlank : Mathf.Max(hero.HeroStat.Attack - hero.TargetCell.CellCreature.CreatureStat.Defense,1f).ToString();
                GetText(Text.Text_SlotPercentage).text = (singleAction == ActionGroup.Flee || singleAction == ActionGroup.Item)? 
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
                GetText(Text.Text_SlotPercentWord).text = (singleAction == ActionGroup.Flee || singleAction == ActionGroup.Item)? 
                    _stringBlank : _stringPercent;

                _selectedAction = action;
                OnSelectedActionChanged?.Invoke(_selectedAction);
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

    // 스킬 목록을 받아와서 출력. 추후에 Init을 할 때 배틀 시스템의 턴 변경 시마다 자기 턴이면 해당 함수를 호출하도록 연결
    public void SetSkills(HeroStat heroStat ,IEnumerable<BaseSkill> skills, bool isMine)
    {
        this.gameObject.SetActive(isMine);
        ClearSkillIcon();

        for (int i = 0; i < 3; i++)
        {
            var iconObj = GetGameObject(GameObjects.ActionIcons).transform.GetChild((int)ActionGroup.Skill1 + i).gameObject;
            var skill = skills.ElementAt(i);
            
            void DisplaySkill(PointerEventData eventData)
            {
                //iconObj.GetOrAddComponent<Image>().color = new Colo
                var skillData = skill.SkillData;
                GetText(Text.Text_ActionName).text = skillData.Name;
                GetText(Text.Text_ActionDescription).text = skillData.Description;
                GetText(Text.Text_DamageNumber).text = Mathf.Max(skill.Owner.CreatureStat.Attack - 
                                                           skill.Owner.TargetCell.CellCreature.CreatureStat.Defense, 1f).ToString();
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
                OnSelectedActionChanged?.Invoke(_selectedAction);
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
