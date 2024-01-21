using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public abstract class HexGridCell
{
    // 이 셀에 해당하는 오브젝트
    protected GameObject _cellObject;

    // 이 셀 위의 오브젝트
    public GameObject OnCellObject { get; set; }

    public AreaTileType TileType { get; set; }

    // 그리드에서의 좌표
    protected int _posx;
    protected int _posz;

    // 크기. 기본 1x1
    protected float _size;

    // 셀 모서리 스프라이트
    protected SpriteRenderer _indicator;
    // 셀 내부 스프라이트
    protected SpriteRenderer _fill;

    protected GameObject _icon;
    public GameObject Icon
    {
        get => _icon;
        set
        {
            _icon = value;
            if (value != null)
            {
                _icon.transform.position = _cellObject.transform.position;
            }
        }
    }

    protected Color _indicatorOriginalColor;
    protected Color _fillOriginalColor;

    // DoTween을 통해 스프라이트 색을 바꾸는데, 이 작업을 의미함.
    // 이를 통해 작업을 도중에 취소할 수 있음
    protected Tweener _colorTween;

    protected HexGridCell(int x, int z, GameObject cellObject, float size = 1)
    {
        _posx = x;
        _posz = z;
        _size = size;
        _cellObject = cellObject;

        _indicator = _cellObject.transform.Find("line").GetComponent<SpriteRenderer>();
        _fill = _cellObject.transform.Find("fill").GetComponent<SpriteRenderer>();
        _indicatorOriginalColor = _indicator.color;
        _fillOriginalColor = _fill.color;
        _cellObject.transform.localScale = new Vector3(_size, _size, 1);
    }

    public abstract void Init();
}
