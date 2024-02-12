using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;

[Serializable]
public struct HeroStat : IStat, INetworkSerializable
{
    private string _name;
    [SerializeField]
    private int _hp;
    [SerializeField]
    private int _maxHp;
    [SerializeField]
    private int _attack;
    [SerializeField]
    private int _defense;
    [SerializeField]
    private int _speed;
    [SerializeField] 
    private int _strength;
    [SerializeField]
    private int _vitality;
    [SerializeField] 
    private int _intelligence;
    [SerializeField]
    private int _dexterity;
    
    public string Name { get => _name; }
    public int Hp { get => _hp; set { _hp = value; StatChangeAction?.Invoke(this); } }
    public int MaxHp { get => _maxHp; set { _maxHp = value; StatChangeAction?.Invoke(this); } }
    public int Attack { get => _attack; set { _attack = value; StatChangeAction?.Invoke(this); } }
    public int Defense { get => _defense; set { _defense = value; StatChangeAction?.Invoke(this); } }
    public int Speed { get => _speed; set { _speed = value; StatChangeAction?.Invoke(this); } }
    public int Strength { get => _strength; set { _strength = value; StatChangeAction?.Invoke(this); } }
    public int Vitality { get => _vitality; set { _vitality = value; StatChangeAction?.Invoke(this); } }
    public int Intelligence { get => _intelligence; set { _intelligence = value; StatChangeAction?.Invoke(this); } }
    public int Dexterity { get => _dexterity; set { _dexterity = value; StatChangeAction?.Invoke(this); } }
    
    public Action<HeroStat> StatChangeAction;

    public HeroStat(Data.HeroData heroData)
    {
        StatChangeAction = null;
        _name = heroData.Name;
        _hp = heroData.Hp;
        _maxHp = heroData.Hp;
        _attack = heroData.Attack;
        _defense = heroData.Defense;
        _speed = heroData.Speed;
        _strength = heroData.Strength;
        _vitality = heroData.Vitality;
        _intelligence = heroData.Intelligence;
        _dexterity = heroData.Dexterity;
    }
    
    #region Event
    public void OnDamage(int damage, int attackCount = 1)
    {
        int trueDamage = Mathf.Max(damage - Defense, 1);
        if (attackCount > 1)
            trueDamage = Mathf.Max(trueDamage / attackCount, 1);

        Hp = Mathf.Clamp(Hp - trueDamage, 0, MaxHp);
    }

    public void OnHeal(int amount)
    {
        Hp = Mathf.Clamp(Hp + amount, 0, MaxHp);
    }
    
    /*public override void AddStat(Define.StatType statType, int amount)
{
    switch (statType)
    {
        case Define.StatType.Attack:
            Attack += amount;
            break;
    }
}

public override void MultiplyStat(Define.StatType statType, float rate)
{
    switch (statType)
    {
        case Define.StatType.Attack:
        Attack *= amount;
        break;
    }
}*/
    #endregion
    
    #region Equipment
    public void AttachEquipment(Data.EquipmentData equipmentData)
    {
        Hp += equipmentData.Hp;
        MaxHp += equipmentData.Hp;
        Attack += equipmentData.Attack;
        Defense += equipmentData.Defense;
        Speed += equipmentData.Speed;
        Strength += equipmentData.Strength;
        Vitality += equipmentData.Vitality;
        Intelligence += equipmentData.Intelligence;
        Dexterity += equipmentData.Dexterity;
    }

    public void DetachEquipment(Data.EquipmentData equipmentData)
    {
        Hp -= equipmentData.Hp;
        MaxHp -= equipmentData.Hp;
        Attack -= equipmentData.Attack;
        Defense -= equipmentData.Defense;
        Speed -= equipmentData.Speed;
        Strength -= equipmentData.Strength;
        Vitality -= equipmentData.Vitality;
        Intelligence -= equipmentData.Intelligence;
        Dexterity -= equipmentData.Dexterity;
    }
    #endregion
    
    #region Network
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref _dexterity);
        serializer.SerializeValue(ref _strength);
        serializer.SerializeValue(ref _vitality);
        serializer.SerializeValue(ref _intelligence);
    }
    #endregion
}