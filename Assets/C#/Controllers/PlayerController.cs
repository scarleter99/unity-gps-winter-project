using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerController : BaseController
{
    private GameObject _leftHand;
    private GameObject _rightHand;
    
    [ReadOnly(false), SerializeField]
    protected PlayerStat _stat;
    protected Define.WeaponType _weaponType;
    protected Bag _bag;
    protected Weapon _weapon;
    protected Dictionary<Define.ArmorType, Armor> _armors;
    
    public ref PlayerStat Stat { get => ref _stat; }
    public Define.WeaponType WeaponType { get => _weaponType; set => _weaponType = value; }
    public Bag Bag { get => _bag; }
    public Weapon Weapon 
    { 
        get => _weapon; 
        set { 
            _weapon?.UnEquip(); 
            value.Equip();
            _weapon = value;
        }
    }
    
    public override void Init()
    {
        WorldObjectType = Define.WorldObject.Player;
        _bag = new Bag(transform);
        _stat = new PlayerStat(name); // TODO : 플레이어 닉네임 설정하면 여기에 할당
        _leftHand = Util.FindChild(gameObject, "weapon_l", true);
        _rightHand = Util.FindChild(gameObject, "weapon_r", true);
        foreach (Define.ArmorType type in (Define.ArmorType[])Enum.GetValues(typeof(Define.ArmorType)))
            _armors.TryAdd(type, null);
        
        Managers.InputMng.MouseAction -= OnMouseEvent;
        Managers.InputMng.MouseAction += OnMouseEvent;
        Managers.InputMng.KeyAction -= OnKeyboard;
        Managers.InputMng.KeyAction += OnKeyboard;
    }
    
    public void PlayerStatChange(PlayerStat statStruct)
    {
        // TODO
    }
    
   
    #region Weapon

    public void ChangeAnimator()
    {
        string path = "Animator Controllers/Players/" + WeaponType.ToString();
        _animator.runtimeAnimatorController = Resources.Load(path) as RuntimeAnimatorController; 
    }
    
    public void ChangeWeaponVisibility(Define.WeaponSide weaponSide, int index, bool isActive)
    {
        switch (weaponSide)
        {
            case Define.WeaponSide.Left:
                _leftHand.transform.GetChild(index).gameObject.SetActive(isActive);
                break;
            case Define.WeaponSide.Right:
                _rightHand.transform.GetChild(index).gameObject.SetActive(isActive);
                break;
        }
    }
    
    #endregion
    
    #region Armor
    public Armor Armor(Define.ArmorType armorType)
    {
        return _armors[armorType];
    }

    public void EquipArmor(Define.ArmorType armorType, Armor equippingArmor)
    {
        var currentArmor = _armors[armorType];
        if (currentArmor != null)
            currentArmor.UnEquip();
        else
            ChangeArmorVisibility(armorType, 0, false);

        currentArmor = equippingArmor;
        if (currentArmor != null)
            currentArmor.Equip();
    }
    
    public void UnEquipArmor(Define.ArmorType armorType) => EquipArmor(armorType, null);

    public void ChangeArmorVisibility(Define.ArmorType armorType, int index, bool isActive)
    {
        // TODO
    }
    #endregion

    #region Event

    // 적절한 Animation Timing에서 호출
    protected override void OnAttackEvent()
    {
        if (_lockTarget != null)
        {
            switch (WeaponType)
            {
                case Define.WeaponType.DoubleSword:
                    _lockTarget.GetComponent<BaseController>().OnDamage(this, 2);
                    break;
                default:
                    _lockTarget.GetComponent<BaseController>().OnDamage(this);
                    break;
            }
        }
    }

    protected void OnKeyboard()
    {
        // temp - for test
        if (Input.GetKeyDown(KeyCode.Alpha1))
            Bag.UseItem(this, 0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            Bag.UseItem(this, 1);
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            Weapon = new SampleSingleSword(this);
        else if (Input.GetKeyDown(KeyCode.Alpha6))
            Weapon = new SampleSwordAndShield(this);
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
    
    public override void OnDamage(BaseController attacker, int amount = 1) 
    {
        var nextState = (AnimState == Define.AnimState.Defend) ? Define.AnimState.Defend : Define.AnimState.Hit;
        var monsterAttacker = attacker as MonsterController;
        Stat.OnDamage(monsterAttacker.Stat.Attack, amount);
        nextState = (_stat.Hp > 0) ? nextState : Define.AnimState.Die;
        AnimState = nextState;
    }
    #endregion
    
    #region Update
    
    protected override void UpdateAttack()
    {
        var currentState = _animator.GetCurrentAnimatorStateInfo(0);
        if (currentState.normalizedTime >= 0.8f && currentState.shortNameHash == _stateHash)
            AnimState = Define.AnimState.Idle;
    }

    protected override void UpdateHit()
    {
        var currentState = _animator.GetCurrentAnimatorStateInfo(0);
        if (currentState.normalizedTime >= 0.8f && currentState.shortNameHash == _stateHash)
            AnimState = Define.AnimState.Idle;
    }

    #endregion

}
