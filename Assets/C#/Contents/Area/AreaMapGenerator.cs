using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public partial class AreaMapGenerator : MonoBehaviour
{
    [SerializeField] private Vector3 _mapOriginPosition;
    [SerializeField] private SerializedDictionary<Define.AreaName, AreaMapData> _dataset;

    private AreaMapData _data;
    private Define.AreaTileType[,] _map;
    private AreaBaseTile[,] _tiles;

    private Transform _parent;
    private Transform _debugTextParent;
    private Light _light;

    [Header("Debug")]
    [SerializeField, Tooltip("테스트 용으로 생성할 Area")] private Define.AreaName _testAreaName;

    [SerializeField] private bool _showPathToBoss;
    [SerializeField] private GameObject _infoText;
    [SerializeField] private GameObject _pathIndicator;
    [ReadOnly] public MapGeneratePhase CurrentGeneratePhase = MapGeneratePhase.NotStarted; // 에디터상에서 버튼으로 생성 테스트하는 데 사용

    public void Init(Define.AreaName area = Define.AreaName.Forest)
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

        GameObject mapObject = GameObject.Find("@Map") ?? Managers.ResourceMng.Instantiate("Area/@Map");
        GameObject textParentObject = GameObject.Find("@DebugText") ?? new GameObject("@DebugText");
        _parent = mapObject.transform.GetChild(0);
        _debugTextParent = textParentObject.transform;
        _light = GameObject.FindGameObjectWithTag("AreaLight").GetComponent<Light>();
    }

    public void GenerateMap()
    {
        GenerateSubtiles();
        GenerateMainTile();
        SetupPlayableField(out List<Vector2Int> playableField, out List<Vector2Int> unplayableField);
        GenerateUnplayableFieldDecoration(unplayableField);
        GeneratePlayableFieldDecoration(playableField);
    }

    public void GenerateSubtiles()
    {
        AreaSubTileGroupData[] subTileGroupDatas = _data.SubTileGroupData;
        if (subTileGroupDatas.Length == 0) return;

        if (subTileGroupDatas.Sum(x => x.Proportion) > 1)
        {
            Debug.LogError("Sum of subtiles' Proportion is over 1!");
            return;
        }

        CurrentGeneratePhase = MapGeneratePhase.SubtileGenerate;

        // 타일이 맵에 최대한 골고루 퍼지도록 하기위해 사용되는 배열: 각 행/열 별로 생성된 타일 수의 합
        // numOfsubtiles[i,j]: i행에 생성된 subtile 수 + j열에 생성된 subtile수
        int[,] numOfSubtiles = new int[_data.MapHeight, _data.MapWidth];
       
        foreach (AreaSubTileGroupData subTileGroupData in subTileGroupDatas)
        {
            subTileGroupData.Init();
            int totalGenerated = 0;

            // Subtile의 정해진 비율에 완벽히 맞추진 않으나 비슷하도록 타일 생성
            while ((float)(totalGenerated) / (_data.MapHeight * _data.MapWidth) < subTileGroupData.Proportion)
            {   
                // 생성될 Subtile 그룹의 길이
                int length = Random.Range(subTileGroupData.MinLength, subTileGroupData.MaxLength);

                // 행/열 합하여 가장 적은 수의 타일이 생성된 위치를 가져옴
                Util.FindMinIndex(numOfSubtiles, out int x, out int z);
                _nextTilePosition = new Vector2Int(x, z);

                Transform tileGroupParent = SetupTileGroupParent(x, z);

                for (int i = 0; i < length; i++)
                {
                    x = _nextTilePosition.x;
                    z = _nextTilePosition.y;
                    Vector3 worldPos = GridToWorldPosition(x, z);

                    // 랜덤한 타일 선택해서 가져옴
                    AreaBaseTileData tileData = subTileGroupData.SelectRandomTile();
                    AreaBaseTile tile = Instantiate(tileData.Tile, worldPos, Quaternion.identity, tileGroupParent).GetComponent<AreaBaseTile>();

                    _tiles[z, x] = tile;
                    _map[z,x] = Define.AreaTileType.SubTile;
                    totalGenerated++;
                    IncreaseNumOfSubtiles(x, z);

                    if (!SetNextTilePosition(x, z)) break;
                }
                subTileGroupData.OnNextTilegroupStart();
            }
        }
        return;

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
    }

    public void GenerateMainTile()
    {
        CurrentGeneratePhase = MapGeneratePhase.Maintilegenerate;
        AreaTileGroupData mainTileGroupData = _data.MainTileGroupData;
        List<Vector2Int> emptyPositions = new();
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
        mainTileGroupData.Init();

        // 일정 수까지는 그룹으로 생성
        while (emptyPositions.Count > MAINTILE_GROUP_GENERATE_END_OFFSET)
        {
            int length = Random.Range(mainTileGroupData.MinLength, mainTileGroupData.MaxLength);

            // 빈 위치 중 랜덤으로 그룹 시작 위치 설정
            _nextTilePosition = emptyPositions[Random.Range(0, emptyPositions.Count)];

            Transform tileGroupParent = SetupTileGroupParent(_nextTilePosition.x, _nextTilePosition.y);

            for (int i = 0; i < length; i++)
            {
                int x = _nextTilePosition.x;
                int z = _nextTilePosition.y;
                Vector3 worldPos = GridToWorldPosition(x, z);

                AreaBaseTileData tileData = mainTileGroupData.SelectRandomTile();
                AreaBaseTile tile = Instantiate(tileData.Tile, worldPos, Quaternion.identity, tileGroupParent).GetComponent<AreaBaseTile>();
                _tiles[z, x] = tile;
                _map[z, x] = Define.AreaTileType.MainTile;
                emptyPositions.Remove(_nextTilePosition);
                if (!SetNextTilePosition(x, z)) break;
            }
            mainTileGroupData.OnNextTilegroupStart();
        }

        // 남은 빈 위치에 생성
        foreach (var pos in emptyPositions)
        {
            int x = pos.x;
            int z = pos.y;
            Vector3 worldPos = GridToWorldPosition(x, z);

            AreaBaseTileData tileData = mainTileGroupData.SelectRandomTile();
            AreaBaseTile tile = Instantiate(tileData.Tile, worldPos, Quaternion.identity, _parent).GetComponent<AreaBaseTile>();

            _tiles[z, x] = tile;
            _map[z, x] = Define.AreaTileType.MainTile;
        }
    }

    public void SetupPlayableField(out List<Vector2Int> playableField, out List<Vector2Int> unplayableField)
    {
        InitBaseTiles();

        CurrentGeneratePhase = MapGeneratePhase.PlayableFieldSetup;
        _light.cullingMask = LayerMask.GetMask(_lightCullingMask);
  
        var tempPlayablePos = new List<Vector2Int>();
        unplayableField = new List<Vector2Int>();

        int playableAreaHeightStartOffset = _data.MapHeight / 2 - _data.PlayableMapHeight / 2;

        int xStart = _data.MapWidth / 2;
        int zStartOffset = 0, zEndOffset = 0;

        // 육각형꼴의 플레이 가능 필드 생성(설정)
        for (int xOffset = 0; xOffset <= _data.PlayableMapWidth / 2; xOffset++)
        {
            for (int z = playableAreaHeightStartOffset + zStartOffset;
                 z < playableAreaHeightStartOffset + _data.PlayableMapHeight - zEndOffset;
                 z++)
            {   
                SetPlayableTile(xStart + xOffset, z);
                SetPlayableTile(xStart - xOffset, z);
            }

            if ((xStart % 2 == 0 && xOffset % 2 == 0) || (xStart % 2 == 1 && xOffset % 2 == 1))
            {
                zEndOffset++;
            }
            else
            {
                zStartOffset++;
            }
        }

        // 플레이 불가능한 필드의 좌표를 리스트에 추가
        for (int z = 0; z < _data.MapHeight; z++)
        {
            for (int x = 0; x < _data.MapWidth; x++)
            {
                if (_map[z, x] == Define.AreaTileType.Empty)
                    continue;
                unplayableField.Add(new Vector2Int(x, z));
                _map[z, x] = Define.AreaTileType.OutOfField;
            }
        }

        // 시작 타일 및 보스 타일과 그 이웃 타일엔 장애물 배치 안함
        _playerStartPosition = new Vector2Int(xStart, playableAreaHeightStartOffset);
        _bossPosition = new Vector2Int(xStart, playableAreaHeightStartOffset + _data.PlayableMapHeight - 1);
        List<Vector2Int> notDecoratableTiles = new() { _playerStartPosition, _bossPosition };
        notDecoratableTiles.AddRange(GetNeighbors(_playerStartPosition));
        notDecoratableTiles.AddRange(GetNeighbors(_bossPosition));

        foreach (var pos in notDecoratableTiles)
        {
            _map[pos.y, pos.x] = Define.AreaTileType.Unblockable;
        }

        playableField = tempPlayablePos;
        return;

        void SetPlayableTile(int x, int z)
        {
            _tiles[z, x].SetLightTargetLayer();
            _map[z, x] = Define.AreaTileType.Empty;
            tempPlayablePos.Add(new Vector2Int(x, z));
        }
    }

    public void GenerateUnplayableFieldDecoration(List<Vector2Int> unplayableField)
    {
        CurrentGeneratePhase = MapGeneratePhase.UnplayableFieldDecorationGenerate;
        GenerateObstacles(unplayableField, _data.UnplayableFieldDecorationProportion);
    }

    public void GeneratePlayableFieldDecoration(List<Vector2Int> playableField)
    {
        CurrentGeneratePhase = MapGeneratePhase.PlayableFieldDecorationGenerate;

        List<Vector2Int> field;
        List<Vector2Int> path;
        float proportion = _data.PlayableFieldDecorationProportion;
        int trycount = 0;

        do
        {   
            Reset();
            GenerateObstacles(field, proportion);
            trycount++;
            if (trycount % 100 == 0) proportion -= 0.05f;
        } while (!FindShortestPathToBoss(field, out path) && proportion >= 0);

        Debug.Log($"TryCount: {trycount}, Obstacle Proportion: {proportion}, Path Length: {path.Count}");

        if (_showPathToBoss)
        {
            foreach (var pos in path)
            {
                Vector3 position = GridToWorldPosition(pos.x, pos.y, 1.07f);
                Instantiate(_pathIndicator, position, Quaternion.Euler(90, 0, 0), _tiles[pos.y, pos.x].gameObject.transform);
            }
        }

        return;

        void Reset()
        {
            field = new List<Vector2Int>(playableField);
            foreach (var pos in field)
            {
                if (_map[pos.y, pos.x] != Define.AreaTileType.Unblockable)
                    _map[pos.y, pos.x] = Define.AreaTileType.Empty;
                _tiles[pos.y, pos.x].DisableDecoration();
            }
        }

    }

    private void GenerateObstacles(List<Vector2Int> field, float proportion)
    {
        int totalGenerated = 0;
        int totalPosCount = field.Count;

        while ((float)(totalGenerated) / totalPosCount < proportion)
        {
            Vector2Int pos = field[Random.Range(0, field.Count)];
            if (_map[pos.y, pos.x] == Define.AreaTileType.Unblockable) continue;
            _tiles[pos.y, pos.x].EnableDecoration();
            _map[pos.y, pos.x] = Define.AreaTileType.Obstacle;
            totalGenerated++;
            field.Remove(pos);
        }
    }
}
