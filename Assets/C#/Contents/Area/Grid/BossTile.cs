using UnityEngine;
using static Define;

public sealed class BossTile : HexGridCell
{
    private const string _iconPath = "Area/icon_boss";

    public BossTile(int x, int z, GameObject cellObject, float size = 1) : base(x, z, cellObject)
    {
        _indicatorColor = new Color(138 / 255f, 0 / 255f, 255 / 255f, 255 / 255f);
        _fillColor = new Color(85 / 255f, 0 / 255f, 163 / 255f, 222 / 255f);
        _indicatorHighlightColor = new Color(200f / 255f, 0f / 255f, 0 / 255f, 255 / 255f);
        _fillHighlightColor = new Color(163 / 255f, 0 / 255f, 0 / 255f, 222 / 255f);
        Init();
    }

    public override void Init()
    {   
       _indicator.color = _indicatorColor;
       _fill.color = _fillColor;

        Icon = Managers.ResourceMng.Instantiate(_iconPath, _cellObject.transform, "icon");
        Icon.transform.position += new Vector3(0, 0, -0.1f);
    }
    public override void OnTileEnter()
    {
        if (_eventTriggered)
        {
            Managers.SceneMng.GetCurrentScene<AreaScene>().AreaState = AreaState.Idle;
            return;
        }
        Managers.SceneMng.GetCurrentScene<AreaScene>().AreaState = AreaState.Idle; // TODO - Boss 구현 시 상태 수정
    }
}