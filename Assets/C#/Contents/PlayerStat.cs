using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    [SerializeField]
    protected int _exp;
    [SerializeField]
    protected int _gold;

    public int Exp
    {
        get => _exp;
        set
        {
            _exp = value;

            int level = Level;
            while (true)
            {
                Data.Stat stat;
                if (Managers.DataMng.StatDict.TryGetValue(level + 1, out stat) == false)
                    break;
                if (_exp < stat.totalExp)
                    break;
                level++;
            }

            if (level != Level)
            {
                Level = level;
                SetStat(Level);
                Debug.Log($"Level Up!: {Level}");
            }
        }
    }
    public int Gold { get => _gold; set => _gold = value; }
    
    private void Start()
    {
        _level = 1;
        _defense = 5;
        _moveSpeed = 5.0f;
        _exp = 0;
        _gold = 0;
        
        SetStat(_level);
    }

    public void SetStat(int level)
    {
        Data.Stat stat = Managers.DataMng.StatDict[1];

        _hp = stat.maxHp;
        _maxHp = stat.maxHp;
        _attack = stat.attack;
    }
    
    protected override void OnDead(Stat attacker)
    {
        
    }
}