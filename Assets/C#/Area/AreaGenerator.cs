using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;


public abstract class AreaGenerator
{
    protected AreaName _areaName;

    protected int _width;
    protected int _height;
    private Vector3 _originPosition;

    protected HexGrid _grid;
    protected Transform _tileParent;


    // For test
    //protected const string _testprefabPath = "Map/Area_1/GrassBase";
    protected const string _testprefabPath = "Map/grid_hex";
    private const string _testStartprefabPath = "Map/Area_1/RockBase";

    protected AreaGenerator(int width, int height, Vector3 originPosition)
    {
        _width = width;
        _height = height;
        _originPosition = originPosition;
        _grid = new HexGrid(_width, _height, _originPosition);
        _tileParent = GameObject.Find("Tiles").transform;
    }

    // 맵 생성 로직 흐름 관리
    public abstract void GenerateMap();

    // 일반 타일
    protected abstract void GenerateCommonTiles();
    
    // 시작 타일: 맨 밑 중간
    protected void GenerateStartTile()
    {
        _grid.SetGridObject(_width / 2, 0, Managers.ResourceMng.Instantiate(_testStartprefabPath, _tileParent));
    }
    
    // 보스 타일: 맨 위 중간
    protected void GenerateBossTile()
    {
        _grid.SetGridObject(_width / 2, _height-1, Managers.ResourceMng.Instantiate(_testStartprefabPath, _tileParent));
    }
}
