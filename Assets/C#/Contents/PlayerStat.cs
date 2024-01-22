using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;

[Serializable]
public struct PlayerStat : IStat, INetworkSerializable
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
    public int Hp { get => _hp; set { _hp = value; OnStatChanged?.Invoke(this); } }
    public int MaxHp { get => _maxHp; set { _maxHp = value; OnStatChanged?.Invoke(this); } }
    public int Attack { get => _attack; set { _attack = value; OnStatChanged?.Invoke(this); } }
    public int Defense { get => _defense; set { _defense = value; OnStatChanged?.Invoke(this); } }
    public int Speed { get => _speed; set { _speed = value; OnStatChanged?.Invoke(this); } }
    public int Dexterity { get => _dexterity; set { _dexterity = value; OnStatChanged?.Invoke(this); } }
    public int Strength { get => _strength; set { _strength = value; OnStatChanged?.Invoke(this); } }
    public int Vitality { get => _vitality; set { _vitality = value; OnStatChanged?.Invoke(this); } }
    public int Intelligence { get => _intelligence; set { _intelligence = value; OnStatChanged?.Invoke(this); } }
    public Action<PlayerStat> OnStatChanged;

    public PlayerStat(string name)
    {
        OnStatChanged = null;
        Data.PlayerStat stat = Managers.DataMng.PlayerStatDict[name];
        _name = name;
        _hp = stat.hp;
        _maxHp = stat.hp;
        _attack = stat.attack;
        _defense = stat.defense;
        _dexterity = stat.dexterity;
        _speed = stat.speed;
        _strength = stat.strength;
        _vitality = stat.vitality;
        _intelligence = stat.intelligence;
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
    public void AttachEquipment(Dictionary<string, int> equipmentStats)
    {
        foreach (var currentStat in equipmentStats)
        {
            var prop = GetType().GetProperty(currentStat.Key);
            if (prop != null)
                prop.SetValue(this, (int)prop.GetValue(this) + currentStat.Value);
        }
    }

    public void DetachEquipment(Dictionary<string, int> equipmentStats)
    {
        foreach (var currentStat in equipmentStats)
        {
            var prop = GetType().GetProperty(currentStat.Key);
            if (prop != null)
                prop.SetValue(this, (int)prop.GetValue(this) - currentStat.Value);
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