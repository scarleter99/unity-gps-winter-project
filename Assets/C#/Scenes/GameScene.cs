using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.GameScene;
        
        GameObject go = new GameObject { name = "Bag" };
        Managers.GameMng.Bag = go.GetOrAddComponent<Bag>();
    }
    
    public override void Clear()
    {
        Debug.Log("GameScene Clear!");
    }
}
