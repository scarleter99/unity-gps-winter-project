using System.Collections;
using System.Collections.Generic;
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
    
    protected override void Init()
    {
        base.Init();
        SceneType = Scene.AreaScene;
        GameObject go = Managers.ResourceMng.Instantiate("Area/@AreaSystem");
        _areaSystem = go.GetOrAddComponent<AreaSystem>();
        AreaName = AreaName.Forest;
        _areaSystem.Init();
        
    }
    
    public override void Clear()
    {
        Debug.Log("AreaScene Clear!");
    }

}