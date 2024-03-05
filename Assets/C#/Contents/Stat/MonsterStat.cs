using System;
using Unity.Netcode;
using UnityEngine;

public class MonsterStat: CreatureStat
{
    private int _speed;
    
    public int Speed { get => _speed; set { _speed = value; StatChangeAction?.Invoke(this); } }

    public override void SetStat(Data.CreatureData creatureData)
    {
        base.SetStat(creatureData);

        Data.MonsterData monsterData = (Data.MonsterData)creatureData;
        _speed = monsterData.Speed;
    }
    
    public override int GetStatByDefine(Define.Stat stat)
    {
        switch (stat)
        {
            case Define.Stat.Hp:
                return Hp;
            case Define.Stat.MaxHp:
                return MaxHp;
            case Define.Stat.Attack:
                return Attack;
            case Define.Stat.Defense:
                return Defense;
            case Define.Stat.Speed:
                return Speed;
        }

        return -1;
    }
} 