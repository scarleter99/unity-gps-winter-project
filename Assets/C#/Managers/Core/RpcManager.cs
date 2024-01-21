using System.Collections;
using System.Collections.Generic;
using Data;
using Unity.Netcode;
using UnityEngine;

public class RpcManager
{
    #region Managers

    [ServerRpc]
    public void StatChangeServerRpc(Define.WorldObject type, ulong id, TestStruct testStruct)
    {
        StatChangeClientRpc(type, id, testStruct);
    }
    
    [ClientRpc]
    private void StatChangeClientRpc(Define.WorldObject type, ulong id, TestStruct testStruct)
    {
        // TODO
        Managers.GameMng.StatChange(type, id, testStruct);
    }
    
    [ServerRpc]
    public void SpawnServerRpc(string path, Define.WorldObject parentType = Define.WorldObject.Unknown, ulong parentId = 0)
    {
        SpawnClientRpc(path, parentType, parentId);
    }
    
    [ClientRpc]
    private void SpawnClientRpc(string path, Define.WorldObject parentType = Define.WorldObject.Unknown, ulong parentId = 0)
    {
        Managers.GameMng.Spawn(path, parentType, parentId);
    }
    
    [ServerRpc]
    public void DespawnServerRpc(Define.WorldObject type, ulong id)
    {
        DespawnClientRpc(type, id);
    }
    
    [ClientRpc]
    private void DespawnClientRpc(Define.WorldObject type, ulong id)
    {
        Managers.GameMng.Despawn(type, id);
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
    
    #endregion
    
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
