using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Hero : Creature
{
    public Data.HeroData HeroData => CreatureData as Data.HeroData;
    public HeroStat HeroStat => (HeroStat)Stat;

    public GameObject Head { get; protected set; }
    public GameObject LeftHand { get; protected set; }
    public GameObject RightHand { get; protected set; }
    
    public Bag Bag { get; protected set; }
    protected Weapon _weapon;
    public Weapon Weapon 
    { 
        get => _weapon; 
        set { 
            _weapon?.UnEquip(); 
            value.Equip(this);
            _weapon = value;
        }
    }
    public Define.WeaponType WeaponType => Weapon.WeaponType;
    public Dictionary<Define.ArmorType, Armor> Armors { get; protected set; }
    
    [SerializeField]
    protected Vector3 _attackPosOffset;
    
    protected override void Init()
    {
        base.Init();
        
        CreatureType = Define.CreatureType.Hero;
        
        Head = Util.FindChild(gameObject, "head", true);
        LeftHand = Util.FindChild(gameObject, "weapon_l", true);
        RightHand = Util.FindChild(gameObject, "weapon_r", true);
        
        Bag = new Bag(transform);
        Armors = new Dictionary<Define.ArmorType, Armor>();
        foreach (Define.ArmorType type in (Define.ArmorType[])Enum.GetValues(typeof(Define.ArmorType)))
            Armors.TryAdd(type, null);
        
        // TODO - TEST CODE
        Managers.InputMng.KeyAction -= OnKeyboardClick;
        Managers.InputMng.KeyAction += OnKeyboardClick;
    }
    
    public override void SetInfo(int templateId)
    {
        base.SetInfo(templateId);
        
        Stat = new HeroStat(HeroData);
    }

    #region Weapon

    public void ChangeAnimator()
    {
        string path = "Animator Controllers/Players/" + WeaponType.ToString();
        Animator.runtimeAnimatorController = Resources.Load(path) as RuntimeAnimatorController; 
    }
    
    public void EquipWeapon(Weapon equippingWeapon)
    {
        _weapon?.UnEquip(); 
        equippingWeapon.Equip(this);
        _weapon = equippingWeapon;
    }
    
    public void UnEquipArmor()
    {
       _weapon?.UnEquip();
       _weapon = null;
    }
    
    public void ChangeWeaponVisibility(Define.WeaponSide weaponSide, int index, bool isActive)
    {
        switch (weaponSide)
        {
            case Define.WeaponSide.Left:
                LeftHand.transform.GetChild(index).gameObject.SetActive(isActive);
                break;
            case Define.WeaponSide.Right:
                RightHand.transform.GetChild(index).gameObject.SetActive(isActive);
                break;
        }
    }
    
    #endregion
    
    #region Armor
    public Armor GetArmor(Define.ArmorType armorType)
    {
        return Armors[armorType];
    }

    public void EquipArmor(Armor equippingArmor)
    {
        // Unequip previous armor
        var currentArmor = Armors[equippingArmor.ArmorType];
        if (currentArmor != null)
            currentArmor.UnEquip();
        else
            ChangeArmorVisibility(equippingArmor.ArmorType, 1, false);

        // Equip new armor
        Armors[equippingArmor.ArmorType] = equippingArmor;
        if (Armors[equippingArmor.ArmorType] != null)
            Armors[equippingArmor.ArmorType].Equip(this);
    }

    public void UnEquipArmor(Define.ArmorType armorType)
    {
        var currentArmor = Armors[armorType];
        if (currentArmor != null)
            currentArmor.UnEquip();

        Armors[armorType] = null;
    }

    public void ChangeArmorVisibility(Define.ArmorType armorType, int index, bool isActive)
    {
        switch (armorType)
        {
             case Define.ArmorType.Body: 
                 transform.GetChild(index - 1).gameObject.SetActive(isActive);
                 break; 
             case Define.ArmorType.Cloak:
                 transform.GetChild(index + 19).gameObject.SetActive(isActive);
                 break;
             case Define.ArmorType.HeadAccessory:
                 Head.transform.GetChild(index - 1).gameObject.SetActive(isActive);
                 break;
             case Define.ArmorType.Helmet:
                 Head.transform.GetChild(index + 96).gameObject.SetActive(isActive);
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
            EquipArmor(new SampleBody1());
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            EquipArmor(new SampleBody2());
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            EquipWeapon(new SampleSingleSword());
        else if (Input.GetKeyDown(KeyCode.Alpha6))
            EquipWeapon(new SampleSingleSword());
    }
}
