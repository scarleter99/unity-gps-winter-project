using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

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
        public int dexterity;
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

    #region AreaData
    [Serializable]
    public class AreaData
    {
        public string name; // AreaName enum의 값과 같아야 함
        public string basemap;
        public int width;
        public int height;
        public int battleTileNum;
        public int encounterTileNum;
        public string mapPrefabPath;
    }

    [Serializable]
    public class AreaDataSet : IData<Define.AreaName, AreaData>
    {
        public List<AreaData> areadatas = new();

        public Dictionary<Define.AreaName, AreaData> MakeDict()
        {   
            var dic = new Dictionary<Define.AreaName, AreaData>();
            foreach (AreaData areadata in areadatas)
            {   
                if (Enum.TryParse(areadata.name, out Define.AreaName areaName))
                {   
                    dic.Add(areaName, areadata);
                }
                else
                {
                    Debug.LogError($"{areadata.name} - No such area name in enum!");
                }
            }
            return dic; 
        }
    }
    #endregion
    
    #region WeaponData
    [Serializable]
    public class Weapon
    {
        public string Name;
        public int Hp;
        public int Attack;
        public int Defense;
        public int Speed;
        public int Dexterity;
        public int Strength;
        public int Vitality;
        public int Intelligence;
        public int Left;
        public int Right;
    }

    [Serializable]
    public class WeaponData : IData<string, Weapon>
    {
        public List<Weapon> weapons = new List<Weapon>();

        public Dictionary<string, Weapon> MakeDict()
        {   
            var dic = new Dictionary<string, Weapon>();
            foreach (var weapon in weapons)
                dic.Add(weapon.Name, weapon);

            return dic;
        }
    }
    #endregion
    
    #region ArmorData
    
    [Serializable]
    public class Armor
    {
        public string Name;
        public int Hp;
        public int Attack;
        public int Defense;
        public int Speed;
        public int Dexterity;
        public int Strength;
        public int Vitality;
        public int Intelligence;
        public int Index;
    }

    [Serializable]
    public class ArmorData : IData<string, Armor>
    {
        public List<Armor> armors = new List<Armor>();

        public Dictionary<string, Armor> MakeDict()
        {   
            var dic = new Dictionary<string, Armor>();
            foreach (var armor in armors)
                dic.Add(armor.Name, armor);

            return dic;
        }
    }
    
    #endregion
    
    public struct TestStruct : INetworkSerializable
    {
        public Vector3 Position;
        public int Hp;
        
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref Position);
            serializer.SerializeValue(ref Hp);
        }
    }

}