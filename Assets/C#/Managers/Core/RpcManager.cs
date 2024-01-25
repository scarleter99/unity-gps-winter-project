using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RpcManager
{
    #region Managers

    [ServerRpc]
    public void StatChangeServerRpc(Define.WorldObject type, ulong id, MonsterStat statStruct)
    {
        StatChangeClientRpc(type, id, statStruct);
    }
    
    [ClientRpc]
    private void StatChangeClientRpc(Define.WorldObject type, ulong id, MonsterStat statStruct)
    {
        Managers.GameMng.StatChange(type, id, statStruct);
    }
    
    [ServerRpc]
    public void SpawnServerRpc(Define.WorldObject type, string path, string name = null)
    {
        SpawnClientRpc(type, path, name);
    }
    
    [ClientRpc]
    private void SpawnClientRpc(Define.WorldObject type, string path, string name = null)
    {
        Managers.GameMng.Spawn(type, path, name);
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

    #region Battle

    [ServerRpc]
    public void BattleStateChangeServerRpc(Define.BattleState battleState)
    {
        BattleChangeClientRpc(battleState);
    }
    
    [ClientRpc]
    private void BattleChangeClientRpc(Define.BattleState battleState)
    {
        BattleSystem battleSystem = GameObject.Find("@BattleSystem").GetOrAddComponent<BattleSystem>();
        battleSystem.BattleState = battleState;
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
