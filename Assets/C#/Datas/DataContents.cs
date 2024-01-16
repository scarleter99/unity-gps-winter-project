using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
    #region PlayerStat

    [Serializable]
    public class PlayerStat
    {
        public string name;
        public int hp;
        public int attack;
        public int defense;
        public int strength;
        public int vitality;
        public int intelligence;
        public int speed;
    }

    [Serializable]
    public class PlayerStatData : IData<string, PlayerStat>
    {
        public List<PlayerStat> stats = new List<PlayerStat>();

        // List형태의 Data를 Dictionary형태로 변환 후 반환
        public Dictionary<string, PlayerStat> MakeDict()
        {
            Dictionary<string, PlayerStat> dic = new Dictionary<string, PlayerStat>();
            foreach (PlayerStat stat in stats)
                dic.Add(stat.name, stat);

            return dic;
        }
    }

    #endregion

    #region MonsterStat

    [Serializable]
    public class MonsterStat
    {
        public string name;
        public int hp;
        public int attack;
        public int defense;
        public int speed;
    }

    [Serializable]
    public class MonsterStatData : IData<string, MonsterStat>
    {
        public List<MonsterStat> stats = new List<MonsterStat>();

        // List형태의 Data를 Dictionary형태로 변환 후 반환
        public Dictionary<string, MonsterStat> MakeDict()
        {
            Dictionary<string, MonsterStat> dic = new Dictionary<string, MonsterStat>();
            foreach (MonsterStat stat in stats)
                dic.Add(stat.name, stat);

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
    
    #region BagItem

    public class BagItem
    {
        public BaseItem item;
        public int count;
        public BagItem() { item = null; count = 0; }
        public BagItem(BaseItem item, int count) { this.item = item; this.count = count; }
        public bool IsNull() { return item == null && count == 0; }
    }

    #endregion
}