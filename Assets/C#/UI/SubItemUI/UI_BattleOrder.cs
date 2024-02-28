using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_BattleOrder : UI_Base
{
    public event Action<BaseAction> SelectedActionChange;
    public event Action<BaseAction, int> SelectedActionClick;

    public int _percentage;
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
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        Bind<TextMeshProUGUI>(typeof(Text));
    }

    private void OnEnable()
    {
        Managers.BattleMng.TurnHeroUIChange -= EnableBattleOrderUI; 
        Managers.BattleMng.TurnHeroUIChange += EnableBattleOrderUI;
        foreach (var pair in Managers.ObjectMng.Heroes)
        {
            var hero = pair.Value;
            hero.WeaponChange -= BindSkills;
            hero.WeaponChange += BindSkills;
        }
    }
    
    // TODO - 없으면 두번째 플레이부터 에러남, 이유 모르겠음
    private void OnDisable()
    {
        // Managers.BattleMng.TurnHeroUIChange -= EnableBattleOrderUI;
        // foreach (var pair in Managers.ObjectMng.Heroes)
        // {
        //     var hero = pair.Value;
        //     hero.WeaponChange -= BindSkills;
        // }
    }

    public void EnableBattleOrderUI(Hero hero)
    {
        BindDefaultActions(hero);
        BindSkills(hero);
    }
    
    #region DefaultAction
    private void BindDefaultActions(Hero hero)
    {
        ClearDefaultActionIcons();
        foreach (ActionGroup singleAction in Enum.GetValues(typeof(ActionGroup)))
        {
            if (singleAction == ActionGroup.Skill1 || singleAction == ActionGroup.Skill2 || singleAction == ActionGroup.Skill3)
                continue;
            
            var iconObj = GetGameObject(GameObjects.ActionIcons).transform.GetChild((int)singleAction).gameObject;
            BaseAction action;
            switch (singleAction)
            {
                case ActionGroup.Flee:
                    action = hero.FleeAction;
                    break;
                case ActionGroup.Move:
                    action = hero.MoveAction;
                    break;
                default: // case ActionGroup.Item:
                    action = hero.SelectBagAction;
                    break;
            };
            
            // TODO - Test Code, json으로 데이터 관리
            void DisplayAction(PointerEventData eventData)
            {
                GetText(Text.Text_ActionName).text = action.ToString();
                GetText(Text.Text_ActionDescription).text = "Test Description check";
                GetText(Text.Text_DamageNumber).text = HasNoDamage(singleAction)? 
                    _stringBlank : Mathf.Max(hero.HeroStat.Attack - hero.TargetCell.CellCreature.CreatureStat.Defense,1f).ToString();
                GetText(Text.Text_DamageWord).text = HasNoDamage(singleAction)? _stringBlank : _stringDamage;
                var percentageText = GetText(Text.Text_SlotPercentage);
                percentageText.text = HasNoPercentage(singleAction)? 
                    _stringBlank: hero.WeaponType switch
                    {
                        Define.WeaponType.Bow => hero.HeroStat.Dexterity.ToString(),
                        Define.WeaponType.Spear => hero.HeroStat.Dexterity.ToString(),
                        Define.WeaponType.Wand => hero.HeroStat.Intelligence.ToString(),
                        Define.WeaponType.SingleSword => hero.HeroStat.Strength.ToString(),
                        Define.WeaponType.DoubleSword => hero.HeroStat.Strength.ToString(),
                        Define.WeaponType.SwordAndShield => hero.HeroStat.Strength.ToString(),
                        Define.WeaponType.TwoHandedSword => hero.HeroStat.Strength.ToString(),
                        null => hero.HeroStat.Strength.ToString()
                    } + '%';
                if (int.TryParse(percentageText.text.Substring(0, Mathf.Max(percentageText.text.Length - 1, 0)), out _percentage) == false)
                    _percentage = 0;
                GetText(Text.Text_SlotPercentWord).text = HasNoPercentage(singleAction)? 
                    _stringBlank : _stringPercent;

                _selectedAction = action;
                SelectedActionChange?.Invoke(_selectedAction);
            }

            void UseAction(PointerEventData eventData)
            {
                Managers.BattleMng.CurrentTurnCreature.CurrentAction = action;

                // Flee, SelectBag 만 이쪽에서 핸들링, 나머지는 BattleManager BattleState set에서 관리
                // TODO - Flee, SelectBag 핸들링도 BattleManager로 넘기는 쪽이 깔끔해보임
                switch (action.ActionAttribute)
                {
                    case Define.ActionAttribute.Flee:
                        //Managers.BattleMng.CurrentTurnCreature.DoAction();
                        break;
                    case Define.ActionAttribute.SelectBag:
                        // TODO - Item UI 띄우기
                        Debug.Log("SelectBag Clicked!");
                        break;
                    case Define.ActionAttribute.Move:
                        Managers.BattleMng.BattleState = Define.BattleState.SelectTarget;
                        break;
                }
                
                gameObject.SetActive(false);
                SelectedActionClick?.Invoke(_selectedAction, _percentage);
            }
            
            iconObj.SetActive(true);
            iconObj.BindEvent(DisplayAction, Define.UIEvent.Enter);
            iconObj.BindEvent(UseAction, Define.UIEvent.Click);
        }
    }
    
    private void ClearDefaultActionIcons()
    {
        foreach (ActionGroup action in Enum.GetValues(typeof(ActionGroup)))
        {
            if (action == ActionGroup.Skill1 || action == ActionGroup.Skill2 || action == ActionGroup.Skill3)
                continue;
            
            var icon = GetGameObject(GameObjects.ActionIcons).transform.GetChild((int)action).gameObject;
            icon.SetActive(false);
            icon.ClearEvent();
        }
    }

    private bool HasNoDamage(ActionGroup action)
    {
        return action == ActionGroup.Flee || action == ActionGroup.Item || action == ActionGroup.Move;
    }

    private bool HasNoPercentage(ActionGroup action)
    {
        return action == ActionGroup.Item || action == ActionGroup.Move;
    }

    #endregion
    
    #region Skill
    
    public void BindSkills(Hero hero)
    {
        ClearSkillIcons();
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
                //if (skill.Owner == null) return; // TODO - SHOULD NOT HAPPEN
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
                    null => heroStat.Strength.ToString(),
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
                gameObject.SetActive(true);
            }

            void UseSkill(PointerEventData eventData)
            {
                Managers.BattleMng.CurrentTurnCreature.CurrentAction = _selectedAction;
                Managers.BattleMng.BattleState = Define.BattleState.SelectTarget;
                gameObject.SetActive(false);
            }
            
            iconObj.SetActive(true);
            iconObj.BindEvent(DisplaySkill, Define.UIEvent.Enter);
            iconObj.BindEvent(UseSkill, Define.UIEvent.Click);
        }
    }

    private void ClearSkillIcons()
    {
        for (int i = 0; i < 2; i++)
        {
            var icon = GetGameObject(GameObjects.ActionIcons).transform.GetChild((int)ActionGroup.Skill1 + i).gameObject;
            icon.gameObject.ClearEvent();
            icon.gameObject.SetActive(false);
        }
    }
    
    #endregion
}
