using DG.Tweening;
using UnityEngine;
using static Define;

public enum TileColorChangeType
{
    Highlight,
    Reset,
    ToNormal
}

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

    protected Color _indicatorColor;
    protected Color _fillColor;
    protected Color _indicatorOriginalColor;
    protected Color _fillOriginalColor;
    protected Color _indicatorHighlightColor;
    protected Color _fillHighlightColor;


    // DoTween을 통해 스프라이트 색을 바꾸는데, 이 작업을 의미함.
    // 이를 통해 작업을 도중에 취소할 수 있음
    protected Tweener _indicatorColorTween;
    protected Tweener _fillColorTween;

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
        SetupMesh();
    }

    public abstract void Init();

    public void ChangeColor(TileColorChangeType changeType, float duration = 0.3f)
    {
        KillColorTween();
        switch (changeType)
        {
            case TileColorChangeType.Highlight:
                _indicatorColorTween = _indicator.DOColor(_indicatorHighlightColor, duration).OnComplete(() => { _indicatorColorTween = null; });
                _fillColorTween = _fill.DOColor(_fillHighlightColor, duration).OnComplete(() => { _fillColorTween = null; });
                break;
            case TileColorChangeType.Reset:
                _indicatorColorTween = _indicator.DOColor(_indicatorColor, duration).OnComplete(() => { _indicatorColorTween = null; });
                _fillColorTween = _fill.DOColor(_fillColor, duration).OnComplete(() => { _fillColorTween = null; });
                break;
            case TileColorChangeType.ToNormal:
                _indicatorColorTween = _indicator.DOColor(_indicatorOriginalColor, duration).OnComplete(() => { _indicatorColorTween = null; });
                _fillColorTween = _fill.DOColor(_fillOriginalColor, duration).OnComplete(() => { _fillColorTween = null; });
                break;
        }

    }

    // 기존 진행중인 colorTween을 중지, 삭제
    private void KillColorTween()
    {
        _indicatorColorTween?.Kill();
        _indicatorColorTween = null;

        _fillColorTween?.Kill();
        _fillColorTween = null;
    }

    private void SetupMesh()
    {
        Mesh mesh = Util.SpriteToMesh(_fill.sprite);
        _cellObject.transform.Find("collider").GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    public abstract void OnTileEnter();
}
