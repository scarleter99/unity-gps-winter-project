using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager
{
    private List<GameObject> _players = new List<GameObject>();
    private List<GameObject> _monsters = new List<GameObject>();

    public Action<int> OnSpawnEvent;

    public List<GameObject> Players { get => _players; }
    public List<GameObject> Monsters { get => _monsters; }

    public GameObject Spawn(Define.WorldObject type, string path, Transform parent = null)
    {
        GameObject go = Managers.ResourceMng.Instantiate(path, parent);

        switch (type)
        {
            case Define.WorldObject.Monster:
                Monsters.Add(go);
                if (OnSpawnEvent != null)
                    OnSpawnEvent.Invoke(1);
                break;
            case Define.WorldObject.Player:
                Players.Add(go);
                break;
        }

        return go;
    }

    public Define.WorldObject GetWorldObjectType(GameObject go)
    {
        BaseController bc = go.GetComponent<BaseController>();
        if (bc == null)
            return Define.WorldObject.Unknown;
        
        return bc.WorldObjectType;
    }

    public void Despawn(GameObject go)
    {
        Define.WorldObject type = GetWorldObjectType(go);

        switch (type)
        {
            case Define.WorldObject.Monster:
                if (Monsters.Contains(go))
                {
                    Monsters.Remove(go);
                    if (OnSpawnEvent != null)
                        OnSpawnEvent.Invoke(-1);
                }
                break;
            case Define.WorldObject.Player:
                if (Players.Contains(go))
                    Players.Remove(go);
                break;
        }
        
        Managers.ResourceMng.Destroy(go);
    }
}
