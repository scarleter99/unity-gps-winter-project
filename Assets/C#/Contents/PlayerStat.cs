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
    [SerializeField] 
    protected int _strength;
    [SerializeField]
    protected int _vitality;
    [SerializeField] 
    protected int _intelligence;
    [SerializeField]
    protected int _cognition;
    [SerializeField]
    protected int _talent;
    [SerializeField]
    protected int _speed;
    [SerializeField]
    protected int _luck;
    
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
                RaiseStats(1);
                Debug.Log($"Level Up!: {Level}");
            }
        }
    }
    public int Gold { get => _gold; set => _gold = value; }
    public int Strength { get => _strength; set => _strength = value; }
    public int Vitality { get => _vitality; set => _vitality = value; }
    public int Intellignence { get => _intelligence; set => _intelligence = value; }
    public int Cognition { get => _cognition; set => _cognition = value; }
    public int Talent { get => _talent; set => _talent = value; }
    public int Speed { get => _speed; set => _speed = value; }
    public int Luck { get => _luck; set => _luck = value; }
    
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

    protected void RaiseStats(int amount)
    {
        _strength += amount;
        _vitality += amount;
        _intelligence += amount;
        _cognition += amount;
        _talent += amount;
        _speed += amount;
        _luck += amount;
    }
    
    protected override void OnDead(Stat attacker)
    {
        
    }
}