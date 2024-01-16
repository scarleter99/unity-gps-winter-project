using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    [SerializeField]
    protected int _gold;
    [SerializeField] 
    protected int _strength;
    [SerializeField]
    protected int _vitality;
    [SerializeField] 
    protected int _intelligence;
    
    public int Gold { get => _gold; set { _gold = value; OnStatChanged?.Invoke(this); } }
    public int Strength { get => _strength; set { _strength = value; OnStatChanged?.Invoke(this); } }
    public int Vitality { get => _vitality; set { _vitality = value; OnStatChanged?.Invoke(this); } }
    public int Intelligence { get => _intelligence; set { _intelligence = value; OnStatChanged?.Invoke(this); } }
    
    protected override void Init()
    {
        base.Init();
        Gold = 0;
    }

    public override void SetStat(string name)
    {
        Data.PlayerStat stat = Managers.DataMng.PlayerStatDict[name];
        Hp = stat.hp;
        MaxHp = stat.hp;
        Attack = stat.attack;
        Defense = stat.defense;
        Speed = stat.speed;
        Strength = stat.strength;
        Vitality = stat.vitality;
        Intelligence = stat.intelligence;
    }
}