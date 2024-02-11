using DG.Tweening;
using UnityEngine;
using static Define;

public enum TileColorChangeType
{
    Highlight,
    Reset,
    ToNormal
}

public abstract class AreaGridTile
{
    // 자식으로 스프라이트, 콜라이더 등을 가지는 부모 오브젝트 (name: grid_hex)
    protected GameObject _tileObject;

    // 이 셀 위의 오브젝트
    public GameObject OnCellObject { get; set; }

    public AreaTileType TileType { get; set; }

    protected Vector3 _worldPosition;
    
    protected bool _eventTriggered = false;

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
                _icon.transform.position = _tileObject.transform.position;
            }
        }
    }

    protected Color _indicatorColor; // 각 tile별 지정된 색
    protected Color _fillColor;
    protected Color _indicatorOriginalColor; // normal tile의 색
    protected Color _fillOriginalColor;
    protected Color _indicatorHighlightColor; // 플레이어의 이동 가능 지점을 보여줄 때 하이라이트되는 색
    protected Color _fillHighlightColor;


    // DoTween을 통해 스프라이트 색을 바꾸는데, 이 작업을 의미함.
    // 이를 통해 작업을 도중에 취소할 수 있음
    private Tweener _indicatorColorTween;
    private Tweener _fillColorTween;

    private const string GRID_TILE_PATH = "Area/grid_hex";

    protected AreaGridTile(Vector3 position)
    {   
        _worldPosition = position;
        InitTileObject();
        InitMesh();
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

    private void InitTileObject()
    {
        Transform tileParent = GameObject.Find("Tiles").transform;
        _tileObject = Managers.ResourceMng.Instantiate(GRID_TILE_PATH, tileParent);
        _tileObject.transform.position = _worldPosition;
        _indicator = _tileObject.transform.Find("line").GetComponent<SpriteRenderer>();
        _fill = _tileObject.transform.Find("fill").GetComponent<SpriteRenderer>();
        _indicatorOriginalColor = _indicator.color;
        _fillOriginalColor = _fill.color;
    }

    private void InitMesh()
    {
        Mesh mesh = Util.SpriteToMesh(_fill.sprite);
        _tileObject.transform.Find("collider").GetComponent<MeshCollider>().sharedMesh = mesh;
    }


    public abstract void OnTileEnter();

    public virtual void OnTileEventFinish()
    {
        _eventTriggered = true;
        Icon.GetComponent<SpriteRenderer>().DOFade(0, 3f);
        ChangeColor(TileColorChangeType.ToNormal, 3f);
        _indicatorColor = _indicatorOriginalColor;
        _fillColor = _fillOriginalColor;
        _indicatorHighlightColor = new Color(50f / 255f, 1f, 0);
        _fillHighlightColor = new Color(50f / 255f, 1f, 0, 65f / 255f);
    }
}
