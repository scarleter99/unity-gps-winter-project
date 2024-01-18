using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

// 모든 Scene의 조상 클래스
public abstract class BaseScene : MonoBehaviour
{ 
    public Define.Scene SceneType { get; protected set; } = Define.Scene.UnknownScene;

    private void Awake()
    {
        Init();
    }
    
    protected virtual void Init()
    {
        Object obj1 = GameObject.FindObjectOfType(typeof(NetworkManager));
        Object obj3 = GameObject.FindObjectOfType(typeof(EventSystem));
        
        if (obj1 == null)
            Managers.ResourceMng.Instantiate("Network/NetworkManager").name = "@NetworkManager";
        if (obj3 == null)
            Managers.ResourceMng.Instantiate("UI/EventSystem").name = "@EventSystem";
    }
    
    public abstract void Clear();
}