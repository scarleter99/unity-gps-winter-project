using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.BattleScene;
        
        gameObject.GetOrAddComponent<BattlefieldSystem>();
    }
    
    public override void Clear()
    {
        Debug.Log("BattleScene Clear!");
    }
}
