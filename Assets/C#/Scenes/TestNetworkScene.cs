using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNetworkScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.TownScene;

        // UIManager test
        Managers.UIMng.ShowPopupUI<UI_TestClientButton>("UI_TestClientButton");
    }

    public override void Clear()
    {
        #if UNITY_EDITOR
        Debug.Log("TestNetworkScene Clear!");
        #endif
    }
}
