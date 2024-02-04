using DG.Tweening;
using UnityEngine;
using static Define;

public sealed class BattleTile : HexGridCell
{
    private const string _iconPath = "Area/icon_battle";


    public BattleTile(int x, int z, GameObject cellObject, float size = 1) : base(x, z, cellObject)
    {
        _indicatorColor = new Color(255f / 255f, 20f / 255f, 20f / 255f, 200f / 255f);
        _fillColor = new Color(255f / 255f, 0f / 255f, 0f / 255f, 50f / 255f);
        _indicatorHighlightColor = new Color(255f / 255f, 20f / 255f, 20f / 255f, 255f / 255f);
        _fillHighlightColor = new Color(255f / 255f, 0f / 255f, 0f / 255f, 170f / 255f);
        Init();
    }

    public override void Init()
    {   
       _indicator.color = _indicatorColor;
       _fill.color = _fillColor;

        Icon = Managers.ResourceMng.Instantiate(_iconPath, _cellObject.transform, "icon");
    }

    public override void OnTileEnter()
    {
        if (_eventTriggered)
        {
            Managers.SceneMng.GetCurrentScene<AreaScene>().AreaState = AreaState.Idle;
            return;
        }
        Managers.SceneMng.GetCurrentScene<AreaScene>().AreaState = AreaState.Battle;
        Managers.SceneMng.GetCurrentScene<AreaScene>().LoadBattleScene();
    }
}