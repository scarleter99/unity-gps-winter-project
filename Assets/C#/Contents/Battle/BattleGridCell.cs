using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BattleGridCell : MonoBehaviour
{
    [SerializeField] private Define.GridSide _gridSide;
    public Define.GridSide GridSide { get => _gridSide; protected set => _gridSide = value; }
    
    public Creature CellCreature { get; set; }
    
    public int Row { get; protected set; }
    public int Col { get; protected set; }

    private SpriteRenderer _indicator;
    private Color _originalColor;

    private Tweener _colorTween;


    private void Start()
    {   
        _indicator = GetComponent<SpriteRenderer>();
        _originalColor = _indicator.color;
    }

    public void SetRowCol(int row, int col)
    {
        Row = row;
        Col = col;
    }

    public void MouseEnter()
    {
        if (GridSide == Define.GridSide.HeroSide)
        {
            ChangeColor(Color.green);
        }
        else if (GridSide == Define.GridSide.MonsterSide)
        {
            ChangeColor(Color.red);
        }
    }

    private void ChangeColor(Color color, float duration = 0.3f)
    {
        KillColorTween();
        _colorTween = _indicator.DOColor(color, duration).OnComplete(() => { _colorTween = null; });
    }

    public void MouseExit()
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
