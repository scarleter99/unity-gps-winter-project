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

    public int Hp { get => _hp; set => _hp = value; }
    public int MaxHp { get => _maxHp; set => _maxHp = value; }
    public int Attack { get => _attack; set => _attack = value; }
    public int Defense { get => _defense; set => _defense = value; }
    public int Speed { get => _speed; set => _speed = value; }

    private void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        SetStat(gameObject.name);
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
}