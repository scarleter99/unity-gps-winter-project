using System;
using Unity.Netcode;
using UnityEngine;

public struct PlayerStatStruct : INetworkSerializable
{
    public int Dexterity;
    public int Strength;
    public int Vitality;
    public int Intelligence;
    
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref Dexterity);
        serializer.SerializeValue(ref Strength);
        serializer.SerializeValue(ref Vitality);
        serializer.SerializeValue(ref Intelligence);
    }
}

[Serializable]
public class PlayerStat : Stat
{
    [SerializeField]
    protected int _gold;
    [SerializeField]
    protected int _dexterity;
    [SerializeField] 
    protected int _strength;
    [SerializeField]
    protected int _vitality;
    [SerializeField] 
    protected int _intelligence;
    
    public int Gold { get => _gold; set { _gold = value; OnStatChanged?.Invoke(this); } }
    public int Dexterity { get => _dexterity; set { _dexterity = value; OnStatChanged?.Invoke(this); } }
    public int Strength { get => _strength; set { _strength = value; OnStatChanged?.Invoke(this); } }
    public int Vitality { get => _vitality; set { _vitality = value; OnStatChanged?.Invoke(this); } }
    public int Intelligence { get => _intelligence; set { _intelligence = value; OnStatChanged?.Invoke(this); } }

    public PlayerStat(string name) : base(name)
    {
        Gold = 0;
    }

    public override void SetStat(string name)
    {
        Data.PlayerStat stat = Managers.DataMng.PlayerStatDict[name];
        _name = name;
        Hp = stat.hp;
        MaxHp = stat.hp;
        Attack = stat.attack;
        Defense = stat.defense;
        Dexterity = stat.dexterity;
        Speed = stat.speed;
        Strength = stat.strength;
        Vitality = stat.vitality;
        Intelligence = stat.intelligence;
    }
}