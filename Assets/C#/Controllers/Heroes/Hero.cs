using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hero : Creature
{
    public Data.HeroData HeroData => CreatureData as Data.HeroData;
    public HeroStat HeroStat => (HeroStat)CreatureStat;

    public GameObject Head { get; protected set; }
    public GameObject LeftHand { get; protected set; }
    public GameObject RightHand { get; protected set; }
    
    public Bag Bag { get; protected set; }
    public Weapon Weapon { get; protected set; }
    public Define.WeaponType WeaponType => Weapon.WeaponType;
    public Dictionary<Define.ArmorType, Armor> Armors { get; protected set; }
    
    protected override void Init()
    {
        base.Init();
        
        Head = Util.FindChild(gameObject, "head", true);
        LeftHand = Util.FindChild(gameObject, "weapon_l", true);
        RightHand = Util.FindChild(gameObject, "weapon_r", true);
        
        //Bag = new Bag(transform);
        Armors = new Dictionary<Define.ArmorType, Armor>();
        foreach (Define.ArmorType type in (Define.ArmorType[])Enum.GetValues(typeof(Define.ArmorType)))
            Armors.TryAdd(type, null);
        
        // TODO - TEST CODE
        Managers.InputMng.KeyAction -= OnKeyboardClick;
        Managers.InputMng.KeyAction += OnKeyboardClick;
        
    }
    
    public override void SetInfo(int templateId)
    {
        CreatureType = Define.CreatureType.Hero;
        
        base.SetInfo(templateId);
        
        CreatureStat = new HeroStat(HeroData);
    }

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
    
    #region Update
    
    protected override void UpdateAttack()
    {
        var currentState = Animator.GetCurrentAnimatorStateInfo(0);
        if (currentState.shortNameHash == _stateHash)
        {
            var elapsedTime = currentState.normalizedTime;
            if (elapsedTime >= 0.98f) 
            {
                switch (WeaponType)
                {
                    case Define.WeaponType.DoubleSword:
                    case Define.WeaponType.SingleSword:
                    case Define.WeaponType.Spear:
                    case Define.WeaponType.SwordAndShield:
                    case Define.WeaponType.TwoHandedSword:
                        AnimState = Define.AnimState.JumpBack;
                        break;
                }
            }
            else if (elapsedTime >= 0.8f)
            {
                switch (WeaponType)
                {
                    case Define.WeaponType.Bow:
                    case Define.WeaponType.Wand:
                        AnimState = Define.AnimState.Idle;
                        break;
                }
            }
        }
    }

    protected override void UpdateHit()
    {
        var currentState = Animator.GetCurrentAnimatorStateInfo(0);
        if (currentState.normalizedTime >= 0.8f && currentState.shortNameHash == _stateHash)
            AnimState = Define.AnimState.Idle;
    }

    #endregion
    
    /*----------------------
        TODO - TEST CODE
    ----------------------*/
    private void OnKeyboardClick()
    {
        // TODO - Test Code
        if (Input.GetKeyDown(KeyCode.Alpha1))
            Bag.UseItem(this, 0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            Bag.UseItem(this, 1);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Weapon weapon = new SampleSingleSword();
            weapon.SetInfo(Define.WEAPON_SAMPLESINGLESWORD_ID);
            EquipWeapon(weapon);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Weapon weapon = new SampleSwordAndShield();
            weapon.SetInfo(Define.WEAPON_SAMPLESWORDANDSHIELD_ID);
            EquipWeapon(weapon);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Armor body = new SampleBody1();
            body.SetInfo(Define.ARMOR_SAMPLEBODY1_ID);
            EquipArmor(body);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Armor body = new SampleBody2();
            body.SetInfo(Define.ARMOR_SAMPLEBODY2_ID);
            EquipArmor(body);
        }
    }
}