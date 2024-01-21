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

    private GameObject _player;

    public Action<int> OnSpawnEvent;

    public void Init()
    {
        for (int i = 0; i < Enum.GetNames(typeof(Define.WorldObject)).Length - 1; i++)
        {
            _gameObjectDics.Add(new Dictionary<ulong, GameObject>());
            _lastIds.Add(0);
        }
    }
    
    public GameObject Player { get => _player; set => _player = value; }

    public void StatChange(Define.WorldObject type, ulong id, TestStruct testStruct)
    {
        _gameObjectDics[(int)type][id].GetOrAddComponent<BaseController>().StatChange(testStruct);
    }
    
    public GameObject Spawn(string path, Define.WorldObject parentType = Define.WorldObject.Unknown, ulong parentId = 0)
    {
        Transform parent = null;
        if (parentType == Define.WorldObject.Unknown)
            parent = _gameObjectDics[(int)parentType][parentId].transform;

        GameObject go = Managers.ResourceMng.Instantiate(path, parent);
        
        int typeNum = (int)GetWorldObjectType(go);
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
