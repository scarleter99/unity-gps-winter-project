using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HeroController : CreatureController
{
    public GameObject Head { get; protected set; }
    public GameObject LeftHand { get; protected set; }
    public GameObject RightHand { get; protected set; }
    
    public Bag Bag { get; protected set; }
    public Define.WeaponType WeaponType { get; protected set; }
    protected Weapon _weapon;
    public Weapon Weapon 
    { 
        get => _weapon; 
        set { 
            _weapon?.UnEquip(); 
            value.Equip();
            _weapon = value;
        }
    }
    protected Dictionary<Define.ArmorType, Armor> _armors;
    
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
        _armors = new Dictionary<Define.ArmorType, Armor>();
        foreach (Define.ArmorType type in (Define.ArmorType[])Enum.GetValues(typeof(Define.ArmorType)))
            _armors.TryAdd(type, null);
        
        Managers.InputMng.MouseAction -= OnMouseEvent;
        Managers.InputMng.MouseAction += OnMouseEvent;
        Managers.InputMng.KeyAction -= OnKeyboard;
        Managers.InputMng.KeyAction += OnKeyboard;
    }

    public HeroStat GetHeroStat()
    {
        HeroStat heroStat = (HeroStat)Stat;
        return heroStat;
    }

    #region WeaponData

    public void ChangeAnimator()
    {
        string path = "Animator Controllers/Players/" + WeaponType.ToString();
        Animator.runtimeAnimatorController = Resources.Load(path) as RuntimeAnimatorController; 
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
    
    #region ArmorData
    public Armor Armor(Define.ArmorType armorType)
    {
        return _armors[armorType];
    }

    public void ChangeArmor(Armor equippingArmor)
    {
        // Unequip previous armor
        var currentArmor = _armors[equippingArmor.ArmorType];
        if (currentArmor != null)
            currentArmor.UnEquip();
        else
            ChangeArmorVisibility(equippingArmor.ArmorType, 1, false);

        // Equip new armor
        _armors[equippingArmor.ArmorType] = equippingArmor;
        if (_armors[equippingArmor.ArmorType] != null)
            _armors[equippingArmor.ArmorType].Equip();
    }

    public void UnEquipArmor(Define.ArmorType armorType)
    {
        var currentArmor = _armors[armorType];
        if (currentArmor != null)
            currentArmor.UnEquip();

        _armors[armorType] = null;
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

    #region Event

    // 적절한 Animation Timing에서 호출
    protected override void OnAttackEvent()
    {
        if (LockTarget != null)
        {
            switch (WeaponType)
            {
                case Define.WeaponType.DoubleSword:
                    LockTarget.GetComponent<CreatureController>().OnDamage(this, 2);
                    break;
                default:
                    LockTarget.GetComponent<CreatureController>().OnDamage(this);
                    break;
            }
        }
    }
    
    // 적절한 Animation Timing에서 호출
    protected override void OnJumpStart()
    {
        switch (AnimState) 
        {
            case Define.AnimState.JumpBack:
                transform.DOMove(_comebackPos, 0.433f);
                break;
            case Define.AnimState.JumpFront:
                transform.DOMove(LockTarget.transform.position + _attackPosOffset, 0.433f);
                break;
        }
    }

    protected void OnKeyboard()
    {
        //////////////////////////////////////////
        // TODO - Test Code
        if (Input.GetKeyDown(KeyCode.Alpha1))
            Bag.UseItem(this, 0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            Bag.UseItem(this, 1);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            ChangeArmor(new SampleBody1(this));
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            ChangeArmor(new SampleBody2(this));
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            Weapon = new SampleSingleSword(this);
        else if (Input.GetKeyDown(KeyCode.Alpha6))
            Weapon = new SampleSwordAndShield(this);
        //////////////////////////////////////////
    }

    private void OnMouseEvent(Define.MouseEvent evt)
    {
        // if (TurnState != Define.TurnState.Action)
        //     return;

        if (AnimState != Define.AnimState.Idle)
            return;
        
        switch (evt)
        {
            case Define.MouseEvent.PointerDown:
                break;
        }
    }
    
    public override void OnDamage(CreatureController attacker, int amount = 1) 
    {
        var nextState = (AnimState == Define.AnimState.Defend) ? Define.AnimState.Defend : Define.AnimState.Hit;
        var monsterAttacker = attacker as MonsterController;
        Stat.OnDamage(monsterAttacker.Stat.Attack, amount);
        nextState = (Stat.Hp > 0) ? nextState : Define.AnimState.Die;
        AnimState = nextState;
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

}
