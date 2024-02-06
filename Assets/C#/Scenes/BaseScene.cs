using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

// 모든 Scene의 조상 클래스
public abstract class BaseScene : MonoBehaviour
{ 
    public Define.SceneType SceneType { get; protected set; } = Define.SceneType.UnknownScene;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        Managers.InputMng.OnUpdate();
    }

    protected virtual void Init()
    {
        // TODO - TEST CODE: 나중엔 최초 Scene에서만 실행
        Managers.InputMng.Init();
        Managers.DataMng.Init();
        Managers.NetworkMng.Init();
        Managers.ServerMng.Init();
        Managers.SoundMng.Init();
        Managers.PoolMng.Init();
        Managers.ObjectMng.Init();
        Managers.BattleMng.Init();
        
        Object obj1 = FindObjectOfType(typeof(NetworkManager));
        Object obj2 = FindObjectOfType(typeof(EventSystem));
        
        if (obj1 == null)
            Managers.ResourceMng.Instantiate("Network/NetworkManager").name = "@NetworkManager";
        if (obj2 == null)
            Managers.ResourceMng.Instantiate("UI/EventSystem").name = "@EventSystem";
    }
    
    public abstract void Clear();
}