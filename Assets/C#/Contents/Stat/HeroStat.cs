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
    private int _dexterity;
    [SerializeField] 
    private int _strength;
    [SerializeField]
    private int _vitality;
    [SerializeField] 
    private int _intelligence;
    
    public string Name { get => _name; }
    public int Hp { get => _hp; set { _hp = value; StatChangeAction?.Invoke(this); } }
    public int MaxHp { get => _maxHp; set { _maxHp = value; StatChangeAction?.Invoke(this); } }
    public int Attack { get => _attack; set { _attack = value; StatChangeAction?.Invoke(this); } }
    public int Defense { get => _defense; set { _defense = value; StatChangeAction?.Invoke(this); } }
    public int Speed { get => _speed; set { _speed = value; StatChangeAction?.Invoke(this); } }
    public int Dexterity { get => _dexterity; set { _dexterity = value; StatChangeAction?.Invoke(this); } }
    public int Strength { get => _strength; set { _strength = value; StatChangeAction?.Invoke(this); } }
    public int Vitality { get => _vitality; set { _vitality = value; StatChangeAction?.Invoke(this); } }
    public int Intelligence { get => _intelligence; set { _intelligence = value; StatChangeAction?.Invoke(this); } }
    public Action<HeroStat> StatChangeAction;

    public HeroStat(int templateId)
    {
        StatChangeAction = null;
        Data.HeroData data = Managers.DataMng.HeroDataDict[templateId];
        _name = data.name;
        _hp = data.hp;
        _maxHp = data.hp;
        _attack = data.attack;
        _defense = data.defense;
        _dexterity = data.dexterity;
        _speed = data.speed;
        _strength = data.strength;
        _vitality = data.vitality;
        _intelligence = data.intelligence;
    }
    
    #region Event
    public void OnDamage(int attackerAttack, int attackCount = 1)
    {
        int damage = Mathf.Max(attackerAttack - Defense, 1);
        if (attackCount > 1)
            damage = Mathf.Max(damage / attackCount, 1);

        Hp = Mathf.Clamp(Hp - damage, 0, MaxHp);
    }

    public void RecoverHp(int amount)
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
    public void AttachEquipment(Dictionary<Define.Stat, int> equipmentStats)
    {
        foreach (var currentStat in equipmentStats)
        {
            switch (currentStat.Key)
            {
                case Define.Stat.Hp:
                    Hp += currentStat.Value;
                    MaxHp += currentStat.Value;
                    break;
                case Define.Stat.Attack:
                    Attack += currentStat.Value;
                    break;
                case Define.Stat.Defense:
                    Defense += currentStat.Value;
                    break;
                case Define.Stat.Dexterity:
                    Dexterity += currentStat.Value;
                    break;
                case Define.Stat.Speed:
                    Speed += currentStat.Value;
                    break;
                case Define.Stat.Strength:
                    Strength += currentStat.Value;
                    break;
                case Define.Stat.Vitality:
                    Vitality += currentStat.Value;
                    break;
                case Define.Stat.Intelligence:
                    Intelligence += currentStat.Value;
                    break;
            }
        }
    }

    public void DetachEquipment(Dictionary<Define.Stat, int> equipmentStats)
    {
        foreach (var currentStat in equipmentStats)
        {
            switch (currentStat.Key)
            {
                case Define.Stat.Hp:
                    Hp = Math.Max(Hp - currentStat.Value, 1);
                    MaxHp -= currentStat.Value;
                    break;
                case Define.Stat.Attack:
                    Attack -= currentStat.Value;
                    break;
                case Define.Stat.Defense:
                    Defense -= currentStat.Value;
                    break;
                case Define.Stat.Dexterity:
                    Dexterity -= currentStat.Value;
                    break;
                case Define.Stat.Speed:
                    Speed -= currentStat.Value;
                    break;
                case Define.Stat.Strength:
                    Strength -= currentStat.Value;
                    break;
                case Define.Stat.Vitality:
                    Vitality -= currentStat.Value;
                    break;
                case Define.Stat.Intelligence:
                    Intelligence -= currentStat.Value;
                    break;
            }
        }
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