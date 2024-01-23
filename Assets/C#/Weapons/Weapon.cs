using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon
{
    protected Define.WeaponType _weaponType;
    protected PlayerController _equipper;
    
    public Dictionary<string, int> StatData { get; protected set; } // TODO : Network 쪽에서 enum Stat 만들어지면 교체
    public Define.WeaponType WeaponType { get => _weaponType; }
    
    public abstract void EffectAfterAttack();
    public abstract void Skill1();
    public abstract void Skill2();

    public Weapon(PlayerController equipper)
    {
        StatData = new Dictionary<string, int>();
        _equipper = equipper;
    }

    protected void LoadDataFromJson(string className)
    {
        var data = Managers.DataMng.WeaponDataDict[className];
        if (data.Hp != 0) StatData.TryAdd("Hp", data.Hp);
        if (data.Attack != 0) StatData.TryAdd("Attack", data.Attack);
        if (data.Defense != 0) StatData.TryAdd("Defense", data.Defense);
        if (data.Speed != 0) StatData.TryAdd("Speed", data.Speed);
        if (data.Dexterity != 0) StatData.TryAdd("Dexterity", data.Dexterity);
        if (data.Strength != 0) StatData.TryAdd("Strength", data.Strength);
        if (data.Vitality != 0) StatData.TryAdd("Vitality", data.Vitality);
        if (data.Intelligence != 0) StatData.TryAdd("Intelligence", data.Intelligence);
        
    }
    
    public void Equip()
    {
        _equipper.Stat.AttachEquipment(StatData);
        
        // debug
        Debug.Log($"Equipped Weapon");
    }

    public void UnEquip()
    {
        _equipper.Stat.DetachEquipment(StatData);
        
        // debug
        Debug.Log($"Unequipped Weapon");
    }
}