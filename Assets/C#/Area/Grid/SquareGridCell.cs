using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SquareGridCell
{   
    // 이 셀에 해당하는 게임에서의 오브젝트 (플레이어같은 셀 위의 오브젝트 아님)
    private GameObject _cellObject;

    // 플레이어, 몬스터 등 이 셀 위의 오브젝트
    private GameObject _onCellObject;

    // 그리드에서의 좌표
    private int _posx;
    private int _posz;

    // 크기. 기본 1x1
    private float _size;

    // 그리드 셀 나타내는 사각형 스프라이트
    // 그리드의 y좌표를 지면보다 약간 높게 설정해야 보임
    private SpriteRenderer _indicator;
    private Color _originalColor;

    public SquareGridCell(int x, int z, float size, SpriteRenderer indicator)
    {
        _posx = x;
        _posz = z;
        _size = size;
        _indicator = indicator;
        _originalColor = indicator.color;
        _cellObject = indicator.gameObject;
        Init();
    }

    private void Init()
    {
        _cellObject.transform.localScale = new Vector3(_size, _size, 1);
    }

    public void OnMouseEnter(Define.GridOwner owner)
    {
        if (owner == Define.GridOwner.Player)
        {
            _indicator.DOColor(Color.green, 0.3f);
        }
        else if (owner == Define.GridOwner.Enemy)
        {
            _indicator.DOColor(Color.red, 0.3f);
        }
    }

    public void OnMouseExit()
    {
        _indicator.DOColor(_originalColor, 0.3f);
    }

}
