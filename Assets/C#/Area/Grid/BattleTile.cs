using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public sealed class BattleTile : HexGridCell
{
    private Color _indicatorColor = new Color(255f / 255f, 20f / 255f, 20f / 255f, 200f / 255f);
    private Color _fillColor = new Color(255f / 255f, 0f /255f, 0f /255f, 50f/ 255f);

    private const string _iconPath = "Map/icon_battle";

    // 타일에 다시 왔을 때 전투 진입 등 이벤트 재발생 방지
    private bool _eventTriggered = false;

    public BattleTile(int x, int z, float size, GameObject cellObject) : base(x, z, size, cellObject)
    {
        Init();
    }

    public override void Init()
    {   
       _indicator.color = _indicatorColor;
       _fill.color = _fillColor;

        Icon = Managers.ResourceMng.Instantiate(_iconPath, _cellObject.transform, "icon");
    }
}
