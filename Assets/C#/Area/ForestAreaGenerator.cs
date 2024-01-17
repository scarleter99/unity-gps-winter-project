using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class ForestAreaGenerator : AreaGenerator
{   
    private const string _areaPrefabPath = "Map/Area_1/Area_1_Map";
    public ForestAreaGenerator(int width, int height, int battleTileNum, int encounterTileNum, Vector3 originPosition) : base(width, height,  battleTileNum,  encounterTileNum, originPosition)
    {
        _areaName = AreaName.Forest;
        _basemap = new[,] {
            { -1, -1, -1, 2, -1, -1, -1 },
            { -1, 1, 1, 1, 1, 0, -1 },
            { 1, 0, 1, 1, 1, 1, 0 },
            { 1, 1, 1, 0, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 0, 1 },
            { 0, 1, 1, 1, 0, 1, 1 },
            { 0, 1, 1, 1, 1, 0, 1 },
            { 1, 1, 0, 0, 1, 1, 1 },
            { 1, 1, 0, 1, 1, 0, 1 },
            { 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 0, 1, 1, 1 },
            { 1, 0, 1, 0, 0, 1, 1 },
            { 1, 0, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 0, 1, 1 },
            { 1, 0, 1, 1, 1, 1, 0 },
            { -1, -1, 1, 3, 1, -1, -1 },
        };
        _grid.InitializeTileTypeArray(_basemap);
    }

    public override void GenerateMap()
    {
        InstantiateBaseMap(_areaPrefabPath);
        GenerateStartTile();
        GenerateBossTile();
        GenerateBattleTile();
        GenerateNormalTile();
    }

    //protected override void GenerateBattleTile()
    //{
    //    int gap = _height / _battleTileNum;
    //}

    //protected override void GenerateEncounterTile()
    //{
    //    throw new System.NotImplementedException();
    //}

    //protected override void GenerateNormalTile()
    //{
    //    for (int z = 0; z < _height; z++)
    //    {
    //        for (int x = 0; x < _width; x++)
    //        {
    //            if (IsValidPosition(x, z) && _grid.GetGridCell(x, z) == null)
    //            {
    //                CreateTile(x, z, AreaTileType.Normal);
    //            }
    //        }
    //    }
    //}
}
