using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
    #region CreatureData
    [Serializable]
    public class CreatureData
    {
        public int DataId;
        public string Name;
        public int Hp;
        public int Attack;
        public int Defense;
        public int Speed;
    }
    #endregion
    
    #region HeroData

    [Serializable]
    public class HeroData : CreatureData
    {
        public int Strength;
        public int Vitality;
        public int Intelligence;
        public int Dexterity;
    }

    [Serializable]
    public class HeroDataLoader : IData<int, HeroData>
    {
        public List<HeroData> heroes = new List<HeroData>();

        // List형태의 Data를 Dictionary형태로 변환 후 반환
        public Dictionary<int, HeroData> MakeDict()
        {
            Dictionary<int, HeroData> dic = new Dictionary<int, HeroData>();
            foreach (HeroData stat in heroes)
                dic.Add(stat.DataId, stat);

            return dic;
        }
    }

    #endregion

    #region MonsterData

    [Serializable]
    public class MonsterData : CreatureData
    {
    }

    [Serializable]
    public class MonsterDataLoader : IData<int, MonsterData>
    {
        public List<MonsterData> monsters = new List<MonsterData>();

        // List형태의 Data를 Dictionary형태로 변환 후 반환
        public Dictionary<int, MonsterData> MakeDict()
        {
            Dictionary<int, MonsterData> dic = new Dictionary<int, MonsterData>();
            foreach (MonsterData stat in monsters)
                dic.Add(stat.DataId, stat);

            return dic;
        }
    }

    #endregion

    #region ItemData

    [Serializable]
    public class ItemData
    {
        public int DataId;
        public string Name;
        public string Description;
        public int Heal;
    }

    [Serializable]
    public class ItemDataLoader : IData<int, ItemData>
    {
        public List<ItemData> items = new List<ItemData>();

        public Dictionary<int, ItemData> MakeDict()
        {   
            var dic = new Dictionary<int, ItemData>();
            foreach (ItemData item in items)
                dic.Add(item.DataId, item);

            return dic;
        }
    }
    #endregion
    
    #region EquipmentData
    [Serializable]
    public class EquipmentData
    {
        public int DataId;
        public string Name;
        public int Hp;
        public int Attack;
        public int Defense;
        public int Speed;
        public int Dexterity;
        public int Strength;
        public int Vitality;
        public int Intelligence;
    }
    #endregion
    
    #region WeaponData
    [Serializable]
    public class WeaponData : EquipmentData
    {
        public int LeftIndex;
        public int RightIndex;
    }

    [Serializable]
    public class WeaponDataLoader : IData<int, WeaponData>
    {
        public List<WeaponData> weapons = new List<WeaponData>();

        public Dictionary<int, WeaponData> MakeDict()
        {   
            var dic = new Dictionary<int, WeaponData>();
            foreach (var weapon in weapons)
                dic.Add(weapon.DataId, weapon);

            return dic;
        }
    }
    #endregion
    
    #region ArmorData
    [Serializable]
    public class ArmorData : EquipmentData
    {
        public int ArmorIndex;
    }

    [Serializable]
    public class ArmorDataLoader : IData<int, ArmorData>
    {
        public List<ArmorData> armors = new List<ArmorData>();

        public Dictionary<int, ArmorData> MakeDict()
        {   
            var dic = new Dictionary<int, ArmorData>();
            foreach (var armor in armors)
                dic.Add(armor.DataId, armor);

            return dic;
        }
    }
    #endregion

    #region SkillData
    [Serializable]
    public class SkillData : EquipmentData
    {
        public int DataId;
        public string Name;
        public string Description;
        public int CoinNum;
        public int ReducedStat;
    }

    [Serializable]
    public class SkillDataLoader : IData<int, SkillData>
    {
        public List<SkillData> skills = new List<SkillData>();

        public Dictionary<int, SkillData> MakeDict()
        {   
            var dic = new Dictionary<int, SkillData>();
            foreach (var skill in skills)
                dic.Add(skill.DataId, skill);

            return dic;
        }
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
}