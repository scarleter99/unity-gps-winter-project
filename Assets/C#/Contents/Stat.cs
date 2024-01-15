using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Stat : MonoBehaviour
{
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

    public int Hp { get => _hp; set { _hp = value; OnStatChanged?.Invoke(this); } }
    public int MaxHp { get => _maxHp; set { _maxHp = value; OnStatChanged?.Invoke(this); } }
    public int Attack { get => _attack; set { _attack = value; OnStatChanged?.Invoke(this); } }
    public int Defense { get => _defense; set { _defense = value; OnStatChanged?.Invoke(this); } }
    public int Speed { get => _speed; set { _speed = value; OnStatChanged?.Invoke(this); } }

    public Action<Stat> OnStatChanged;

    private void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        SetStat(gameObject.name);
    }

    public virtual void OnAttacked(Stat attacker)
    {
        Hp -= Mathf.Max(0, attacker.Attack - Defense);
        if (Hp <= 0)
        {
            Hp = 0;
            OnDead(attacker);
        }
    }
    
    protected virtual void OnDead(Stat attacker) { }

    public virtual void SetStat(string name)
    {
        Data.MonsterStat stat = Managers.DataMng.MonsterStatDict[name];
        Hp = stat.hp;
        MaxHp = stat.hp;
        Attack = stat.attack;
        Defense = stat.defense;
        Speed = stat.speed;
    }
}