using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public abstract class Hero : Creature
{
    public Data.HeroData HeroData => CreatureData as Data.HeroData;
    public HeroStat HeroStat => (HeroStat)CreatureStat;

    public GameObject Head { get; protected set; }
    public GameObject LeftHand { get; protected set; }
    public GameObject RightHand { get; protected set; }

    public SelectBag SelectBag { get; protected set; }
    public Bag Bag { get; protected set; }
    [CanBeNull]
    public Weapon Weapon { get; protected set; }
    public Define.WeaponType? WeaponType => Weapon?.WeaponType;
    public Dictionary<Define.ArmorType, Armor> Armors { get; protected set; }

    public event Action<Hero> WeaponChange;
    
    protected override void Init()
    {
        base.Init();
        
        Head = Util.FindChild(gameObject, "head", true);
        LeftHand = Util.FindChild(gameObject, "weapon_l", true);
        RightHand = Util.FindChild(gameObject, "weapon_r", true);

        SelectBag = new SelectBag();
        SelectBag.SetInfo(this);
        Bag = new Bag();
        Bag.SetInfo();
        Bag.Owner = this;
        
        // TODO - TEST CODE
        ApproachType = Define.ApproachType.Jump;
        
        Armors = new Dictionary<Define.ArmorType, Armor>();
        foreach (Define.ArmorType type in (Define.ArmorType[])Enum.GetValues(typeof(Define.ArmorType)))
            Armors.TryAdd(type, null);
    }
    
    public override void SetInfo(int templateId)
    {
        CreatureType = Define.CreatureType.Hero;
        
        base.SetInfo(templateId);
        
        CreatureStat = new HeroStat(HeroData);
    }

    #region Action

    private bool NeedsInvoke(out int percent)
    {
        switch (CurrentAction.ActionAttribute)
        {
            case Define.ActionAttribute.SelectBag:
            case Define.ActionAttribute.Flee:
            case Define.ActionAttribute.Move:
            case Define.ActionAttribute.AttackItem:
            case Define.ActionAttribute.BuffItem:
            case Define.ActionAttribute.DebuffItem:
            case Define.ActionAttribute.HealItem:
                percent = 0;
                return false;
        }
        
        Debug.Log(CurrentAction.ActionAttribute);

        percent = CurrentAction.ActionAttribute switch
        {
            Define.ActionAttribute.AttackSkill => WeaponType switch
            {
                Define.WeaponType.Bow => HeroStat.Dexterity,
                Define.WeaponType.Spear => HeroStat.Dexterity,
                Define.WeaponType.Wand => HeroStat.Intelligence,
                Define.WeaponType.SingleSword => HeroStat.Strength,
                Define.WeaponType.DoubleSword => HeroStat.Strength,
                Define.WeaponType.SwordAndShield => HeroStat.Strength,
                Define.WeaponType.TwoHandedSword => HeroStat.Strength,
                null => HeroStat.Strength
            },
            Define.ActionAttribute.BuffSkill => HeroStat.Intelligence,
            Define.ActionAttribute.DebuffSkill => HeroStat.Intelligence,
            Define.ActionAttribute.HealSkill => HeroStat.Intelligence,
            Define.ActionAttribute.TauntSkill => HeroStat.Vitality
        };

        return true;
    }
    
    #endregion
    
    // TODO - Data Id로 무기 및 아머를 장착하도록 구현
    #region Weapon
    public void ChangeAnimator()
    {
        string path = "Animator Controllers/Players/" + WeaponType.ToString();
        Animator.runtimeAnimatorController = Resources.Load(path) as RuntimeAnimatorController; 
    }
    
    public void EquipWeapon(Weapon equippingWeapon)
    {
        if (Weapon != null)
        {
            if (Weapon.WeaponData.DataId == equippingWeapon.WeaponData.DataId)
                return;
            UnEquipWeapon();
        }
        
        Weapon = equippingWeapon;
        HeroStat.AttachEquipment(Weapon.EquipmentData);
        Weapon.Equip(this);
        ChangeWeaponVisibility(true);
        ChangeAnimator();
        WeaponChange?.Invoke(this);
    }
    
    public void UnEquipWeapon()
    {
        if (Weapon != null)
        {
            HeroStat.DetachEquipment(Weapon.EquipmentData);
            Weapon.UnEquip();
            ChangeWeaponVisibility(false);
            Weapon = null;
        }
    }
    
    public void ChangeWeaponVisibility(bool isActive)
    {
        int leftIndex = Weapon.WeaponData.LeftIndex;
        int rightIndex = Weapon.WeaponData.RightIndex;
        if (leftIndex != 0)
        {
            LeftHand.transform.GetChild(leftIndex).gameObject.SetActive(isActive);
        }
        if (rightIndex != 0)
        {
            RightHand.transform.GetChild(rightIndex).gameObject.SetActive(isActive);
        }
    }
    #endregion
    
    #region Armor
    public void EquipArmor(Armor equippingArmor)
    {
        Define.ArmorType armorType = equippingArmor.ArmorType;
        if (Armors[armorType] != null)
        {
            if (Armors[armorType].ArmorData.DataId == equippingArmor.ArmorData.DataId)
                return;
            UnEquipArmor(armorType);
        }
        
        Armors[armorType] = equippingArmor;
        HeroStat.AttachEquipment(Armors[armorType].EquipmentData);
        Armors[armorType].Equip(this);
        ChangeArmorVisibility(armorType ,true);
    }

    public void UnEquipArmor(Define.ArmorType armorType)
    {
        if (Armors[armorType] != null)
        {
            HeroStat.DetachEquipment(Armors[armorType].EquipmentData);
            Armors[armorType].UnEquip();
            ChangeArmorVisibility(armorType, false);
            Armors[armorType] = null;
        }
    }

    public void ChangeArmorVisibility(Define.ArmorType armorType, bool isActive)
    {
        int idx = Armors[armorType].ArmorData.ArmorIndex;
        switch (armorType)
        {
             case Define.ArmorType.Body: 
                 transform.GetChild(idx - 1).gameObject.SetActive(isActive);
                 break; 
             case Define.ArmorType.Cloak:
                 transform.GetChild(idx + 19).gameObject.SetActive(isActive);
                 break;
             case Define.ArmorType.HeadAccessory:
                 Head.transform.GetChild(idx - 1).gameObject.SetActive(isActive);
                 break;
             case Define.ArmorType.Helmet:
                 Head.transform.GetChild(idx + 96).gameObject.SetActive(isActive);
                 break;
        }
    }
    #endregion
}
