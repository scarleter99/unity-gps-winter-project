using System.Collections;
using System.Collections.Generic;
using Data;
using Unity.Netcode;
using UnityEngine;

public class RpcManager
{
    [ServerRpc]
    public void UpdatePositionServerRpc(Transform transform, Vector3 dest, float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, dest,
            NetworkManager.Singleton.ServerTime.FixedDeltaTime * speed);

        if (dest != transform.position)
        {
            Vector3 targetDirection = dest - transform.position;
            targetDirection.y = 0f; // 오브젝트 평행 유지
            transform.forward = targetDirection;
        }
    }

    [ServerRpc]
    public void UpdateTestStructServerRpc(ulong gameObjectId, TestStruct testStruct, Define.WorldObject gameObjectType)
    {
        UpdateTestStructClientRpc(gameObjectId, testStruct, gameObjectType);
    }
    
    [ClientRpc]
    private void UpdateTestStructClientRpc(ulong gameObjectId, TestStruct testStruct, Define.WorldObject gameObjectType)
    {
        // TODO
        //Managers.GameMng.UpdateStat(gameObjectId, testStruct, gameObjectType);
    }
    
    [ServerRpc]
    public void InstantiateObjectServerRpc(string path, ulong parentId = 0, string name = null)
    {
        InstantiateObjectClientRpc(path, parentId, name);
    }
    
    [ClientRpc]
    private void InstantiateObjectClientRpc(string path, ulong parentId = 0, string name = null)
    {
        // TODO
        //Managers.ResourceMng.Instantiate(path, parentId, name);
    }
    
    [ServerRpc]
    public void DestroyObjectServerRpc(ulong gameObjectId, Define.WorldObject gameObjectType)
    {
        DestroyObjectClientRpc(gameObjectId, gameObjectType);
    }
    
    [ClientRpc]
    private void DestroyObjectClientRpc(ulong gameObjectId, Define.WorldObject gameObjectType)
    {
        // TODO
        //Managers.ResourceMng.Destroy(gameObjectId);
    }

    [ServerRpc]
    public void LoadSceneServerRpc(Define.Scene type)
    {
        LoadSceneClientRpc(type);
    }
    
    [ClientRpc]
    private void LoadSceneClientRpc(Define.Scene type)
    {
        Managers.SceneMng.LoadScene(type);
    }
    
    #region UI RPC
    
    [ServerRpc]
    public void ShowSceneUIServerRpc<T>(string name) where T : UI_Scene
    {
        ShowSceneUIClientRpc<T>(name);
    }
    
    [ClientRpc]
    private void ShowSceneUIClientRpc<T>(string name) where T : UI_Scene
    {
        Managers.UIMng.ShowSceneUI<T>(name);
    }
    
    [ServerRpc]
    public void ShowPopupUIServerRpc<T>(string name) where T : UI_Popup
    {
        ShowPopupUIClientRpc<T>(name);
    }
    
    [ClientRpc]
    private void ShowPopupUIClientRpc<T>(string name) where T : UI_Popup
    {
        Managers.UIMng.ShowPopupUI<T>(name);
    }
    
    [ServerRpc]
    public void MakeSubItemUIServerRpc<T>(ulong parentId, string name) where T : UI_Base
    {
        MakeSubItemUIClientRpc<T>(parentId, name);
    }
    
    [ClientRpc]
    private void MakeSubItemUIClientRpc<T>(ulong parentId, string name) where T : UI_Base
    {
        // TODO
        //Managers.UIMng.MakeSubItemUI<T>(name); 
    }
    
    [ServerRpc]
    public void MakeWorldSpaceUIServerRpc<T>(ulong parentId, string name) where T : UI_Scene
    {
        MakeWorldSpaceUIClientRpc<T>(parentId, name);
    }
    
    [ClientRpc]
    private void MakeWorldSpaceUIClientRpc<T>(ulong parentId, string name) where T : UI_Scene
    {
        // TODO
        //Managers.UIMng.MakeWorldSpaceUI<T>(name);
    }
    
    #endregion
}
