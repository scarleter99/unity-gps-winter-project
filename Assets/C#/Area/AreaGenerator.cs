using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static Define;


public abstract class AreaGenerator
{
    protected GameObject _map;
    protected AreaName _areaName;

    // 맵 나타내는 배열
    // -1: 맵 바깥쪽, 0: 장애물, 1: 이동 가능 타일
    protected int[,] _basemap;

    protected int _width;
    protected int _height;
    private Vector3 _originPosition;

    protected HexGrid _grid;
    protected Transform _tileParent;

    protected int _battleTileNum;
    protected int _encounterTileNum;

    // For test
    //protected const string _testprefabPath = "Map/Area_1/GrassBase";
    private const string _testGridPositionTextPath = "Map/TestPositionText";

    protected const string _gridTilePath = "Map/grid_hex";

    protected AreaGenerator(int width, int height, int battleTileNum, int encounterTileNum, Vector3 originPosition)
    {
        _width = width;
        _height = height;
        _battleTileNum = battleTileNum;
        _encounterTileNum = encounterTileNum;
        _originPosition = originPosition;
        _grid = new HexGrid(_width, _height, _originPosition);
        _tileParent = new GameObject("Tiles").transform;
    }

    // 맵 생성 로직 흐름 관리
    public abstract void GenerateMap();

    // 일반 타일
    protected void GenerateNormalTile()
    {
        for (int z = 0; z < _height; z++)
        {
            for (int x = 0; x < _width; x++)
            {
                if (_grid.IsTileEmpty(x, z))
                {
                    CreateTile(x, z, AreaTileType.Normal);
                }
            }
        }
    }

    protected void GenerateBattleTile()
    {
        int windowSize = 2;
        int[] windowStartArr = new int[_height - windowSize - 2];
        for (int i = 2; i < _height - windowSize; i++)
        {
            windowStartArr[i - 2] = i;
        }
        int count = 0;

        while (count < _battleTileNum)
        {
            // 모든 windowstart가 한번씩 다 선택됐다면 다시 초기화
            if (windowStartArr.Length == 0)
            {
                windowStartArr = new int[_height - windowSize - 1];
                for (int i = 2; i < _height - windowSize; i++)
                {
                    windowStartArr[i - 2] = i;
                }
            } 
            int x = 0, z = 0;
            int trycnt = 0;
            bool selected = false;
            while (!selected)
            {
                if (trycnt == 100)
                {
                    Debug.LogError("Could not select tile position!");
                    break;
                }
                trycnt++;
                // z 좌표: 랜덤으로 선택된 windowstart를 시작으로 windowsize만큼의 범위에서 랜덤 선택
                int windowstart = windowStartArr[Random.Range(0, windowStartArr.Length)];
                z = Random.Range(windowstart, windowstart + windowSize - 1);
                // x 좌표는 그냥 width 범위에서 랜덤
                x = Random.Range(0, _width);
                // 빈 타일이어야 하며, 근처 1칸 범위에 다른 전투 타일이 없어야 함
                if (_grid.IsTileEmpty(x,z) && !_grid.CheckNeighbor(x, z, AreaTileType.Battle)) selected = true;
            }

            CreateTile(x, z, AreaTileType.Battle);
            count++;

            // 한 번 선택된 windowstart는 다시 선택되지 않음 -> 한 곳에 전투 타일이 몰리는 것을 방지
            windowStartArr = windowStartArr.Where(n => n != z).ToArray();
        }
    }

    protected void GenerateEncounterTile()
    {

    }
    
    // 시작 타일: 맨 밑 중간
    protected void GenerateStartTile()
    {
        CreateTile(_width / 2, 0, AreaTileType.Start);
    }
    
    // 보스 타일: 맨 위 중간
    protected void GenerateBossTile()
    {
        CreateTile(_width / 2, _height-1, AreaTileType.Boss);
    }

    protected void InstantiateBaseMap(string path)
    {
        GameObject map = Managers.ResourceMng.Instantiate(path);
        map.transform.position = Vector3.zero;
        _map = map;
        _tileParent.transform.SetParent(_map.transform);
    }

    // for test: 그리드 셀 위에 해당 셀의 좌표 나타내는 텍스트 띄움
    protected void InstantiateGridPositionText(int x, int z)
    {
        GameObject canvas = Managers.ResourceMng.Instantiate(_testGridPositionTextPath, _tileParent);
        canvas.GetComponentInChildren<TextMeshProUGUI>().SetText($"{z}, {x}");
        canvas.transform.position = _grid.GetWorldPosition(x, z, 2);
        canvas.transform.rotation = Quaternion.Euler(60, 0, 0);
    }

    protected void CreateTile(int x, int z, AreaTileType tileType)
    {
        GameObject tile = Managers.ResourceMng.Instantiate(_gridTilePath, _tileParent);
        tile.transform.position = _grid.GetWorldPosition(x, z, 1.02f);
        HexGridCell cell;
        switch (tileType)
        {
            case AreaTileType.Normal:
                cell = new NormalTile(x, z, 1, tile);
                break;
            case AreaTileType.Battle:
                cell = new BattleTile(x, z, 1, tile);
                break;
            default:
                cell = new NormalTile(x, z, 1, tile);
                break;
        }

        _grid.SetGridCell(x, z, cell);
        _grid.SetTileType(x, z, tileType);
        // for test  ////////
        InstantiateGridPositionText(x, z);
        /////////////////////
    }
}
