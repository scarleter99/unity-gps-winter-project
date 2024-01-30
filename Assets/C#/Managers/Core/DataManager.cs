using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IData<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

// 시작하면 바로 데이터를 Load하여 Dict로 관리
public class DataManager
{
    public Dictionary<int, Data.HeroData> HeroDataDict { get; private set; }
    public Dictionary<int, Data.MonsterData> MonsterDataDict { get; private set; }
    public Dictionary<int, Data.ItemData> ItemDataDict { get; private set; }
    public Dictionary<Define.AreaName, Data.AreaData> AreaDataDict { get; private set; }
    public Dictionary<string, Data.WeaponData> WeaponDataDict { get; private set; }
    public Dictionary<string, Data.ArmorData> ArmorDataDict { get; private set; }

    public void Init()
    {
        HeroDataDict = LoadJson<Data.HeroDataLoader, int, Data.HeroData>("HeroData").MakeDict();
        MonsterDataDict = LoadJson<Data.MonsterDataLoader, int, Data.MonsterData>("MonsterData").MakeDict();
        ItemDataDict = LoadJson<Data.ItemDataLoader, int, Data.ItemData>("ItemData").MakeDict();
        AreaDataDict = LoadJson<Data.AreaDataSet, Define.AreaName, Data.AreaData>("AreaData").MakeDict();
        WeaponDataDict = LoadJson<Data.WeaponDataLoader, string, Data.WeaponData>("WeaponData").MakeDict();
        ArmorDataDict = LoadJson<Data.ArmorDataLoader, string, Data.ArmorData>("ArmorData").MakeDict();
    }

    // path 위치의 Json 파일을 TextAsset 타입으로 로드
    Data LoadJson<Data, Key, Value>(string path) where Data : IData<Key, Value>
    {
        TextAsset textAsset = Managers.ResourceMng.Load<TextAsset>($"Datas/{path}");
        return JsonUtility.FromJson<Data>(textAsset.text);
    }
}
