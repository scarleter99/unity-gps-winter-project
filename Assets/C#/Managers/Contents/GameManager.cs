using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Data;
using UnityEngine;

public class GameManager
{
    private List<Dictionary<ulong, GameObject>> _gameObjectDics = new List<Dictionary<ulong, GameObject>>(); // Unknown타입의 Dic은 생성하지 않음
    private List<ulong> _lastIds = new List<ulong>(); // id는 1부터 시작

    public void Init()
    {
        for (int i = 0; i < Enum.GetNames(typeof(Define.WorldObject)).Length - 1; i++)
        {
            _gameObjectDics.Add(new Dictionary<ulong, GameObject>());
            _lastIds.Add(0);
        }
    }
    
    public void StatChange(Define.WorldObject type, ulong id, MonsterStat statStruct)
    {
        _gameObjectDics[(int)type][id].GetOrAddComponent<BaseController>().StatChange(statStruct);
    }
    
    public void PlayerStatChange(ulong id, PlayerStat playerStatStruct)
    {
        _gameObjectDics[(int)Define.WorldObject.Player][id].GetOrAddComponent<PlayerController>().PlayerStatChange(playerStatStruct);
    }
    
    public GameObject Spawn(Define.WorldObject type, string path)
    {
        GameObject parent = null;
        switch (type)
        {
            case Define.WorldObject.Player:
                parent = GameObject.Find("@Players");
                if (parent == null)
                {
                    parent = Managers.ResourceMng.Instantiate("@Players");
                    parent.name = "@Players";
                }
                break;
            case Define.WorldObject.Monster:
                parent = GameObject.Find("@Monsters");
                if (parent == null)
                {
                    parent = Managers.ResourceMng.Instantiate("@Monsters");
                    parent.name = "@Monsters";
                }
                break;
        }
        
        GameObject go = Managers.ResourceMng.Instantiate(path, parent.transform);
        
        int typeNum = (int)type;
        ulong id = ++_lastIds[typeNum];
        
        // TODO
        // go에 id 할당
        _gameObjectDics[typeNum][id] = go;

        return go;
    }

    public void Despawn(Define.WorldObject type, ulong id)
    {
        if (type == Define.WorldObject.Unknown)
        {
            Debug.Log("Invalid Despawn");
            return;
        }

        GameObject go = _gameObjectDics[(int)type][id];
        _gameObjectDics[(int)type].Remove(id);
        Managers.ResourceMng.Destroy(go);
    }

    public Define.WorldObject GetWorldObjectType(GameObject go)
    {
        BaseController bc = go.GetComponent<BaseController>();
        if (bc == null)
            return Define.WorldObject.Unknown;
        
        return bc.WorldObjectType;
    }
}
