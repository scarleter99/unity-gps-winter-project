using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_BattleOrder : UI_Base
{
    public event Action<BaseSkill> OnSelectedSkillChanged;

    private BaseSkill _selectedSkill;

    enum ActionIcons
    {
        Attack,
        Skill,
        Item,
        Flee
    }

    enum Text
    {
        Text_SkillName,
        Text_SkillDescription,
        Text_Damage,
        Text_SlotPercentage,
        Text_Target
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(ActionIcons));
        Bind<TextMeshProUGUI>(typeof(Text));
    }

    // 스킬 목록을 받아와서 출력. 추후에 Init을 할 때 배틀 시스템의 턴 변경 시마다 자기 턴이면 해당 함수를 호출하도록 연결
    private void SetSkills(HeroStat heroStat ,IEnumerable<BaseSkill> skills, bool isMine)
    {
        this.gameObject.SetActive(isMine);
        ClearSkillIcon();

        int i = 0;
        foreach (var skill in skills)
        {
            GameObject iconObj = GetGameObject(ActionIcons.Skill).transform.GetChild(i++).gameObject;

            void DsiplaySkill(PointerEventData eventData)
            {
                //iconObj.GetOrAddComponent<Image>().color = new Colo
                var skillData = skill.SkillData;
                GetText(Text.Text_SkillName).text = skillData.Name;
                GetText(Text.Text_SkillDescription).text = skillData.Description;
                GetText(Text.Text_Damage).text = Mathf.Max(skill.Owner.CreatureStat.Attack - 
                                                           skill.Owner.TargetCell.CellCreature.CreatureStat.Defense, 1f).ToString();
                GetText(Text.Text_Target).text = skill.Owner.TargetCell.CellCreature.name;
                GetText(Text.Text_SlotPercentage).text = skill.ActionAttribute == Define.ActionAttribute.Tempt?
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

                _selectedSkill = skill;
                OnSelectedSkillChanged?.Invoke(_selectedSkill);
            }

            void UseSkill(PointerEventData eventData)
            {
                // TODO - BattleState 넘기고 Select Target으로 이어주기
                this.gameObject.SetActive(false);
            }

            iconObj.SetActive(true);
            iconObj.BindEvent(DsiplaySkill, Define.UIEvent.Enter);
            iconObj.BindEvent(UseSkill, Define.UIEvent.Click);
        }
    }

    private void ClearSkillIcon()
    {
        foreach (Transform icon in GetGameObject(ActionIcons.Skill).transform)
        {
            icon.gameObject.SetActive(false);
            icon.gameObject.ClearEvent();
        }
    }
}
