﻿using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
#region Stat
    [Serializable]
    public class Stat
    {
        public int level;
        public int maxHp;
        public int attack;
        public int totalExp;
    }
    
    [Serializable]
    public class StatData : IData<int, Stat>
    {
        public List<Stat> stats = new List<Stat>();
        
        // List형태의 Data를 Dictionary형태로 변환 후 반환
        public Dictionary<int, Stat> MakeDict()
        {
            Dictionary<int, Stat> dic = new Dictionary<int, Stat>();
            foreach (Stat stat in stats)
                dic.Add(stat.level, stat);
            
            return dic;
        }
    }
#endregion

#region Item
    [Serializable]
    public class Item
    {
        public string name;
        public string description;
    }
    
    [Serializable]
    public class ItemData : IData<string, Item>
    {
        public List<Item> items = new List<Item>();

        public Dictionary<string, Item> MakeDict()
        {
            var dic = new Dictionary<string, Item>();
            foreach (Item item in items)
                dic.Add(item.name, item);
            
            return dic;
        }
    }
#endregion
}