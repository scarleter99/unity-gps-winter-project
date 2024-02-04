using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BattleScene : BaseScene
{
    private BattleManager _battleManager;
    public BattleManager BattleManager { get => _battleManager; }
    
    protected override void Init()
    {
        base.Init();
        SceneType = Define.SceneType.BattleScene;

        Managers.BattleManager.InitBattle();

        Managers.UIMng.ShowSceneUI<UI_BattleScene>();
    }

    public override void Clear()
    {
        Debug.Log("BattleScene Clear!");
    }
}
