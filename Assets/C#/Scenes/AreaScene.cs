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
            _areaSystem.AreaName = value;
        }
    }
    private AreaSystem _areaSystem;

    public AreaState AreaState
    {
        get => _areaSystem.AreaState;
        set => _areaSystem.AreaState = value;
    }

    public Action OnBattleSceneLoadStart;
    public Action OnBattleSceneUnloadFinish;
    
    protected override void Init()
    {
        base.Init();
        SceneType = Scene.AreaScene;
        GameObject areaSystem = Managers.ResourceMng.Instantiate("Area/@AreaSystem");
        _areaSystem = areaSystem.GetOrAddComponent<AreaSystem>();
        AreaName = AreaName.Forest; // TODO - 나중엔 외부에서 지정해줘야 함
        _areaSystem.Init();

        OnBattleSceneLoadStart -= _areaSystem.OnBattleSceneLoadStart;
        OnBattleSceneLoadStart += _areaSystem.OnBattleSceneLoadStart;
        OnBattleSceneUnloadFinish -= _areaSystem.OnBattleSceneUnloadFinish;
        OnBattleSceneUnloadFinish += _areaSystem.OnBattleSceneUnloadFinish;
    }

    public void LoadBattleScene()
    {   
        _areaSystem.FreezeCamera();
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
