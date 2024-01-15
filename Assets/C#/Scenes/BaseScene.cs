using System.Collections;
using System.Collections.Generic;
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
        Object obj1 = GameObject.FindObjectOfType(typeof(EventSystem));
        Object obj2 = GameObject.FindObjectOfType(typeof(ConnectionApprovalHandler));
        
        if (obj1 == null)
            Managers.ResourceMng.Instantiate("UI/EventSystem").name = "@EventSystem";
        if (obj2 == null)
            Managers.ResourceMng.Instantiate("Network/ConnectionHandler").name = "@ConnectionHandler";
    }
    
    public abstract void Clear();
}