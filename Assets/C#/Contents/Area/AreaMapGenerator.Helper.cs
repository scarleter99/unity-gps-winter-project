using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public partial class AreaMapGenerator
{
    private const float TILE_WIDTH = 4;
    private const float TILE_HEIGHT = 3.5f;
    private const int MAINTILE_GROUP_GENERATE_END_OFFSET = 10;
    private Vector2Int _nextTileGroupStartPositon;

    // 이웃에서 다음 subtile 배치에 적합한 위치 선택
    private bool SetNextTileGroupStartPosition(int x, int z)
    {
        Vector2Int[] emptyNeighbors =
            GetAllNeighbors(x, z).Where(pos => _map[pos.y, pos.x] == Define.AreaTileType.Empty).ToArray();

        if (emptyNeighbors.Length == 0)
        {
            return false;
        }

        _nextTileGroupStartPositon = emptyNeighbors[Random.Range(0, emptyNeighbors.Length)];
        return true;
    }

    // 그리드 좌표를 월드 좌표로 변환
    private Vector3 GridToWorldPosition(int x, int z, float y = 0)
    {
        if (x % 2 == 1) return new Vector3(x * TILE_WIDTH * 0.75f, y, (z + 0.5f) * TILE_HEIGHT) + _mapOriginPosition;
        else return new Vector3(x * TILE_WIDTH * 0.75f, y, z * TILE_HEIGHT) + _mapOriginPosition;
    }

    // 월드 좌표를 그리드 좌표로 변환
    private void WorldToGridPosition(Vector3 worldPosition, out int x, out int z)
    {
        x = Mathf.RoundToInt((worldPosition.x - (int)_mapOriginPosition.x) / (TILE_WIDTH * 0.75f));
        //float tempz = (worldPosition.z - (int)_mapOriginPosition.z) / TILE_HEIGHT;
        //z = x % 2 == 1 ? Mathf.RoundToInt(tempz - 0.5f) : Mathf.RoundToInt(tempz);
        z = Mathf.RoundToInt((worldPosition.z - (int)_mapOriginPosition.z) / TILE_HEIGHT - (x % 2 == 1 ? 0.5f : 0f));
    }

    private bool IsPositionValid(int x, int z)
    {
        return x >= 0 && x < _data.MapWidth && z >= 0 && z < _data.MapHeight;
    }


    private List<Vector2Int> GetAllNeighbors(int x, int z)
    {
        int[,] dir = x % 2 == 0
            ? new[,] { { 0, 1 }, { 1, 0 }, { 1, -1 }, { 0, -1 }, { -1, -1 }, { -1, 0 } }
            : new[,] { { 0, 1 }, { 1, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 }, { -1, 1 } };
        
        List <Vector2Int> neighbors = new List<Vector2Int>();

        for (int i = 0; i < 6; i++)
        {
            int newx = x + dir[i, 0];
            int newz = z + dir[i, 1];
            if (IsPositionValid(newx, newz))
            {
                neighbors.Add(new Vector2Int(newx, newz));
            }
        }
        return neighbors;
    }

    // TODO: for debug, 타일 정보 나타낼 수 있는 텍스트 띄움
    private void SpawnInfoText(int x, int z, string text, Transform parent)
    {
        GameObject canvas = Instantiate(_infoText, GridToWorldPosition(x, z, 2), Quaternion.Euler(60, 0, 0), parent);
        canvas.GetComponentInChildren<TextMeshProUGUI>().SetText(text);
    }

    // 타일 그룹의 부모 설정
    private Transform SetupTileGroupParent(int x, int z)
    {
        Transform tileGroupParent = new GameObject("TileGroup").transform;
        tileGroupParent.SetParent(_parent);
        tileGroupParent.position = GridToWorldPosition(x, z);
        if (_showInfoText)
        {
            SpawnInfoText(x, z, "Start", tileGroupParent);
        }
        return tileGroupParent;
    }
}
