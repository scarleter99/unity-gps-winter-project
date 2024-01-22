using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.XR;

[System.Serializable]
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
}