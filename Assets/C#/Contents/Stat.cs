using System;
using Unity.Netcode;
using UnityEngine;

public struct StatStruct : INetworkSerializable
{
    public string Name;
    public int Hp;
    public int MaxHp;
    public int Attack;
    public int Defense;
    public int Speed;
    
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref Name);
        serializer.SerializeValue(ref Hp);
        serializer.SerializeValue(ref MaxHp);
        serializer.SerializeValue(ref Attack);
        serializer.SerializeValue(ref Defense);
        serializer.SerializeValue(ref Speed);
    }
}

[Serializable]
public class Stat
{
    protected string _name;
    [SerializeField]
    protected int _hp;
    [SerializeField]
    protected int _maxHp;
    [SerializeField]
    protected int _attack;
    [SerializeField]
    protected int _defense;
    [SerializeField]
    protected int _speed;

    public string Name { get => _name; }
    public int Hp { get => _hp; set { _hp = value; OnStatChanged?.Invoke(this); } }
    public int MaxHp { get => _maxHp; set { _maxHp = value; OnStatChanged?.Invoke(this); } }
    public int Attack { get => _attack; set { _attack = value; OnStatChanged?.Invoke(this); } }
    public int Defense { get => _defense; set { _defense = value; OnStatChanged?.Invoke(this); } }
    public int Speed { get => _speed; set { _speed = value; OnStatChanged?.Invoke(this); } }

    public Action<Stat> OnStatChanged;

    public Stat(string name)
    {
        SetStat(name);
    }

    public virtual void OnDamage(Stat attacker, int attackCount = 1)
    {
        int damage = Mathf.Max(attacker.Attack - Defense, 1);
        if (attackCount > 1)
            damage = Mathf.Max(damage / attackCount, 1);

        Hp = Mathf.Clamp(Hp - damage, 0, MaxHp);
    }

    public virtual void SetStat(string name)
    {
        Data.MonsterStat stat = Managers.DataMng.MonsterStatDict[name];
        _name = name;
        Hp = stat.hp;
        MaxHp = stat.hp;
        Attack = stat.attack;
        Defense = stat.defense;
        Speed = stat.speed;
    }

    public void RecoverHp(int amount)
    {
        Hp = Mathf.Clamp(Hp + amount, 0, MaxHp);
    }

    /*public virtual void AddStat(Define.StatType statType, int amount)
    {
        switch (statType)
        {
            case Define.StatType.Attack:
                Attack += amount;
                break;
        }
    }
    
    public virtual void MultiplyStat(Define.StatType statType, float rate)
    {
        switch (statType)
        {
            case Define.StatType.Attack:
                Attack *= amount;
                break;
        }
    }*/
} 