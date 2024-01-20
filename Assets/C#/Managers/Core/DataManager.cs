using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public interface IData<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

// 시작하면 바로 데이터를 Load하여 Dict로 관리
public class DataManager
{
    public Dictionary<string, Data.PlayerStat> PlayerStatDict { get; private set; }
    public Dictionary<string, Data.MonsterStat> MonsterStatDict { get; private set; }
    public Dictionary<string, Data.Item> ItemDict { get; private set; }

    public Dictionary<Define.AreaName, Data.AreaData> AreaDataDict { get; private set; }

    public void Init()
    {
        PlayerStatDict = LoadJson<Data.PlayerStatData, string, Data.PlayerStat>("PlayerStatData").MakeDict();
        MonsterStatDict = LoadJson<Data.MonsterStatData, string, Data.MonsterStat>("MonsterStatData").MakeDict();
        ItemDict = LoadJson<Data.ItemData, string, Data.Item>("ItemData").MakeDict();
        AreaDataDict = LoadJson<Data.AreaDataSet, Define.AreaName, Data.AreaData>("AreaData").MakeDict();
    }

    // path 위치의 Json 파일을 TextAsset 타입으로 로드
    Data LoadJson<Data, Key, Value>(string path) where Data : IData<Key, Value>
    {
        TextAsset textAsset = Managers.ResourceMng.Load<TextAsset>($"Datas/{path}");
        return JsonUtility.FromJson<Data>(textAsset.text);
    }
}
