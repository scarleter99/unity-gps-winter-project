using UnityEngine;
using static Define;

public sealed class NormalTile : AreaGridTile
{
    public NormalTile(Vector3 position) : base(position)
    {
        _indicatorColor = _indicatorOriginalColor;
        _fillColor = _fillOriginalColor;
        _indicatorHighlightColor = new Color(50f / 255f, 1f, 0);
        _fillHighlightColor = new Color(50f / 255f, 1f, 0, 65f / 255f);

        Init();
    }

    public override void Init()
    {
        return;
    }
    public override void OnTileEnter()
    {
        Managers.SceneMng.GetCurrentScene<AreaScene>().AreaState = AreaState.Idle;
    }
}
