using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BattleScene : BaseScene
{
    private BattleSystem _battleSystem;
    public BattleSystem BattleSystem { get => _battleSystem; }
    
    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.BattleScene;

        GameObject go = Managers.ResourceMng.Instantiate("Battle/@BattleSystem");
        _battleSystem = go.GetOrAddComponent<BattleSystem>();

        Managers.UIMng.ShowSceneUI<UI_BattleScene>();
    }

    public override void Clear()
    {
        Debug.Log("BattleScene Clear!");
    }
}
