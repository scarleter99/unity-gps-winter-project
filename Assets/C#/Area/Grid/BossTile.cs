using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public sealed class BossTile : HexGridCell
{
    private Color _indicatorColor = new Color(138 / 255f, 0 / 255f, 255 / 255f, 255 / 255f);
    private Color _fillColor = new Color(85 / 255f, 0 /255f, 163 /255f, 222/ 255f);

    private const string _iconPath = "Map/icon_boss";

    // 타일에 다시 왔을 때 전투 진입 등 이벤트 재발생 방지
    private bool _eventTriggered = false;

    public BossTile(int x, int z, GameObject cellObject, float size = 1) : base(x, z, cellObject)
    {
        Init();
    }

    public override void Init()
    {   
       _indicator.color = _indicatorColor;
       _fill.color = _fillColor;

        Icon = Managers.ResourceMng.Instantiate(_iconPath, _cellObject.transform, "icon");
        Icon.transform.position += new Vector3(0, 0, -0.1f);
    }
}
