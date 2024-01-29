using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SquareGridCell
{   
    // player or enemy
    private Define.GridSide _side;

    // 이 셀에 해당하는 게임에서의 오브젝트 (플레이어같은 셀 위의 오브젝트 아님)
    private GameObject _cellObject;

    // 플레이어, 몬스터 등 이 셀 위의 오브젝트
    public GameObject OnCellObject { get; set; }

    // 그리드에서의 좌표
    private int _posx;
    private int _posz;

    // 크기. 기본 1x1
    private float _size;

    // 셀 나타내는 사각형 스프라이트
    // 그리드의 y좌표를 지면보다 약간 높게 설정해야 보임
    private SpriteRenderer _indicator;
    private Color _originalColor;

    // DoTween을 통해 스프라이트 색을 바꾸는데, 이 작업을 의미함.
    // 이를 통해 작업을 도중에 취소할 수 있음
    private Tweener _colorTween;

    public SquareGridCell(int x, int z, float size, GameObject cellObject, Define.GridSide side)
    {
        _posx = x;
        _posz = z;
        _size = size;
        _cellObject = cellObject;
        _side = side;
        Init();
    }

    private void Init()
    {
        _indicator = _cellObject.GetComponent<SpriteRenderer>();
        _originalColor = _indicator.color;
        _cellObject.transform.localScale = new Vector3(_size, _size, 1);
        switch (_side)
        {
            case Define.GridSide.Player:
                _cellObject.tag = "PlayerGrid";
                break;
            case Define.GridSide.Enemy:
                _cellObject.tag = "EnemyGrid";
                break;
        }
    }

    public void OnMouseEnter()
    {
        if (_side == Define.GridSide.Player)
        {
            ChangeColor(Color.green);
        }
        else if (_side == Define.GridSide.Enemy)
        {
            ChangeColor(Color.red);
        }
    }

    private void ChangeColor(Color color, float duration = 0.3f)
    {
        KillColorTween();
        _colorTween = _indicator.DOColor(color, duration).OnComplete(() => { _colorTween = null; });
    }

    public void OnMouseExit()
    {   
        ChangeColor(_originalColor);
    }

    // 기존 진행중인 colorTween을 중지, 삭제
    private void KillColorTween()
    {
        _colorTween?.Kill();
        _colorTween = null;
    }

}
