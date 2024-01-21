using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScene : BaseScene
{
    private BattleSystem _battleSystem;
    public BattleSystem BattleSystem { get => _battleSystem; }
    
    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.BattleScene;

        GameObject go = Managers.ResourceMng.Instantiate("Etc/@BattleSystem");
        _battleSystem = go.GetOrAddComponent<BattleSystem>();
    }
    
    public override void Clear()
    {
        Debug.Log("BattleScene Clear!");
    }
}
