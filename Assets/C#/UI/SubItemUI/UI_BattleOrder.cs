using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_BattleOrder : UI_Base
{
    public event Action<Skill> OnSelectedSkillChanged;

    private Skill _selectedSkill;

    enum SkillIconGroup
    {
        SkillIcons
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
        Bind<GameObject>(typeof(SkillIconGroup));
        Bind<TextMeshProUGUI>(typeof(Text));
    }

    // 임시용 스킬 클래스 ////////////////////
    public class Skill
    {
        public enum SkillTarget
        {
            Oneself,
            OneEnemy,
            GroupEnemy,
            OnePlayer,
            GroupPlayer,
        }

        public string Name, Description;
        public int Damage;
        public int SlotCount;
        public Define.Stat AffectedStat;
        public SkillTarget Target;
        public Sprite Icon;

        public void UseSkill()
        {

        }
    }
    ////////////////////////////////////////

    // 스킬 목록을 받아와서 출력. 추후에 Init을 할 때 배틀 시스템의 턴 변경 시마다 자기 턴이면 해당 함수를 호출하도록 연결
    private void SetSkills(PlayerStat playerStat ,IEnumerable<Skill> skills, bool isMine)
    {
        this.gameObject.SetActive(isMine);
        ClearSkillIcon();

        int i = 0;
        foreach (var skill in skills)
        {
            GameObject iconObj = GetGameObject(SkillIconGroup.SkillIcons).transform.GetChild(i++).gameObject;

            void DsiplaySkill(PointerEventData eventData)
            {
                //iconObj.GetOrAddComponent<Image>().color = new Colo

                GetText(Text.Text_SkillName).text = skill.Name;
                GetText(Text.Text_SkillDescription).text = skill.Description;
                GetText(Text.Text_Damage).text = skill.Damage.ToString();
                GetText(Text.Text_Target).text = skill.Target.ToString();
                GetText(Text.Text_SlotPercentage).text = skill.AffectedStat switch
                {
                    Define.Stat.Strength => playerStat.Strength.ToString(),
                    Define.Stat.Vitality => playerStat.Strength.ToString(),
                    Define.Stat.Dexterity => playerStat.Strength.ToString(),
                    Define.Stat.Intelligence => playerStat.Strength.ToString(),
                } + '%';

                _selectedSkill = skill;
                OnSelectedSkillChanged?.Invoke(_selectedSkill);
            }

            void UseSkill(PointerEventData eventData)
            {
                skill.UseSkill();
                this.gameObject.SetActive(false);
            }

            iconObj.SetActive(true);
            iconObj.BindEvent(DsiplaySkill, Define.UIEvent.Enter);
            iconObj.BindEvent(UseSkill, Define.UIEvent.Click);
        }
    }

    private void ClearSkillIcon()
    {
        foreach (Transform icon in GetGameObject(SkillIconGroup.SkillIcons).transform)
        {
            icon.gameObject.SetActive(false);
            icon.gameObject.ClearEvent();
        }
    }
}
