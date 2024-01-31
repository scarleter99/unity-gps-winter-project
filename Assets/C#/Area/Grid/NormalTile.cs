using UnityEngine;
using static Define;

public sealed class NormalTile : HexGridCell
{
    public NormalTile(int x, int z, GameObject cellObject, float size = 1) : base(x, z, cellObject, size)
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
