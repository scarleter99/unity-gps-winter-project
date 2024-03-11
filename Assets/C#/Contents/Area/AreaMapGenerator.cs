using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using Random = UnityEngine.Random;

public partial class AreaMapGenerator : MonoBehaviour
{
    [SerializeField] private Vector3 _mapOriginPosition;
    [SerializeField] private SerializedDictionary<Define.AreaName, AreaMapData> _dataset;

    private AreaMapData _data;
    private Define.AreaTileType[,] _map;
    private AreaBaseTile[,] _tiles;

    private Transform _parent;

    [Header("Debug")]
    [SerializeField, Tooltip("테스트 용으로 생성할 Area")] private Define.AreaName _testAreaName;

    [SerializeField] private bool _showInfoText;
    [SerializeField] private GameObject _infoText;

    public void Init(Define.AreaName area)
    {
        #if UNITY_EDITOR
        area = _testAreaName;
        ClearMap();
        #endif

        _data = _dataset[area];
        _tiles = new AreaBaseTile[_data.MapHeight, _data.MapWidth];
        _map = new Define.AreaTileType[_data.MapHeight, _data.MapWidth];

        for (int i = 0; i < _data.MapHeight; i++)
        {   
            for (int j = 0; j < _data.MapWidth; j++)
            {
                _map[i, j] = Define.AreaTileType.Empty;
            }
        }

        if (!GameObject.Find("@Map"))
        {
            _parent = Managers.ResourceMng.Instantiate("Area/@Map").transform.GetChild(0);
        }
        else
        {
            _parent = GameObject.Find("@Map").transform.GetChild(0);
        }
    }

    public void GenerateMap()
    {
        GenerateSubtiles();
        GenerateMainTile();
    }

    public void GenerateSubtiles()
    {
        AreaSubTileData[] subTileData = _data.SubTileData;
        if (subTileData.Length == 0) return;

        if (subTileData.Sum(x => x.Proportion) > 1)
        {
            Debug.LogError("Sum of subtiles' Proportion is over 1!");
            return;
        }

        // 타일이 맵에 골고루 퍼지도록 하기위해 사용되는 배열: 각 행/열 별로 생성된 타일 수의 합
        // numOfsubtiles[i,j]: i행에 생성된 subtile 수 + j열에 생성된 subtile수
        int[,] numOfSubtiles = new int[_data.MapHeight, _data.MapWidth];

        void IncreaseNumOfSubtiles(int x, int z)
        {
            for (int i = 0; i < _data.MapHeight; i++)
            {
                numOfSubtiles[i, x]++;
            }
            for (int i = 0; i < _data.MapWidth; i++)
            {
                numOfSubtiles[z, i]++;
            }
        }
       
        foreach (AreaSubTileData subtile in subTileData)
        {
            subtile.Init();
            int totalGenerated = 0;

            // Subtile의 정해진 비율에 완벽히 맞추진 않으나 비슷하도록 타일 생성
            while ((float)(totalGenerated) / (_data.MapHeight * _data.MapWidth) < subtile.Proportion)
            {   
                // 생성될 Subtile 그룹의 길이
                int length = Random.Range(subtile.MinLength, subtile.MaxLength);

                // 행/열 합하여 가장 적은 수의 타일이 생성된 위치를 가져옴
                Util.FindMinIndex(numOfSubtiles, out int x, out int z);
                _nextTileGroupStartPositon = new Vector2Int(x, z);

                Transform tileGroupParent = SetupTileGroupParent(x, z);

                for (int i = 0; i < length; i++)
                {
                    x = _nextTileGroupStartPositon.x;
                    z = _nextTileGroupStartPositon.y;
                    Vector3 worldPos = GridToWorldPosition(x, z);

                    AreaBaseTile tile = subtile.SelectRandomTile();
                    Instantiate(tile.Tile, worldPos, Quaternion.identity, tileGroupParent);
                    _tiles[z,x] = tile;
                    _map[z,x] = Define.AreaTileType.SubTile;
                    totalGenerated++;
                    IncreaseNumOfSubtiles(x, z);

                    if (!SetNextTileGroupStartPosition(x, z)) break;
                }
                subtile.OnNextTilegroupStart();
            }
            Debug.Log($"Subtile Proportion: {(float)(totalGenerated) / (_data.MapHeight * _data.MapWidth)}, Target Proportion: {subtile.Proportion}");
        }
    }

    public void GenerateMainTile()
    {
        AreaBaseTileData mainTileData = _data.MainTileData;
        List<Vector2Int> emptyPositions = new List<Vector2Int>();
        for (int z = 0; z < _data.MapHeight; z++)
        {
            for (int x = 0; x < _data.MapWidth; x++)
            {
                if (_map[z, x] == Define.AreaTileType.Empty)
                {
                    emptyPositions.Add(new Vector2Int(x, z));
                }
            }
        }
        mainTileData.Init();

        // 일정 수까지는 그룹으로 생성
        while (emptyPositions.Count > MAINTILE_GROUP_GENERATE_END_OFFSET)
        {
            int length = Random.Range(mainTileData.MinLength, mainTileData.MaxLength);

            // 빈 위치 중 랜덤으로 그룹 시작 위치 설정
            _nextTileGroupStartPositon = emptyPositions[Random.Range(0, emptyPositions.Count)];

            Transform tileGroupParent = SetupTileGroupParent(_nextTileGroupStartPositon.x, _nextTileGroupStartPositon.y);

            for (int i = 0; i < length; i++)
            {
                int x = _nextTileGroupStartPositon.x;
                int z = _nextTileGroupStartPositon.y;
                Vector3 worldPos = GridToWorldPosition(x, z);

                AreaBaseTile tile = mainTileData.SelectRandomTile();
                Instantiate(tile.Tile, worldPos, Quaternion.identity, tileGroupParent);
                _tiles[z, x] = tile;
                _map[z, x] = Define.AreaTileType.MainTile;
                emptyPositions.Remove(_nextTileGroupStartPositon);
                if (!SetNextTileGroupStartPosition(x, z)) break;
            }
            mainTileData.OnNextTilegroupStart();
        }

        // 남은 빈 위치에 생성
        foreach (var pos in emptyPositions)
        {
            int x = pos.x;
            int z = pos.y;
            Vector3 worldPos = GridToWorldPosition(x, z);
            AreaBaseTile tile = mainTileData.SelectRandomTile();
            Instantiate(tile.Tile, worldPos, Quaternion.identity, _parent);
            _tiles[z, x] = tile;
            _map[z, x] = Define.AreaTileType.MainTile;
        }
    }
    private void ClearMap()
    {
        if (GameObject.Find("@Map"))
        {
            Transform parent = GameObject.Find("@Map").transform.GetChild(0);
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                Destroy(parent.GetChild(i).gameObject);
            }
        }
    }
}
