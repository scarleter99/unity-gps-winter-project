using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestTitleScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.TestTitleScene;

        Managers.InputMng.KeyAction += OnKeyboard;

        // UIManager test
        Managers.UIMng.ShowSceneUI<UI_TestInven>();
    }

    void OnKeyboard()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            Managers.SceneMng.LoadScene(Define.Scene.TestGameScene);
        }
    }

    public override void Clear()
    {
        Debug.Log("TitleScene Clear!");
    }
}
