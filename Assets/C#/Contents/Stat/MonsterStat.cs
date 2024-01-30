using System;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public struct MonsterStat: IStat, INetworkSerializable
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

    public string Name { get => _name; }
    public int Hp { get => _hp; set { _hp = value; StatChangeAction?.Invoke(this); } }
    public int MaxHp { get => _maxHp; set { _maxHp = value; StatChangeAction?.Invoke(this); } }
    public int Attack { get => _attack; set { _attack = value; StatChangeAction?.Invoke(this); } }
    public int Defense { get => _defense; set { _defense = value; StatChangeAction?.Invoke(this); } }
    public int Speed { get => _speed; set { _speed = value; StatChangeAction?.Invoke(this); } }

    public Action<MonsterStat> StatChangeAction;

    public MonsterStat(int templateId)
    {
        StatChangeAction = null;
        Data.MonsterData data = Managers.DataMng.MonsterDataDict[templateId];
        _name = data.name;
        _hp = data.hp;
        _maxHp = data.hp;
        _attack = data.attack;
        _defense = data.defense;
        _speed = data.speed;
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
    #endregion
    
    #region Network
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref _name);
        serializer.SerializeValue(ref _hp);
        serializer.SerializeValue(ref _maxHp);
        serializer.SerializeValue(ref _attack);
        serializer.SerializeValue(ref _defense);
        serializer.SerializeValue(ref _speed);
    }
    #endregion
} 