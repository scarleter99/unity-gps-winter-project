using System;
using UnityEngine;
using static Define;

public class AreaScene : BaseScene
{
    private AreaName _areaName;

    public AreaName AreaName
    {
        get => _areaName;
        set
        {
            _areaName = value;
            AreaManager.AreaName = value;
        }
    }

    private AreaManager AreaManager;

    public AreaState AreaState
    {
        get => AreaManager.AreaState;
        set => AreaManager.AreaState = value;
    }

    public Action OnBattleSceneLoadStart;
    public Action OnBattleSceneUnloadFinish;
    
    protected override void Init()
    {
        base.Init();
        AreaManager = Managers.AreaMng;
        SceneType = SceneType.AreaScene;
        AreaName = AreaName.Forest; // TODO - 나중엔 외부에서 지정해줘야 함
        AreaManager.Init();

        OnBattleSceneLoadStart -= AreaManager.OnBattleSceneLoadStart;
        OnBattleSceneLoadStart += AreaManager.OnBattleSceneLoadStart;
        OnBattleSceneUnloadFinish -= AreaManager.OnBattleSceneUnloadFinish;
        OnBattleSceneUnloadFinish += AreaManager.OnBattleSceneUnloadFinish;
    }

    public void LoadBattleScene()
    {   
        AreaManager.FreezeCamera();
        StartCoroutine(Managers.SceneMng.LoadBattleScene());
    }

    public void UnloadBattleScene()
    {
        StartCoroutine(Managers.SceneMng.UnloadBattleScene());
        AreaState = AreaState.Idle;
    }
    
    public override void Clear()
    {
        Debug.Log("AreaScene Clear!");
    }

}
