using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// XZ를 축으로 하는 그리드
// 육각형은 평평한 부분이 위 (flat-top)
public class HexGrid
{
    private int _width;
    private int _height;
    private float _cellwidth;
    private float _cellheight;
    private Vector3 _originPosition;

    private GameObject[,] _gridArray;
    private bool[,] _isValid;

    public HexGrid(int width, int height, Vector3 originposition, float cellwidth = 1.875f * 2, float cellheight = 1.625f * 2)
    {
        if (width % 2 == 0)
        {
            Debug.LogError("Grid width must be odd number!");
            return;
        }
        if (height < width / 2 + 1)
        {
            Debug.LogError($"Grid height is too small! Must be equal or larger than {width / 2 + 1}!");
            return;
        }
        _width = width;
        _height = height;
        _cellwidth = cellwidth;
        _cellheight = cellheight;
        _originPosition = originposition;
        _gridArray = new GameObject[height, width];
        InitializeIsValid();
    }

    // 유효한 그리드인지 확인해주는 2차원 bool _isValid 초기화
    // 타일이 육각형이며 맵 모양도 있기 때문에 width와 height를 꽉 채워서 생성하는 게 아님
    // _isValid에 의해 맵 모양이 결정됨
    private void InitializeIsValid()
    {
        _isValid = new bool[_height, _width];
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                _isValid[j, i] = false;
            }
        }

        // flat-top hexagon, diamond-shape grid
        int startoffset = (_width + 1) / 4;
        int endoffset = (_width - 1) / 4;

        for (int x = 0; x < _width; x++)
        {
            if (x < (_width - 1) / 2)
            {
                for (int z = startoffset; z < _height - endoffset; z++)
                {
                    _isValid[z, x] = true;
                }
                if (x % 2 == 0) startoffset -= 1;
                else endoffset -= 1;
            }
            else
            {
                for (int z = startoffset; z < _height - endoffset; z++)
                {   
                    _isValid[z, x] = true;
                }
                if (x % 2 == 1) startoffset += 1;
                else endoffset += 1;
            }
        }
        #region legacy: for point-top hex, diamond-shaped grid
        //int startoffset = (_width - 3) / 2;
        //int endoffset = (_width - 3) / 2;

        //for (int z = 0; z < _height; z++)
        //{
        //    if (z < _width - 3)
        //    {
        //        for (int x = startoffset; x < _width - endoffset; x++)
        //        {
        //            _isValid[z, x] = true;
        //        }
        //        if (z % 2 == 0) startoffset -= 1;
        //        else endoffset -= 1;
        //    }
        //    else if (_height - z <= _width - 3)
        //    {
        //        if (z % 2 == 0) startoffset += 1;
        //        else endoffset += 1;
        //        for (int x = startoffset; x < _width - endoffset; x++)
        //        {
        //            _isValid[z, x] = true;
        //        }
        //    }
        //    else
        //    {
        //        if (z % 2 == 1) endoffset = 1;
        //        else endoffset = 0;
        //        for (int x = 0; x < _width - endoffset; x++)
        //        {   
        //            _isValid[z, x] = true;
        //        }
        //    }
        //}
        #endregion

    }

    // 그리드 좌표를 월드 좌표로 변환
    public Vector3 GetWorldPosition(int x, int z)
    {   
        if (x % 2 == 1) return new Vector3(x * _cellwidth * 0.75f, 0, (z+0.5f) * _cellheight) + _originPosition;
        else return new Vector3(x * _cellwidth * 0.75f, 0, z * _cellheight) + _originPosition;
    }

    // 월드 좌표를 그리드 좌표로 변환
    public void GetGridPosition(Vector3 worldPosition, out int x, out int z)
    {
        x = Mathf.FloorToInt((worldPosition - _originPosition).x / _cellwidth);
        z = Mathf.FloorToInt((worldPosition - _originPosition).z / _cellheight);
    }

    public void SetGridObject(int x, int z, GameObject gridObject, bool setPosition = true)
    {
        _gridArray[z,x] = gridObject;
        if (setPosition)
        {
            gridObject.transform.position = GetWorldPosition(x, z);
        }
    }
    
    public GameObject GetGridObject(int x, int z)
    {
        return _gridArray[z,x];
    }

    public bool isGridPositionValid(int x, int z)
    {
        return _isValid[z, x];
    }
}
