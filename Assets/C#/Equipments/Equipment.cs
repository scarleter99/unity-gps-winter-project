using System.Collections.Generic;
using UnityEngine;

public abstract class Equipment
{
    public Dictionary<Define.Stat, int> StatData { get; protected set; }
    protected PlayerController _equipper;
    
    public Equipment(PlayerController equipper)
    {
        StatData = new Dictionary<Define.Stat, int>();
        _equipper = equipper;
    }
    
    public abstract void Equip();
    public abstract void UnEquip();
    protected abstract void LoadDataFromJson(string className);
}
