using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class UIManager
{
    private int _order = 10; // 현재까지 최근에 사용한 오더
    
    private UI_Scene _sceneUI; // 현재의 고정 캔버스 UI
    private Stack<UI_Popup> _popupStack = new Stack<UI_Popup>(); // 팝업 캔버스 UI Stack

    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
                root = new GameObject { name = "@UI_Root" };

            return root;
        }
    }

    //sort가 true면, go의 Canvas 컴포넌트를 가져와 _order값을 1더해서 설정 (PopupUI)
    //sort가 false면, go의 Canvas 컴포넌트를 가져와 _order값을 0으로 설정(SceneUI)
    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true; // 부모 캔버스와는 독립적인 오더값을 가짐

        if (sort)
        {
            canvas.sortingOrder = _order++;
        }
        else
        {
            canvas.sortingOrder = 0;
        }
    }

    // 이름이 name인 SceneUI를 생성한 후 T컴포넌트로 반환
    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.ResourceMng.Instantiate($"UI/SceneUI/{name}");
        T sceneUI = Util.GetOrAddComponent<T>(go);
        _sceneUI = sceneUI;

        go.transform.SetParent(Root.transform);
        
        return sceneUI;
    }

    // 이름이 name인 PopupUI를 생성한 후 T컴포넌트로 반환
    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.ResourceMng.Instantiate($"UI/PopupUI/{name}");
        T popupUI = Util.GetOrAddComponent<T>(go);
        _popupStack.Push(popupUI);
        
        go.transform.SetParent(Root.transform);
        
        return popupUI;
    }

    // 이름이 name인 SubItemUI를 생성한 후 T컴포넌트로 반환
    public T MakeSubItemUI<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.ResourceMng.Instantiate($"UI/SubItemUI/{name}");
        if (parent != null)
            go.transform.SetParent(parent);
        
        return go.GetOrAddComponent<T>();
    }

    // 이름이 name인 WorldSpaceUI를 생성한 후 T컴포넌트로 반환
    public T MakeWorldSpaceUI<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.ResourceMng.Instantiate($"UI/WorldSpaceUI/{name}");
        if (parent != null)
            go.transform.SetParent(parent);

        Canvas canvas = go.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;

        return go.GetOrAddComponent<T>();
    }

    // 가장 Order가 높은 PopupUI 제거
    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0)
            return;

        UI_Popup popupUI= _popupStack.Pop();
        Managers.ResourceMng.Destroy(popupUI.gameObject);
        popupUI = null;
        _order--;
    }

    // 가장 Order가 높은 PopupUI 확인 후 제거
    public void ClosePopupUI(UI_Popup popup)
    {
        if (_popupStack.Count == 0)
            return;

        if (_popupStack.Peek() != popup)
        {
            Debug.Log("Close Popup Failed");
            return;
        }
        
        ClosePopupUI();
    }

    // 모든 PopupUI 제거
    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0)
        {
            ClosePopupUI();
        }
    }
    
    public void Clear()
    {
        CloseAllPopupUI();
        _sceneUI = null;
    }
}
