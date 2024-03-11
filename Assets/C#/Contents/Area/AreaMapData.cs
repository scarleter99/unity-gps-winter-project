using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[Serializable]
public class AreaMapData
{
    public int MapWidth;
    public int MapHeight;
    public int PlayableMapWidth;
    public int PlayableMapHeight;
    public AreaBaseTileData MainTileData;
    public AreaSubTileData[] SubTileData;

}

[Serializable]
public class AreaBaseTileData
{
    public AreaBaseTile[] Tiles;
    public int MinLength;
    public int MaxLength;
    private List<AreaBaseTile> _globalAvailableTiles;
    private List<AreaBaseTile> _localAvailableTiles;
    private Dictionary<string, int> _globalCount = new();
    private Dictionary<string, int> _localCount = new();

    public void Init()
    {
        _globalCount = new();
        _localCount = new();
        _globalAvailableTiles = Tiles.ToList();
        _localAvailableTiles = Tiles.ToList();
    }

    public AreaBaseTile SelectRandomTile()
    {
        if (_localAvailableTiles.Count == 0)
        {
            Debug.LogError("No more selectable tile left!");
        }

        AreaBaseTile tile = _localAvailableTiles[Random.Range(0, _localAvailableTiles.Count)];

        Util.IncreaseDictCount(_globalCount, tile.Name);
        Util.IncreaseDictCount(_localCount, tile.Name);
        
        if (tile.HasGlobalLimit && _globalCount[tile.Name] == tile.GlobalLimitCount)
        {
            _globalAvailableTiles.Remove(tile);
            _localAvailableTiles.Remove(tile);
        }
        if (tile.HasLocalLimit && _localCount[tile.Name] == tile.LocalLimitCount)
        {
            _localAvailableTiles.Remove(tile);
        }

        return new AreaBaseTile(tile.Tile);
    }

    public void OnNextTilegroupStart()
    {
        _localCount = new();
        _localAvailableTiles = new List<AreaBaseTile>(_globalAvailableTiles);
    }
}

[Serializable]
public class AreaSubTileData : AreaBaseTileData
{
    [Range(0, 1), Tooltip("Proportion of this subtile in the map")]
    public float Proportion;
}

[Serializable]
public class AreaBaseTile
{
    public GameObject Tile;
    public string Name => Tile.name;
    public bool HasGlobalLimit;
    public bool HasLocalLimit;

    [Header("Only applied when HasLimit is True")]
    public int GlobalLimitCount;
    public int LocalLimitCount;

    public AreaBaseTile(GameObject tile)
    {
        Tile = tile;
    }

    public void EnableDecoration()
    {

    }
}